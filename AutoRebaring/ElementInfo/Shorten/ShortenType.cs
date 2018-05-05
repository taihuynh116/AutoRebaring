using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace AutoRebaring.ElementInfo.Shorten
{
    public class ShortenType : IShortenType
    {
        public bool IsLockhealAll
        {
            get
            {
                if (ShortenU1 == ShortenEnum.Big && ShortenU2 == ShortenEnum.Big && ShortenV1 == ShortenEnum.Big && ShortenV2 == ShortenEnum.Big) return true;
                return false;
            }
        }
        public ShortenEnum ShortenU1 { get; set; }
        public ShortenEnum ShortenU2 { get; set; }
        public ShortenEnum ShortenV1 { get; set; }
        public ShortenEnum ShortenV2 { get; set; }
        public double DeltaU1 { get; set; }
        public double DeltaU2 { get; set; }
        public double DeltaV1 { get; set; }
        public double DeltaV2 { get; set; }

        public ShortenType() { }
    }
}
