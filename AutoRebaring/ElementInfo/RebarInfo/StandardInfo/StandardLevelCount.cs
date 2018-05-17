using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo
{
    public class StandardLevelCount
    {
        public int IDElement { get; set; }
        public StandardCreatingEnum StandardCreating { get; set; }
        public StandardShapeEnum StandardShape { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public int LocationIndex { get; set; }
        public int Count { get; set; }
        public void AddNumber(int number)
        {
            Count += number;
        }
        public StandardLevelCount() { }
        public StandardLevelCount(int idElem, IStandardPlaneSingleInfo standPlaneSingInfo)
        {
            IDElement = idElem;
            StandardCreating = standPlaneSingInfo.StandardCreating;
            StandardShape = standPlaneSingInfo.StandardShape;
            RebarLocation = standPlaneSingInfo.RebarLocation;
            LocationIndex = standPlaneSingInfo.LocationIndex;
        }
    }
}
