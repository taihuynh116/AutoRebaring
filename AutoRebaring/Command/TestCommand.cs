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
using AutoRebaring.Constant;
using AutoRebaring.ElementInfo.RebarInfo.StirrupInfo;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;

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

    [Transaction(TransactionMode.Manual)]
    public class ReadPoint : IExternalCommand
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

            Element elem = doc.GetElement(sel.PickObject(ObjectType.Element));
            RebarShape rebarShape = new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Where(x => x.Name == "TD_U_02").Cast<RebarShape>().First();
            RebarBarType rebarType = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Where(x => x.Name == "T10").Cast<RebarBarType>().First();

            XYZ pnt = sel.PickPoint() + (XYZ.BasisX + XYZ.BasisY) * ConstantValue.milimeter2Feet * 35;
            Rebar rebar= Rebar.CreateFromRebarShape(doc, rebarShape, rebarType, elem, pnt, XYZ.BasisX, XYZ.BasisY);
            doc.Regenerate();
            rebar.LookupParameter("B").Set(ConstantValue.milimeter2Feet * (1000/2 - 35+ 10 * 20));
            rebar.LookupParameter("C").Set(ConstantValue.milimeter2Feet * (3000 - 35*2));
            rebar.LookupParameter("D").Set(ConstantValue.milimeter2Feet * (1000 / 2 - 35 + 10 * 20));

            RebarShapeDrivenAccessor rsda = rebar.GetShapeDrivenAccessor();
            rsda.SetLayoutAsNumberWithSpacing(30, ConstantValue.milimeter2Feet * 100, true, true, true);

            pnt = sel.PickPoint() + (-XYZ.BasisX - XYZ.BasisY) * ConstantValue.milimeter2Feet * 35;
            rebar = Rebar.CreateFromRebarShape(doc, rebarShape, rebarType, elem, pnt, -XYZ.BasisX, -XYZ.BasisY);
            doc.Regenerate();
            rebar.LookupParameter("B").Set(ConstantValue.milimeter2Feet * (1000 / 2 - 35 + 10 * 20));
            rebar.LookupParameter("C").Set(ConstantValue.milimeter2Feet * (3000 - 35 * 2));
            rebar.LookupParameter("D").Set(ConstantValue.milimeter2Feet * (1000 / 2 - 35  + 10 * 20));

            rsda = rebar.GetShapeDrivenAccessor();
            rsda.SetLayoutAsNumberWithSpacing(30, ConstantValue.milimeter2Feet * 100, true, true, true);

            rebar.SetUnobscuredInView(doc.ActiveView, true);

            tx.Commit();
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CreatePolygonPoint : IExternalCommand
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

            ElementInfoUtils.AddElementTypeInfo();
            ElementInfoUtils.PickElement(doc, sel);

            ElementInfoUtils.AddTestInformationWall(7, 5, 1, 5, 1, 6, 7, 5, 1, 5, 1, 6);
            //ElementInfoUtils.AddTestInformationColumn(10, 8);

            //Window = new WindowForm();
            //Window.SetDimension(1000, 1200, 20, 250, "THÔNG TIN ĐẦU VÀO");
            //Window.Form = new InputForm();
            //Window.ShowDialog();

            ElementInfoUtils.GetRelatedElements();
            ElementInfoUtils.GetAllParameters();

            foreach (List<UV> boundary in Singleton.Instance.GetPlaneInfo(0).StirrupRebarPointLists)
            {
                for (int i = 0; i < boundary.Count; i++)
                {
                    XYZ p1 = new XYZ(boundary[i].U, boundary[i].V, 0);
                    int j = (i == boundary.Count - 1) ? 0 : i + 1;
                    XYZ p2 = new XYZ(boundary[j].U, boundary[j].V, 0);
                    doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p1, p2));
                }
                break;
            }

            tx.Commit();
            return Result.Succeeded;
        }
    }
}
