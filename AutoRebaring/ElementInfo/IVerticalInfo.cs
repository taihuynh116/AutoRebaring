using Autodesk.Revit.DB;
using AutoRebaring.Database;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public interface IVerticalInfo
    {
        Level StartLevel { get; set; }
        Level EndLevel { get; set; }
        double Top { get; }
        double TopBeam { get; set; }
        double TopFloor { get; set; }
        double TopOffset { get; }
        double TopLimit { get; set; }
        double TopLockHead { get; }
        double TopSmall { get; }
        List<double> TopAnchorAfters { get; set; }
        double TopStirrup1 { get; }
        double TopStirrup2 { get; }
        double Bottom { get; set; }
        double BottomOffset { get;}
        double BottomStirrup1 { get; }
        double BottomStirrup2 { get; }
        double BottomOffsetValue { get;}
        double TopOffsetValue { get;  }
        double BottomOffsetValueStirrup { get; }
        double TopOffsetValueStirrup { get; }
        List<double> RebarDevelopmentLengths { get; set; }
        List<StirrupDistribution> StirrupDistributions { get; set; }
    }
    public class VerticalInfo:IVerticalInfo
    {
        public DevelopmentRebar DevelopmentRebar { get; set; }
        public GeneralParameterInput GeneralParameterInput { get; set; }
        public List<IStandardRebarInfo> StandardRebarInfos { get; set; }
        public Level StartLevel { get; set; }
        public Level EndLevel { get; set; }
        public double Top { get { return DevelopmentRebar.IsInsideBeam ? TopFloor : TopBeam; } }
        public double TopBeam { get; set; }
        public double TopFloor { get; set; }
        public double TopOffset { get { return Top - TopOffsetValue; } }
        public double TopLimit { get; set; }
        public double TopLockHead { get { return Top - GeomUtil.milimeter2Feet(GeneralParameterInput.ConcreteTopCover); } }
        public double TopSmall { get { return Top - GeomUtil.milimeter2Feet(GeneralParameterInput.CoverTopSmall); } }
        public List<double> TopAnchorAfters { get; set; }
        public double TopStirrup1 { get { return TopStirrup2 - TopOffsetValueStirrup; } }
        public double TopStirrup2 { get { return DevelopmentRebar.IsStirrupInsideBeam ? TopFloor : TopBeam; } }
        public double Bottom { get; set; }
        public double BottomOffset { get { return Bottom + BottomOffsetValue; } }
        public double BottomStirrup1 { get { return Bottom; } }
        public double BottomStirrup2 { get { return BottomStirrup1 + BottomOffsetValueStirrup; } }
        public double BottomOffsetValue
        {
            get
            {
                double d = 0;
                if (DevelopmentRebar.OffsetInclude) d = GeomUtil.milimeter2Feet(DevelopmentRebar.BottomOffset);
                if (DevelopmentRebar.OffsetRatioInclude) d = Math.Max(d, (Top - Bottom) / DevelopmentRebar.BottomOffsetRatio);
                return d;
            }
        }
        public double TopOffsetValue
        {
            get
            {
                double d = 0;
                if (DevelopmentRebar.OffsetInclude) d = GeomUtil.milimeter2Feet(DevelopmentRebar.TopOffset);
                if (DevelopmentRebar.OffsetRatioInclude) d = Math.Max(d, (Top - Bottom) / DevelopmentRebar.TopOffsetRatio);
                return d;
            }
        }
        public double BottomOffsetValueStirrup
        {
            get
            {
                double d = 0;
                if (DevelopmentRebar.StirrupOffsetInclude) d = GeomUtil.milimeter2Feet(DevelopmentRebar.BottomStirrupOffset);
                if (DevelopmentRebar.StirrupOffsetRatioInclude) d = Math.Max(d, (TopStirrup2 - BottomStirrup1) / DevelopmentRebar.BottomStirrupOffsetRatio);
                return d;
            }
        }
        public double TopOffsetValueStirrup
        {
            get
            {
                double d = 0;
                if (DevelopmentRebar.StirrupOffsetInclude) d = GeomUtil.milimeter2Feet(DevelopmentRebar.TopStirrupOffset);
                if (DevelopmentRebar.StirrupOffsetRatioInclude) d = Math.Max(d, (TopStirrup2 - BottomStirrup1) / DevelopmentRebar.TopStirrupOffsetRatio);
                return d;
            }
        }
        public List<double> RebarDevelopmentLengths { get; set; }
        public List<StirrupDistribution> StirrupDistributions { get; set; }
    }

    public class ColumnVerticalInfo : VerticalInfo
    {
        public double TopAnchorAfter { get {return TopAnchorAfters[0]; } }
        public double RebarDevelopmentLength { get { return RebarDevelopmentLengths[0]; } }
    }
    public class WallVerticalInfo:VerticalInfo
    {

    }
}
