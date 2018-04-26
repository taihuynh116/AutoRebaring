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
}
