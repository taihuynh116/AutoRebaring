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

namespace AutoRebaring.Command
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
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

            InputForm inputForm = Window.Form;
            ElementInfoCollection elemInfoCol = new ElementInfoCollection(inputForm);

            List<StandardLogistic> stanLogs = new List<StandardLogistic>();
            for (int i = 0; i < elemInfoCol[0].LocationCount; i++)
            {
                StandardLogistic stanLog = new StandardLogistic(elemInfoCol, inputForm, i);
                stanLog.RunLogistic();
                stanLogs.Add(stanLog);
            }


            tx.Commit();
            return Result.Succeeded;
        }
    }
}
