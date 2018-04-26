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
        public ShortenEnum ShortenEnum
        {
            get
            {
                int maxShorten = Math.Max((int)ShortenU1, Math.Max((int)ShortenU2, Math.Max((int)ShortenV1, (int)ShortenV2)));
                return (ShortenEnum)maxShorten;
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
