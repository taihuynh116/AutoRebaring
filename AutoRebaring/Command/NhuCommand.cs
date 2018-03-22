using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using AutoRebaring.Constant;
using AutoRebaring.Database;
using AutoRebaring.ElementInfo;
using AutoRebaring.ElementInfo.Shorten;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Command
{
    [Transaction(TransactionMode.Manual)]
    public class NhuCommand : IExternalCommand
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

            List<Element> elems = sel.PickElementsByRectangle() as List<Element>;
            List<Element> sortElems = new List<Element>();

            Element e1 = elems[0];
            XYZ p1 = (e1.Location as LocationPoint).Point;
            Element e2 = elems[1];
            XYZ p2 = (e2.Location as LocationPoint).Point;

            if (p1.Z < p2.Z)
            {
                sortElems = new List<Element> { e1, e2 };
            }
            else
            {
                sortElems = new List<Element> { e2, e1 };
            }

            List<ColumnPlaneInfo> cpis = new List<ColumnPlaneInfo>();
            for (int i = 0; i < sortElems.Count; i++)
            {
                IRevitInfo iri = new RevitInfo(doc, sortElems[i]);
                ColumnParameter cp = new ColumnParameter()
                {
                    B1_Param = "b",
                    B2_Param = "h"
                };
                GeneralParameterInput gpi = new GeneralParameterInput()
                {
                    ShortenLimit = ConstantValue.milimeter2Feet * 100
                };
                ColumnPlaneInfo cpi = new ColumnPlaneInfo(iri, cp);
                cpis.Add(cpi);
            }
            for (int i = 0; i < cpis.Count-1; i++)
            {
                cpis[i].CPIAfter = cpis[i + 1];
            }

            tx.Commit();
            return Result.Succeeded;
        }
    }
}
