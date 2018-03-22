using AutoRebaring.Database;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo.SingleInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo
{
    public interface IStandardPlaneInfo
    {
        List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        List<IStandardPlaneSingleInfo> ShortenStandardPlaneInfos { get; set; }
        List<IStandardPlaneSingleInfo> ImplantStandardPlaneInfos { get; set; }
    }
    public class ColumnStandardPlaneInfo : IStandardPlaneInfo
    {
        public IPlaneInfo PlaneInfo { get; set; }
        public IDesignInfo DesignInfo { get; set; }
        public GeneralParameterInput GeneralParameterInput { get; set; }
        public IStandardGeneralInfo StandardGeneralInfo { get; set; }
        public List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ShortenStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ImplantStandardPlaneInfos { get; set; }
        public ColumnStandardPlaneInfo(IPlaneInfo planeInfo, IDesignInfo designInfo, IStandardGeneralInfo standGenInfo)
        {
            PlaneInfo = planeInfo; DesignInfo = designInfo; StandardGeneralInfo = standGenInfo;
        }
        private void GetNormalStandardPlaneInfos()
        {
            double spacB1 = (PlaneInfo.B1s[0] - GeneralParameterInput.ConcreteCover *2  )
        }
    }
    public class WallStandardPlaneInfo : IStandardPlaneInfo
    {
        public List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ShortenStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ImplantStandardPlaneInfos { get; set; }
    }
}
