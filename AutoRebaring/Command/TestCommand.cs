using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using AutoRebaring.ElementInfo;
using AutoRebaring.Form;
using AutoRebaring.RebarLogistic;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.Single;

namespace AutoRebaring.Command
{
    [Transaction(TransactionMode.Manual)]
    public class StirrupRectangle : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            Transaction tx = new Transaction(doc, "AutoRebaring");
            tx.Start();

            ElementInfoUtils.AddElementTypeInfo();
            ElementInfoUtils.PickElement(doc, sel);

            //ElementInfoUtils.AddTestInformationWall(7, 5, 1, 5, 1, 6, 7, 5, 1, 5, 1, 6);
            //ElementInfoUtils.AddTestInformationColumn(10, 8);

            WindowForm Window = new WindowForm();
            Window.SetDimension(1000, 1200, 20, 250, "THÔNG TIN ĐẦU VÀO");
            Window.Form = new InputForm();
            Window.ShowDialog();

            ElementInfoUtils.GetRelatedElements();
            ElementInfoUtils.GetAllParameters();

            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(0);
            for (int i = 0; i < planeInfo.StirrupRebarPointLists.Count; i++)
            {
                List<UV> uvPnts = planeInfo.StirrupRebarPointLists[i];
                for (int j = 0; j < uvPnts.Count; j++)
                {
                    XYZ p0 = new XYZ(uvPnts[j].U, uvPnts[j].V, 0);
                    XYZ p1 = new XYZ(uvPnts[j == uvPnts.Count - 1 ? 0 : j + 1].U, uvPnts[j == uvPnts.Count - 1 ? 0 : j + 1].V, 0);
                    doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p0, p1));
                }
            }

            tx.Commit();
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class DeleteRebar : IExternalCommand
    {
        private WindowForm Window { get; set; }
        const string r = "Revit";
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            Transaction tx = new Transaction(doc, "AutoRebaring");
            tx.Start();

            List<Element> elems = new FilteredElementCollector(doc).OfClass(typeof(Rebar)).ToList();
            List<ElementId> filterElems = new List<ElementId>();
            foreach (var item in elems)
            {
                if (item == null) continue;
                string s = "";
                try
                {
                    s = item.LookupParameter("Comments").AsString();
                }
                catch { }
                if (s == "add-in")
                {
                    filterElems.Add(item.Id);
                }
            }
            doc.Delete(filterElems);

            tx.Commit();
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class SelectColumn : IExternalCommand
    {
        private WindowForm Window { get; set; }
        const string r = "Revit";
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            Transaction tx = new Transaction(doc, "AutoRebaring");
            tx.Start();

            List<ElementId> filElemIds = new List<ElementId>();
            List<Element> elems = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralColumns).ToList();
            foreach (var item in elems)
            {
                string mark = "";
                try
                {
                    mark = item.LookupParameter("Mark").AsString();
                }
                catch { };
                if (mark == "B.C3A")
                {
                    filElemIds.Add(item.Id);
                }
            }
            sel.SetElementIds(filElemIds);

            tx.Commit();
            return Result.Succeeded;
        }
    }
}
