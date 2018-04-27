using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.Constant;
using AutoRebaring.Database;
using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.Single;

namespace AutoRebaring.ElementInfo
{
    public class ColumnDesignInfo :IDesignInfo
    {
        #region IDesignInput
        public int ID { get; set; }
        public Level Level { get; set; }
        public List<RebarBarType> StandardTypes { get; set; }
        public List<double> StandardDiameters { get { return StandardTypes.Select(x => x.BarDiameter).ToList(); } }
        public List<RebarHookType> StandardHookTypes { get; set; }
        public List<int> StandardNumbers { get; set; }
        public List<RebarBarType> StirrupTypes { get; set; }
        public List<double> StirrupDiameters { get { return StirrupTypes.Select(x => x.BarDiameter).ToList(); } }
        public List<double> BotTopSpacings { get; set; }
        public List<double> MiddleSpacings { get; set; }
        public List<double> StandardSpacings { get; set; }
        #endregion
        public ColumnDesignInfo() { }
        public ColumnDesignInfo(Level level, RebarBarType standType, RebarHookType hookType, int n1, int n2, RebarBarType stirType1, RebarBarType stirType2, double bt1, double bt2, double m1, double m2)
        {
            Level = level;
            StandardTypes = new List<RebarBarType> { standType };
            StandardHookTypes = new List<RebarHookType> { hookType };
            StandardNumbers = new List<int> { n1, n2 };
            StirrupTypes = new List<RebarBarType> { stirType1, stirType2 };
            BotTopSpacings = new List<double> { bt1, bt2 };
            MiddleSpacings = new List<double> { m1, m2 };
        }
        public ColumnDesignInfo(Level level, List<RebarBarType> standTypes, List<RebarHookType> hookTypes, List<int> standNumbers, List<RebarBarType> stirrTypes, List<double> btSpacs, List<double> mSpacs)
        {
            Level = level;
            StandardTypes = standTypes;
            StandardHookTypes = hookTypes;
            StandardNumbers = standNumbers;
            StirrupTypes = stirrTypes;
            BotTopSpacings = btSpacs;
            MiddleSpacings = mSpacs;
        }
        public ColumnDesignInfo(IDesignInfo designInfo, Level level)
        {
            Level = level;
            StandardTypes = designInfo.StandardTypes;
            StandardHookTypes = designInfo.StandardHookTypes;
            StandardNumbers = designInfo.StandardNumbers;
            StirrupTypes = designInfo.StirrupTypes;
            BotTopSpacings = designInfo.BotTopSpacings;
            MiddleSpacings = designInfo.MiddleSpacings;
        }
        public void GetStandardSpacing()
        {
            IPlaneInfo pi = Singleton.Instance.GetPlaneInfo(ID);
            ARCoverParameter cp = Singleton.Instance.CoverParameter;

            double standDia = StandardTypes[0].BarDiameter;
            double stirrDia = StirrupTypes[0].BarDiameter;
            double cover = cp.ConcreteCover * ConstantValue.milimeter2Feet * 2 + stirrDia * 2 *ConstantValue.RebarStandardOffsetControl+ standDia;
            StandardSpacings = new List<double>
            {
                (pi.B1s[0] - cover)/(StandardNumbers[0]-1)*2,
                (pi.B2s[0] - cover)/(StandardNumbers[1]-1)*2
            };
        }
    }
    public class WallDesignInfo : IDesignInfo
    {
        #region IDesignInput
        public int ID { get; set; }
        public Level Level { get; set; }
        public List<RebarBarType> StandardTypes { get; set; }
        public List<double> StandardDiameters { get; set; }
        public List<int> StandardNumbers { get; set; }
        public List<RebarHookType> StandardHookTypes { get; set; }
        public List<RebarBarType> StirrupTypes { get; set; }
        public List<double> StirrupDiameters { get; set; }
        public List<double> BotTopSpacings { get; set; }
        public List<double> MiddleSpacings { get; set; }
        public List<double> StandardSpacings { get; set; }
        #endregion

        public WallDesignInfo(Level level, RebarBarType edgeStandType,RebarHookType edgeHookType, RebarBarType midStandType, RebarHookType middleHookType, int ne11, int ne12, int ce12, int ne2, int de2, int nm, RebarBarType stirType1, RebarBarType stirType2, RebarBarType stirType3,
            double bt1, double bt2, double m1, double m2)
        {
            Level = level;
            StandardTypes = new List<RebarBarType> { edgeStandType, midStandType };
            StandardHookTypes = new List<RebarHookType> { edgeHookType, middleHookType };
            StandardNumbers = new List<int> { ne11, ne12, ce12, ne2,de2, nm };
            StirrupTypes = new List<RebarBarType> { stirType1, stirType2, stirType3 };
            BotTopSpacings = new List<double> { bt1, bt2 };
            MiddleSpacings = new List<double> { m1, m2 };
            StandardDiameters = StandardTypes.Select(x => x.BarDiameter).ToList();
            StirrupDiameters = StirrupTypes.Select(x => x.BarDiameter).ToList();
        }
        public WallDesignInfo(Level level, List<RebarBarType> standTypes, List<RebarHookType> hookTypes, List<int> standNumbers, List<RebarBarType> stirrTypes, List<double> btSpacs, List<double> mSpacs)
        {
            Level = level;
            StandardTypes = standTypes;
            StandardHookTypes = hookTypes;
            StandardNumbers = standNumbers;
            StirrupTypes = stirrTypes;
            BotTopSpacings = btSpacs;
            MiddleSpacings = mSpacs;
            StandardDiameters = StandardTypes.Select(x => x.BarDiameter).ToList();
            StirrupDiameters = StirrupTypes.Select(x => x.BarDiameter).ToList();
        }
        public WallDesignInfo(IDesignInfo designInfo, Level level)
        {
            Level = level;
            StandardTypes = designInfo.StandardTypes;
            StandardHookTypes = designInfo.StandardHookTypes;
            StandardNumbers = designInfo.StandardNumbers;
            StirrupTypes = designInfo.StirrupTypes;
            BotTopSpacings = designInfo.BotTopSpacings;
            MiddleSpacings = designInfo.MiddleSpacings;
            StandardDiameters = designInfo.StandardDiameters;
            StirrupDiameters = designInfo.StirrupDiameters;
        }
        public void GetStandardSpacing()
        {
            IPlaneInfo pi = Singleton.Instance.GetPlaneInfo(ID);
            ARCoverParameter cp = Singleton.Instance.CoverParameter;

            double edgeStandDia = StandardTypes[0].BarDiameter;
            double middleStandDia = StandardTypes[1].BarDiameter;
            double stirrDia = StirrupTypes[0].BarDiameter;
            double cover = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            StandardSpacings = new List<double>
            {
                (pi.B1s[0] - cover- stirrDia*1.5-edgeStandDia)/(StandardNumbers[0]-1)*2,
                StandardNumbers[1]==0?0:(pi.B1s[0] - cover- stirrDia*2-edgeStandDia)/(StandardNumbers[1]-1)*2,
                (pi.B2s[0] - cover- stirrDia*1.5-edgeStandDia)/(StandardNumbers[3]-1)*2,
                (pi.B1s[1]+stirrDia+ middleStandDia)/(StandardNumbers[4]+1)
            };
        }
    }
}
