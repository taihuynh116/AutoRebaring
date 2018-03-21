using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
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

            Wall e = doc.GetElement(sel.PickObject(ObjectType.Element)) as Wall;
            Line l = (e.Location as LocationCurve).Curve as Line;
            SketchPlane sp = SketchPlane.Create(doc, Plane.CreateByOriginAndBasis(l.GetEndPoint(0), l.Direction.Normalize(), XYZ.BasisZ));
            ModelCurve mc = doc.Create.NewModelCurve(l, sp);

            tx.Commit();
            return Result.Succeeded;
        }
    }
}
