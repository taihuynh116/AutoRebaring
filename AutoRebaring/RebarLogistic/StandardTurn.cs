using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.RebarLogistic
{
    public class StandardTurn
    {
        public Variable Variable { get; set; }
        public bool Swap { get; set; }
        public int Index { get; set; }
        public IElementInfo ElementInfo { get; set; }
        public int ElementIndex { get { return ElementInfo.Index; } }
        public double Start1 { get; set; }
        public double Start2 { get; set; }
        public double End1 { get; set; }
        public double End2 { get; set; }
        public double L1 { get; set; }
        public double L2 { get; set; }
        public bool Finish1 { get; set; }
        public bool Finish2 { get; set; }
        public bool Implant1 { get; set; }
        public bool Implant2 { get; set; }
        public double l1Finish { get; set; }
        public double l2Finish { get; set; }
        public bool IsImplanted { get; set; }
        public void SetFinish1(double lmax)
        {
            setFinish(lmax, Start1, End1, Finish1, l1Finish);
        }
        public void SetFinish2(double lmax)
        {
            setFinish(lmax, Start2, End2, Finish2, l2Finish);
        }
        public void setFinish(double lmax, double start, double end, bool type, double len)
        {
            if (checkManual(lmax, start, end))
            {
                double topLimit = ElementInfo.VerticalInfo.TopLockHead;
                double l = L1 - (end - topLimit);
                type = true;
                len = ConstantValue.milimeter2Feet * Math.Round(l * ConstantValue.feet2MiliMeter);
            }
        }
        public bool checkManual(double lmax, double start, double end)
        {
            double topLimit = ElementInfo.VerticalInfo.TopLockHead;
            return GeomUtil.IsEqualOrBigger(start + lmax, topLimit);
        }
        public void SetImplant1(double lmax, List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            setImplant(lmax, Start1, End1, Implant1, lImplants, lPlusImplants, ap, dp);
        }
        public void SetImplant2(double lmax, List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            setImplant(lmax, Start2, End2, Implant2, lImplants, lPlusImplants, ap, dp);
        }
        public void setImplant(double lmax, double start, double end, bool type, List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            setImplantChoice(lImplants, lPlusImplants, ap, dp);
            if (checkManual(lmax, start, end))
            {
                type = true;
            }
        }
        public void setImplantChoice(List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            if (!IsImplanted)
            {
                Variable.SetImplant(lImplants, lPlusImplants, ap, dp, ElementInfo);
                IsImplanted = true;
            }
        }

    }
}
