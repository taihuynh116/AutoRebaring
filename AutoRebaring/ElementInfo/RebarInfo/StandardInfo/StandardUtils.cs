using AutoRebaring.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo
{
    public class StandardUtils
    {
        public static void GetStandardLevelCounts()
        {
            for (int i = 0; i < Singleton.Instance.StandardPlaneInfos.Count; i++)
            {
                IStandardPlaneInfo standPlaneInfo = Singleton.Instance.StandardPlaneInfos[i];
                for (int j = 0; j < standPlaneInfo.NormalStandardPlaneInfos.Count; j++)
                {
                    IStandardPlaneSingleInfo standPlaneSingInfo = standPlaneInfo.NormalStandardPlaneInfos[j];
                    Singleton.Instance.GetStandardLevelCount(i, standPlaneSingInfo)
                        .AddNumber(standPlaneSingInfo.Number);
                }
                for (int j = 0; j < standPlaneInfo.LockheadStandardPlaneInfos.Count; j++)
                {
                    IStandardPlaneSingleInfo standPlaneSingInfo = standPlaneInfo.LockheadStandardPlaneInfos[j];
                    Singleton.Instance.GetStandardLevelCount(i, standPlaneSingInfo)
                        .AddNumber(standPlaneSingInfo.Number);
                }
            }
        }
    }
}
