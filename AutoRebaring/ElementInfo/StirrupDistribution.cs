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
        public List<double> StartZ1s { get; set; } = new List<double>();
        public double Z1 { get; set; }
        public List<double> EndZ1s { get; set; } = new List<double>();
        public List<int> Number1s { get; set; } = new List<int>();
        public double BeforeZ2 { get; set; }
        public List<double> StartZ2s { get; set; } = new List<double>();
        public double Z2 { get; set; }
        public List<double> EndZ2s { get; set; } = new List<double>();
        public List<int> Number2s { get; set; } = new List<int>();

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
            var instance = Singleton.Instance;

            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(idElem);
            StirrupDistribution stirDis = Singleton.Instance.GetStirrupDistribution(idElem, id);
            StirrupDistribution stirDisAfter = Singleton.Instance.GetStirrupDistributionAfter(idElem, id);
            double btSpac1 = designInfo.BotTopSpacings[0];
            double btSpac2 = designInfo.BotTopSpacings[1];
            double mSpac1 = designInfo.MiddleSpacings[0];
            double mSpac2 = designInfo.MiddleSpacings[1];

            if (id == 0)
            {
                if (id == Singleton.Instance.GetStirrupDistribuitionsCount(idElem) - 1)
                {
                    stirDis.StirrupLocation = StirrupLocation.Bottom;

                    double len = stirDis.Z2 - (stirDis.Z1 + 50*ConstantValue.milimeter2Feet);
                    double spa = btSpac1;
                    int n = (int)Math.Floor(len / spa);
                    n = len / spa > n + 0.55 ? n += 2 : n += 1;
                    stirDis.StartZ2s.Add(stirDis.Z1 + 50 * ConstantValue.milimeter2Feet + (n - 1) * spa);
                    stirDis.Number2s.Add(n);
                    stirDis.EndZ2s.Add(stirDis.StartZ2s[0] - (n - 1) * spa);

                     spa = btSpac2;
                     n = (int)Math.Floor(len / spa);
                    n = len / spa > n + 0.55 ? n += 2 : n += 1;
                    stirDis.StartZ2s.Add(stirDis.Z1 + 50 * ConstantValue.milimeter2Feet + (n - 1) * spa);
                    stirDis.Number2s.Add(n);
                    stirDis.EndZ2s.Add(stirDis.StartZ2s[1] - (n - 1) * spa);
                }
                else
                {
                    stirDis.StirrupLocation = StirrupLocation.Bottom;

                    double len = stirDis.Z2 - stirDis.Z1;
                    double spa = btSpac1;
                    int n = (int)Math.Floor(len / spa);
                    n = len/ spa > n+0.55 ? n+=2 : n += 1;
                    stirDis.StartZ2s.Add(stirDis.Z2);
                    stirDis.Number2s.Add(n);
                    stirDis.EndZ2s.Add(stirDis.StartZ2s[0] - (n - 1) * spa);

                    spa = btSpac2;
                    n = (int)Math.Floor(len / spa);
                    n = len / spa > n + 0.55 ? n += 2 : n += 1;
                    stirDis.StartZ2s.Add(stirDis.Z2);
                    stirDis.Number2s.Add(n);
                    stirDis.EndZ2s.Add(stirDis.StartZ2s[1] - (n - 1) * spa);

                    stirDisAfter.BeforeZ1 = stirDis.StartZ2s[0] + spa;
                    Singleton.Instance.UpdateStirrupDistribution(stirDisAfter);
                }
            }
            else if (id == Singleton.Instance.GetStirrupDistribuitionsCount(idElem) -1)
            {
                stirDis.StirrupLocation = StirrupLocation.Top;

                double len = stirDis.Z1 - stirDis.BeforeZ1;
                double spa = mSpac1;
                int n = (int)Math.Floor(len / spa);
                n = GeomUtil.IsEqual(n, len / spa) ? n : n += 1;
                stirDis.StartZ1s.Add(stirDis.BeforeZ1 + (n - 1) * spa);
                stirDis.Number1s.Add(n);
                stirDis.EndZ1s.Add(stirDis.StartZ1s[0] - (n - 1) * spa);

                spa = mSpac2;
                n = (int)Math.Floor(len / spa);
                n = GeomUtil.IsEqual(n, len / spa) ? n : n += 1;
                stirDis.StartZ1s.Add(stirDis.BeforeZ1 + (n - 1) * spa);
                stirDis.Number1s.Add(n);
                stirDis.EndZ1s.Add(stirDis.StartZ1s[1] - (n - 1) * spa);

                len = stirDis.Z2 - stirDis.Z1;
                spa = btSpac1;
                n = (int)Math.Floor(len / spa);
                n = len / spa > n + 0.55 ? n += 2 : n += 1;
                stirDis.StartZ2s.Add(stirDis.Z1 + (n - 1) * spa);
                stirDis.Number2s.Add(n);
                stirDis.EndZ2s.Add(stirDis.StartZ2s[0] - (n - 1) * spa);

                spa = btSpac2;
                n = (int)Math.Floor(len / spa);
                n = len / spa > n + 0.55 ? n += 2 : n += 1;
                stirDis.StartZ2s.Add(stirDis.Z1 + (n - 1) * spa);
                stirDis.Number2s.Add(n);
                stirDis.EndZ2s.Add(stirDis.StartZ2s[1] - (n - 1) * spa);
            }
            else
            {
                stirDis.StirrupLocation = StirrupLocation.Middle;

                double len = stirDis.Z1 - stirDis.BeforeZ1;
                double spa = mSpac1;
                int n = (int)Math.Floor(len / spa);
                n = GeomUtil.IsEqual(n, len / spa) ? n : n += 1;
                stirDis.StartZ1s.Add(stirDis.BeforeZ1 + (n - 1) * spa);
                stirDis.Number1s.Add(n);
                stirDis.EndZ1s.Add(stirDis.StartZ1s[0] - (n - 1) * spa);

                spa = mSpac2;
                n = (int)Math.Floor(len / spa);
                n = GeomUtil.IsEqual(n, len / spa) ? n : n += 1;
                stirDis.StartZ1s.Add(stirDis.BeforeZ1 + (n - 1) * spa);
                stirDis.Number1s.Add(n);
                stirDis.EndZ1s.Add(stirDis.StartZ1s[1] - (n - 1) * spa);

                stirDis.BeforeZ2 = stirDis.StartZ1s[0] + btSpac1;
                len = stirDis.Z2 - stirDis.BeforeZ2;
                spa = btSpac1;
                n = (int)Math.Floor(len / spa);
                n = GeomUtil.IsEqual(n, len / spa) ? n : n += 1;
                stirDis.StartZ2s.Add(stirDis.BeforeZ2 + (n - 1) * spa);
                stirDis.Number2s.Add(n);
                stirDis.EndZ2s.Add(stirDis.StartZ2s[0] - (n - 1) * spa);

                spa = btSpac2;
                n = (int)Math.Floor(len / spa);
                n = GeomUtil.IsEqual(n, len / spa) ? n : n += 1;
                stirDis.StartZ2s.Add(stirDis.BeforeZ2 + (n - 1) * spa);
                stirDis.Number2s.Add(n);
                stirDis.EndZ2s.Add(stirDis.StartZ2s[1] - (n - 1) * spa);

                stirDisAfter.BeforeZ1 = stirDis.StartZ2s[0] + mSpac1;
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
