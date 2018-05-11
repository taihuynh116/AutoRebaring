using AutoRebaring.Constant;
using AutoRebaring.Single;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public enum StirrupLocation { Bottom, Middle, Top}
    public class StirrupDistribution
    {
        public int ID { get; set; }
        public int IDElement { get; set; }
        public double BeforeZ1 { get; set; }
        public double StartZ1 { get; set; }
        public double Z1 { get; set; }
        public double BeforeZ2 { get; set; }
        public double StartZ2 { get; set; }
        public double Z2 { get; set; }
        public double Number1 { get; set; }
        public double Number2 { get; set; }
        public StirrupLocation StirrupLocation { get; set; }
        public StirrupDistribution(int id, int idElem, double z1, double z2)
        {
            ID = id; IDElement = idElem; Z1 = z1; Z2 = z2;
        }
        public StirrupDistribution() { }
        //public static List<StirrupDistribution> CheckMerge(StirrupDistribution sd1, StirrupDistribution sd2, double spac, out bool isMerge)
        //{
        //    double z1max = sd1.Z1 < sd2.Z1 ? sd2.Z1 : sd1.Z1;
        //    double z2min = sd1.Z2 < sd2.Z2 ? sd1.Z2 : sd2.Z2;
        //    double del = z1max - z2min - spac * 2;
        //    isMerge = false;
        //    if (GeomUtil.IsSmaller(del, 0))
        //    {
        //        double z1min = sd1.Z1 < sd2.Z1 ? sd1.Z1 : sd2.Z1;
        //        double z2max = sd1.Z2 < sd2.Z2 ? sd2.Z2 : sd1.Z2;
        //        isMerge = true;
        //        return new List<StirrupDistribution> { new StirrupDistribution(sd1.ID,sd1.IDElement, z1min, z2max) };
        //    }
        //    return new List<StirrupDistribution> { sd1, sd2 };
        //}
        public static void GetDetailDistribution(int id, int idElem)
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(idElem);
            StirrupDistribution stirDis = Singleton.Instance.GetStirrupDistribution(idElem, id);
            StirrupDistribution stirDisAfter = Singleton.Instance.GetStirrupDistributionAfter(idElem, id);
            double btSpac = designInfo.BotTopSpacings[0];
            double mSpac = designInfo.MiddleSpacings[0];

            if (id == 0)
            {
                if (id == Singleton.Instance.GetStirrupDistribuitionsCount(idElem) - 1)
                {
                    stirDis.StirrupLocation = StirrupLocation.Bottom;

                    double len = stirDis.Z2 - (stirDis.Z1 + 50*ConstantValue.milimeter2Feet);
                    int n = (int)Math.Floor(len / btSpac);
                    n = len / btSpac > n + 0.55 ? n += 2 : n += 1;
                    stirDis.StartZ2 = (stirDis.Z1 + 50 * ConstantValue.milimeter2Feet) + (n - 1) * btSpac;
                    stirDis.Number2 = n;
                }
                else
                {
                    stirDis.StirrupLocation = StirrupLocation.Bottom;

                    double len = stirDis.Z2 - stirDis.Z1;
                    int n = (int)Math.Floor(len / btSpac);
                    n = len/ btSpac > n+0.55 ? n+=2 : n += 1;
                    stirDis.StartZ2 = stirDis.Z2;
                    stirDis.Number2 = n;

                    stirDisAfter.BeforeZ1 = stirDis.StartZ2 + mSpac;
                    Singleton.Instance.UpdateStirrupDistribution(stirDisAfter);
                }
            }
            else if (id == Singleton.Instance.GetStirrupDistribuitionsCount(idElem) -1)
            {
                stirDis.StirrupLocation = StirrupLocation.Top;

                double len = stirDis.Z1 - stirDis.BeforeZ1;
                int n = (int)Math.Floor(len / mSpac);
                n = GeomUtil.IsEqual(n, len / mSpac) ? n : n += 1;
                stirDis.StartZ1 = stirDis.BeforeZ1 + (n - 1) * mSpac;
                stirDis.Number1 = n;

                len = stirDis.Z2 - stirDis.Z1;
                n = (int)Math.Floor(len / btSpac);
                n = len / btSpac > n + 0.55 ? n += 2 : n += 1;
                stirDis.StartZ2 = stirDis.Z1 + (n - 1) * btSpac;
                stirDis.Number2 = n;
            }
            else
            {
                stirDis.StirrupLocation = StirrupLocation.Middle;

                double len = stirDis.Z1 - stirDis.BeforeZ1;
                int n = (int)Math.Floor(len / mSpac);
                n = GeomUtil.IsEqual(n, len / mSpac) ? n : n += 1;
                stirDis.StartZ1 = stirDis.BeforeZ1 + (n - 1) * mSpac;
                stirDis.Number1 = n;

                stirDis.BeforeZ2 = stirDis.StartZ1 + btSpac;
                len = stirDis.Z2 - stirDis.BeforeZ2;
                n = (int)Math.Floor(len / btSpac);
                n = GeomUtil.IsEqual(n, len / btSpac) ? n : n += 1;
                stirDis.StartZ2 = stirDis.BeforeZ2 + (n - 1) * btSpac;
                stirDis.Number2 = n;

                stirDisAfter.BeforeZ1 = stirDis.StartZ2 + mSpac;
                Singleton.Instance.UpdateStirrupDistribution(stirDisAfter);
            }

            Singleton.Instance.UpdateStirrupDistribution(stirDis);
        }
        public int CompareTo(object obj)
        {
            StirrupDistribution other = obj as StirrupDistribution;
            return this.Z1.CompareTo(other.Z1);
        }
    }
}
