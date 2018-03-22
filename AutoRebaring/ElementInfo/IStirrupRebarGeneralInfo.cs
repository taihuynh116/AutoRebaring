using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class StirrupRebarSingleGeneralInfo
    {
        public RebarBarType StirrupType { get; set; }
        public List<string> ParameterNames { get; set; }
        public List<string> ParameterValues { get; set; }
    }
    public interface IStirrupRebarGeneralInfo
    {
        List<StirrupRebarSingleGeneralInfo> StirrupRebarSingleGeneralInfos { get; set; }
    }

    public class ColumnStirrupRebarGeneralInfo : IStirrupRebarGeneralInfo
    {
        public List<StirrupRebarSingleGeneralInfo> StirrupRebarSingleGeneralInfos { get; set; }
    }
    public class WallStirrupRebarGeneralInfo : IStirrupRebarGeneralInfo
    {
        public List<StirrupRebarSingleGeneralInfo> StirrupRebarSingleGeneralInfos { get; set; }
    }
}
