using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public interface IStandardRebarGeneralInfo
    {
        List<double> Diameters { get; set; }
        List<double> DiameterAfters { get; set; }
        List<double> DiameterBefores { get; set; }
        List<RebarBarType> Types { get; set; }
        List<RebarBarType> TypeAfters { get; set; }
        List<RebarBarType> TypeBefores { get; set; }
    }
    public class ColumnRebarGeneralInfo : IStandardRebarGeneralInfo
    {
        public List<double> Diameters { get; set; }
        public List<double> DiameterAfters { get; set; }
        public List<double> DiameterBefores { get; set; }
        public List<RebarBarType> Types { get; set; }
        public List<RebarBarType> TypeAfters { get; set; }
        public List<RebarBarType> TypeBefores { get; set; }
    }
    public class WallRebarGeneralInfo : IStandardRebarGeneralInfo
    {
        public List<double> Diameters { get; set; }
        public List<double> DiameterAfters { get; set; }
        public List<double> DiameterBefores { get; set; }
        public List<RebarBarType> Types { get; set; }
        public List<RebarBarType> TypeAfters { get; set; }
        public List<RebarBarType> TypeBefores { get; set; }
    }
}
