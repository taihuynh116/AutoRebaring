using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class StirrupDistribution
    {
        public double Z1 { get; set; }
        public double Z2 { get; set; }
        public StirrupDistribution(double z1, double z2)
        {
            Z1 = z1; Z2 = z2;
        }
        public static List<StirrupDistribution> CheckMerge(StirrupDistribution sd1, StirrupDistribution sd2, double spac, out bool isMerge)
        {
            double z1max = sd1.Z1 < sd2.Z1 ? sd2.Z1 : sd1.Z1;
            double z2min = sd1.Z2 < sd2.Z2 ? sd1.Z2 : sd2.Z2;
            double del = z1max - z2min - spac * 2;
            isMerge = false;
            if (GeomUtil.IsSmaller(del, 0))
            {
                double z1min = sd1.Z1 < sd2.Z1 ? sd1.Z1 : sd2.Z1;
                double z2max = sd1.Z2 < sd2.Z2 ? sd2.Z2 : sd1.Z2;
                isMerge = true;
                return new List<StirrupDistribution> { new StirrupDistribution(z1min, z2max) };
            }
            return new List<StirrupDistribution> { sd1, sd2 };
        }
        public int CompareTo(object obj)
        {
            StirrupDistribution other = obj as StirrupDistribution;
            return this.Z1.CompareTo(other.Z1);
        }
    }
}
