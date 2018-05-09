using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace AutoRebaring.ElementInfo.RebarInfo.StirrupInfo.SingleInfo
{
    public class StirrupID
    {
        public static int ID = 0;
    }
    public class StirrupPlaneSingleInfo : IStirrupPlaneSingleInfo
    {
        public int ID { get; set; }
        public UV StartPoint { get; set; }
        public XYZ VectorX { get; set; }
        public XYZ VectorY { get; set; }
        public List<object> ParameterValues { get; set; }
        public StirrupPlaneSingleInfo()
        {
            ID = StirrupID.ID;
            StirrupID.ID++;
        }
        public Rebar CreateRebar(int idElem, StirrupDistribution stirDis)
        {
            return null;
        }
    }
}
