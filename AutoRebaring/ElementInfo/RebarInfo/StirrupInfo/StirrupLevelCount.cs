using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StirrupInfo
{
    public class StirrupLevelCount
    {
        public int IDElement { get; set; }
        public StirrupTypeEnum StirrupType { get; set; }
        public int Count { get; set; } = 0;
        public void AddNumber(int number)
        {
            Count += number;
        }
        public StirrupLevelCount() { }
        public StirrupLevelCount(int idElem, IStirrupPlaneSingleInfo stirrPlaneSingInfo)
        {
            IDElement = idElem;
            StirrupType = stirrPlaneSingInfo.StirrupType;
        }
    }
}
