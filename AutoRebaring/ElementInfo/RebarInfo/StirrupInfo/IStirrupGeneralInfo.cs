using Autodesk.Revit.DB.Structure;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StirrupInfo
{
    public class StirrupGeneralInfo : IRebarGeneralInfo
    {
        public RebarLayoutRule LayoutRule { get; set; }
        public RebarHookType HookType { get; set; }
        public List<double> Diameters { get; set; }
        public List<double> DiameterAfters { get; set; }
        public List<double> DiameterBefores { get; set; }
        public List<RebarBarType> Types { get; set; }
        public List<RebarBarType> TypeAfters { get; set; }
        public List<RebarBarType> TypeBefores { get; set; }
        public RebarStyle Style { get; set; }
    }
}
