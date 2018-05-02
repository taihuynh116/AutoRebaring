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

            ElementInfoUtils.AddTestInformationWall(7, 5, 1, 5, 1, 6, 7, 5, 1, 5, 1, 6);
            //Window = new WindowForm();
            //Window.SetDimension(1000, 1200, 20, 250, "THÔNG TIN ĐẦU VÀO");
            //Window.Form = new InputForm();
            //Window.ShowDialog();

            ElementInfoUtils.GetRelatedElements();
            ElementInfoUtils.GetAllParameters();
            ElementInfoUtils.GetVariable();

            int locCount = Singleton.Instance.GetElementTypeInfo().LocationCount;
            for (int i = 0; i < locCount; i++)
            {
                StandardLogistic stanLog = new StandardLogistic(i);
                stanLog.RunLogistic();
            }

            for (int i = 0; i < locCount; i++)
            {
                for (int j = 0; j < Singleton.Instance.GetStandardTurnCount(0); j++)
                {
                    StandardTurn st = Singleton.Instance.GetStandardTurn(j, 0);
                    IStandardPlaneInfo standPlaneInfo = Singleton.Instance.GetStandardPlaneInfo(st.IDElement);
                    IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(st.IDElement);
                    IShortenType shortenType = null;
                    shortenType = planeInfo.ShortenTypes[0];
                    standPlaneInfo.CreateRebar(j, i);
                }
            }

            var instance = Singleton.Instance;

            tx.Commit();
            return Result.Succeeded;
        }
    }
}
