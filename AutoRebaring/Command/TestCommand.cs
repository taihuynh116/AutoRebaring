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
}
