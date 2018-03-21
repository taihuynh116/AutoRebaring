using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class IStandardRebarInfo
    {
        public double Diameter { get; set; }
        public double DiameterAfter { get; set; }
        public double DiameterBefore { get; set; }
        public RebarBarType Type { get; set; }
        public RebarBarType TypeAfter { get; set; }
        public RebarBarType TypeBefore { get; set; }
    }

    public class IStirrupRebarInfo
    {
        public RebarBarType StirrupType { get; set; }
        public List<string> ParameterNames { get; set; }
        public List<double> ParameterValues { get; set; }
    }
}
