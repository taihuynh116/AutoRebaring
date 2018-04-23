using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using AutoRebaring.ElementInfo;
using AutoRebaring.Form;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Command
{
    [Transaction(TransactionMode.Manual)]
    public class Test : IExternalCommand
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

            Element e = doc.GetElement(sel.PickObject(ObjectType.Element, new WallAndColumnSelection()));

            //IRevitInfo revitInfo = new RevitInfo(doc, e);
            //IVerticalInfo  verticalInfo= new VerticalInfo(revitInfo);


            tx.Commit();
            return Result.Succeeded;
        }
    }
}
