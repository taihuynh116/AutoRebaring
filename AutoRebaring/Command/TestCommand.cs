using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using AutoRebaring.ElementInfo;
using AutoRebaring.Form;
using AutoRebaring.Single;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Command
{
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

            List<Element> elems = new FilteredElementCollector(doc).OfClass(typeof(Rebar)).Where(x => x != null).ToList();
            List<Element> filterElems = new List<Element>();
            foreach (var item in elems)
            {
                string s = "";
                try {
                    s = item.LookupParameter("Comments").AsString();
                }
                catch { }
                if (s == "add-in")
                {
                    filterElems.Add(item);
                }
            }
            doc.Delete(filterElems.Select(x => x.Id).ToList());

            tx.Commit();
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class CreateDetail : IExternalCommand
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

            Window = new WindowForm();
            Window.SetDimension(1000, 1200, 20, 250, "THÔNG TIN ĐẦU VÀO");
            Window.Form = new InputForm();
            Window.ShowDialog();

            ElementInfoUtils.GetRelatedElements();
            ElementInfoUtils.GetAllParameters();

            doc.ActiveView.SketchPlane = SketchPlane.Create(doc, Plane.CreateByOriginAndBasis(doc.ActiveView.Origin, doc.ActiveView.RightDirection, doc.ActiveView.UpDirection));

            XYZ p1 = sel.PickPoint(), p2 = sel.PickPoint();
            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                IVerticalInfo verticalInfo = Singleton.Instance.GetVerticalInfo(i);
                XYZ p11 = null, p12 = null;
                if (i % 2 == 0)
                {
                    p11 = new XYZ(p1.X, p1.Y, verticalInfo.BottomOffset);
                    p12 = new XYZ(p1.X, p1.Y, verticalInfo.TopOffset);
                }
                else
                {
                    p11 = new XYZ(p2.X, p2.Y, verticalInfo.BottomOffset);
                    p12 = new XYZ(p2.X, p2.Y, verticalInfo.TopOffset);
                }
                DetailCurve dl = doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p11, p12));
            }

            tx.Commit();
            return Result.Succeeded;
        }
    }
}
