using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class ColumnDesignInfo 
    {
        #region IDesignInput
        public Level Level { get; set; }
        public List<RebarBarType> StandardTypes { get; set; }
        public List<int> StandardNumbers { get; set; }
        public List<RebarBarType> StirrupTypes { get; set; }
        public List<double> BotTopSpacings { get; set; }
        public List<double> MiddleSpacings { get; set; }
        #endregion

        public RebarBarType StandardType { get { return StandardTypes[0]; } }
        public int N1 { get { return StandardNumbers[0]; } }
        public int N2 { get { return StandardNumbers[1]; } }
        public RebarBarType StirrupType1 { get { return StirrupTypes[0]; } }
        public RebarBarType StirrupType2 { get { return StirrupTypes[1]; } }
        public double BotTopSpacing1 { get { return BotTopSpacings[0]; } }
        public double BotTopSpacing2 { get { return BotTopSpacings[1]; } }
        public double MiddleSpacing1 { get { return MiddleSpacings[0]; } }
        public double MiddleSpacing2 { get { return MiddleSpacings[1]; } }

        public ColumnDesignInfo(Level level, RebarBarType standType, int n1, int n2, RebarBarType stirType1, RebarBarType stirType2, double bt1, double bt2, double m1, double m2)
        {
            Level = level;
            StandardTypes = new List<RebarBarType> { standType };
            StandardNumbers = new List<int> { n1, n2 };
            StirrupTypes = new List<RebarBarType> { stirType1, stirType2 };
            BotTopSpacings = new List<double> { bt1, bt2 };
            MiddleSpacings = new List<double> { m1, m2 };
        }
    }
    public class WallDesignInfo
    {
        #region IDesignInput
        public Level Level { get; set; }
        public List<RebarBarType> StandardTypes { get; set; }
        public List<int> StandardNumbers { get; set; }
        public List<RebarBarType> StirrupTypes { get; set; }
        public List<double> BotTopSpacings { get; set; }
        public List<double> MiddleSpacings { get; set; }
        #endregion

        public RebarBarType EdgeStandardType { get { return StandardTypes[0]; } }
        public RebarBarType MiddleStandardType { get { return StandardTypes[1]; } }
        public int NE11 { get { return StandardNumbers[0]; } }
        public int NE12 { get { return StandardNumbers[1]; } }
        public int CE12 { get { return StandardNumbers[2]; } }
        public int NE2 { get { return StandardNumbers[3]; } }
        public bool IsDoubleNE2 { get; set; }
        public int NM { get { return StandardNumbers[4]; } }
        public RebarBarType StirrupType1 { get { return StirrupTypes[0]; } }
        public RebarBarType StirrupType2 { get { return StirrupTypes[1]; } }
        public RebarBarType StirrupType3 { get { return StirrupTypes[2]; } }
        public double BotTopSpacing1 { get { return BotTopSpacings[0]; } }
        public double BotTopSpacing2 { get { return BotTopSpacings[1]; } }
        public double MiddleSpacing1 { get { return MiddleSpacings[0]; } }
        public double MiddleSpacing2 { get { return MiddleSpacings[1]; } }

        public WallDesignInfo(Level level, RebarBarType edgeStandType, RebarBarType midStandType, int ne11, int ne12, int ce12, int ne2, bool isDouNe2, int nm, RebarBarType stirType1, RebarBarType stirType2, RebarBarType stirType3,
            double bt1, double bt2, double m1, double m2)
        {
            Level = level;
            StandardTypes = new List<RebarBarType> { edgeStandType, midStandType };
            StandardNumbers = new List<int> { ne11, ne12, ce12, ne2, nm };
            IsDoubleNE2 = isDouNe2;
            StirrupTypes = new List<RebarBarType> { stirType1, stirType2, stirType3 };
            BotTopSpacings = new List<double> { bt1, bt2 };
            MiddleSpacings = new List<double> { m1, m2 };
        }
    }
}
