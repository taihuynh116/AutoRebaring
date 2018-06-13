using AutoRebaring.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StirrupInfo
{
    public class StirrupUtils
    {
        public static void GetStirrupLevelCounts()
        {
            for (int i = 0; i < Singleton.Instance.StirrupPlaneInfos.Count; i++)
            {
                IStirrupPlaneInfo stirPlaneInfo = Singleton.Instance.StirrupPlaneInfos[i];
                for (int j = 0; j < stirPlaneInfo.EdgeCoverStirrupPlaneInfo.Count; j++)
                {
                    IStirrupPlaneSingleInfo stirPlaneSingInfo = stirPlaneInfo.EdgeCoverStirrupPlaneInfo[j];
                    Singleton.Instance.GetStirrupLevelCount(i, stirPlaneSingInfo)
                        .AddNumber(1);
                }
                for (int j = 0; j < stirPlaneInfo.CoverStirrupPlaneInfos.Count; j++)
                {
                    IStirrupPlaneSingleInfo stirPlaneSingInfo = stirPlaneInfo.CoverStirrupPlaneInfos[j];
                    Singleton.Instance.GetStirrupLevelCount(i, stirPlaneSingInfo)
                        .AddNumber(1);
                }
                for (int j = 0; j < stirPlaneInfo.CStirrupPlaneInfos.Count; j++)
                {
                    IStirrupPlaneSingleInfo stirPlaneSingInfo = stirPlaneInfo.CStirrupPlaneInfos[j];
                    Singleton.Instance.GetStirrupLevelCount(i, stirPlaneSingInfo)
                        .AddNumber(1);
                }
            }
        }
    }
}
