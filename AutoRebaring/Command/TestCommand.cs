using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using AutoRebaring.ElementInfo;
using AutoRebaring.Form;
using AutoRebaring.RebarLogistic;
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
                try
                {
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

    [Transaction(TransactionMode.Manual)]
    public class TestColumn : IExternalCommand
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

            ElementInfoUtils.AddTestInformationColumn(6, 10);
            //Window = new WindowForm();
            //Window.SetDimension(1000, 1200, 20, 250, "THÔNG TIN ĐẦU VÀO");
            //Window.Form = new InputForm();
            //Window.ShowDialog();

            ElementInfoUtils.GetRelatedElements();
            ElementInfoUtils.GetAllParameters();
            ElementInfoUtils.GetVariable();

            doc.ActiveView.SketchPlane = SketchPlane.Create(doc, Plane.CreateByOriginAndBasis(doc.ActiveView.Origin, doc.ActiveView.RightDirection, doc.ActiveView.UpDirection));
            IPlaneInfo pi = Singleton.Instance.GetPlaneInfo(0);
            //foreach (List<UV> pnts in pi.StandardRebarPointLists)
            //{
            List<UV> pnts = pi.StandardRebarPointLists[2];
            XYZ p0 = new XYZ(pnts[0].U, pnts[0].V, 0);
            XYZ p1 = new XYZ(pnts[1].U, pnts[1].V, 0);
            XYZ p2 = new XYZ(pnts[2].U, pnts[2].V, 0);
            XYZ p3 = new XYZ(pnts[3].U, pnts[3].V, 0);
            doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p0, p1));
            doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p1, p2));
            doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p2, p3));
            doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p3, p0));
            //}

            tx.Commit();
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class TestWall : IExternalCommand
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

            ElementInfoUtils.AddTestInformationWall(5, 3, 1, 5, 0, 6, 5, 3, 1, 5, 0, 6);
            //Window = new WindowForm();
            //Window.SetDimension(1000, 1200, 20, 250, "THÔNG TIN ĐẦU VÀO");
            //Window.Form = new InputForm();
            //Window.ShowDialog();

            ElementInfoUtils.GetRelatedElements();
            ElementInfoUtils.GetAllParameters();
            ElementInfoUtils.GetVariable();

            doc.ActiveView.SketchPlane = SketchPlane.Create(doc, Plane.CreateByOriginAndBasis(doc.ActiveView.Origin, doc.ActiveView.RightDirection, doc.ActiveView.UpDirection));
            IPlaneInfo pi = Singleton.Instance.GetPlaneInfo(0);

            //foreach (List<UV> pnts in pi.BoundaryPointLists)
            //{
            //    //List<UV> pnts = pi.StandardRebarPointLists[2];

            //    XYZ p0 = new XYZ(pnts[0].U, pnts[0].V, 0);
            //    XYZ p1 = new XYZ(pnts[1].U, pnts[1].V, 0);
            //    XYZ p2 = new XYZ(pnts[2].U, pnts[2].V, 0);
            //    XYZ p3 = new XYZ(pnts[3].U, pnts[3].V, 0);
            //    doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p0, p1));
            //    doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p1, p2));
            //    doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p2, p3));
            //    doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p3, p0));
            //}

            foreach (List<UV> pnts in pi.StandardRebarPointLists)
            {
                //List<UV> pnts = pi.StandardRebarPointLists[2];

                XYZ p0 = new XYZ(pnts[0].U, pnts[0].V, 0);
                XYZ p1 = new XYZ(pnts[1].U, pnts[1].V, 0);
                XYZ p2 = new XYZ(pnts[2].U, pnts[2].V, 0);
                XYZ p3 = new XYZ(pnts[3].U, pnts[3].V, 0);
                doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p0, p1));
                doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p1, p2));
                doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p2, p3));
                doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(p3, p0));
            }

            tx.Commit();
            return Result.Succeeded;
        }
    }

}
