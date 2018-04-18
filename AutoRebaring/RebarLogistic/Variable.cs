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
    public class Variable
    {
        public double Lmax { get; set; }
        public double Lmin { get; set; }
        public double Step { get; set; }
        public List<double> LStandards { get; set; }
        public List<double> L1Standards { get; set; }
        public List<double> L2Standards { get; set; }
        public List<double> LResiduals { get; set; }
        public List<double> L1Residuals { get; set; }
        public List<double> L2Residuals { get; set; }
        public List<double> LImplants { get; set; }
        public List<double> L1Implants { get; set; }
        public List<double> L2Implants { get; set; }
        public List<double> L1ImplantStandards { get; set; }
        public List<double> L2ImplantStandards { get; set; }
        public List<double> L1ImplantResiduals { get; set; }
        public List<double> L2ImplantResiduals { get; set; }
        public List<double> L1StandardImplants { get; set; }
        public List<double> L2StandardImplants { get; set; }
        public List<double> L1ResidualImplants { get; set; }
        public List<double> L2ResidualImplants { get; set; }
        public List<double> L1Standardmms { get { return L1Standards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2Standardmms { get { return L2Standards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> LStandardmms { get { return LStandards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L1Residualmms { get { return L1Residuals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2Residualmms { get { return L2Residuals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> LResidualmms { get { return LResiduals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> LImplantmms { get { return LImplants.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L1Implantmms { get { return L1Implants.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2Implantmms { get { return L2Implants.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L1ImplantStandardmms { get { return L1ImplantStandards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2ImplantStandardmms { get { return L2ImplantStandards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L1ImplantResidualmms { get { return L1ImplantResiduals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2ImplantResidualmms { get { return L2ImplantResiduals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L1StandardImplantmms { get { return L1StandardImplants.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2StandardImplantmms { get { return L2StandardImplants.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L1ResidualImplantmms { get { return L1ResidualImplants.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2ResidualImplantmms { get { return L2ResidualImplants.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public int CountStandard12 { get { return L1Standards.Count; } }
        public int CountStandardEqualZero { get { return LStandards.Count; } }
        public int CountStandardImplant { get { return L1StandardImplants.Count; } }
        public int CountResidual12 { get { return L2Residuals.Count; } }
        public int CountResidualEqualZero { get { return LResiduals.Count; } }
        public int CountResidualImplant { get { return L1ResidualImplants.Count; } }
        public int CountImplant12 { get { return L1Implants.Count; } }
        public int CountImplantEqualZero { get { return LImplants.Count; } }

        public Variable(double Lmax, double Lmin, double step, List<double> lstandards, List<double> lplusStandards)
        {
            this.Lmax = Lmax; this.Lmin = Lmin; this.Step = step;
            L1Standards = new List<double>();
            L2Standards = new List<double>();
            LResiduals = new List<double>();
            L1Residuals = new List<double>();
            L2Residuals = new List<double>();
            LStandards = lstandards;

            for (int i = 0; i < LStandards.Count; i++)
            {
                for (int j = 0; j < LStandards.Count - i; j++)
                {
                    L1Standards.Add(LStandards[i]);
                }
                for (int j = i; j < LStandards.Count; j++)
                {
                    L2Standards.Add(LStandards[j]);
                }
            }
            for (int i = 0; i < lplusStandards.Count; i++)
            {
                for (double l1 = Lmax; l1 >= lplusStandards[i] / 2; l1 -= Step)
                {
                    double l2 = lplusStandards[i] - l1;
                    if (l2 > l1) break;
                    if (l2 < Lmin) continue;
                    bool isStandard1 = false;
                    bool isStandard2 = false;
                    for (int k1 = 0; k1 < LStandards.Count; k1++)
                    {
                        if (l1 == LStandards[k1])
                        {
                            isStandard1 = true;
                            break;
                        }
                    }
                    for (int k2 = 0; k2 < LStandards.Count; k2++)
                    {
                        if (l2 == LStandards[k2])
                        {
                            isStandard2 = true;
                            break;
                        }
                    }
                    if (isStandard1 && isStandard2) continue;
                    L1Standards.Add(l1); L2Standards.Add(l2);
                }
            }

            for (double l = Lmax / 2; l <= Lmax; l += Step)
            {
                bool isStandard = false;
                for (int j = 0; j < LStandards.Count; j++)
                {
                    if (LStandards[j] == l)
                    {
                        isStandard = true;
                        break;
                    }
                }
                if (!isStandard) LResiduals.Add(l);
            }
            for (double l = Lmax / 2 - step; l >= Lmin; l -= Step)
            {
                bool isStandard = false;
                for (int j = 0; j < LStandards.Count; j++)
                {
                    if (LStandards[j] == l)
                    {
                        isStandard = true;
                        break;
                    }
                }
                if (!isStandard) LResiduals.Add(l);
            }

            for (int i = 0; i < LStandards.Count; i++)
            {
                for (int j = 0; j < LResiduals.Count; j++)
                {
                    L1Residuals.Add(LStandards[i]);
                    L2Residuals.Add(LResiduals[j]);
                }
            }
            for (int i = 0; i < LResiduals.Count; i++)
            {
                for (int j = 0; j < LResiduals.Count - i; j++)
                {
                    L1Residuals.Add(LResiduals[i]);
                }
                for (int j = i; j < LResiduals.Count; j++)
                {
                    L2Residuals.Add(LResiduals[j]);
                }
            }
        }
        public void SetImplant(List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp, IElementInfo ei)
        {
            SetImplant(lImplants, lPlusImplants, ap.AnchorMultiply, dp.DevelopmentMultiply, ei.VerticalInfo.BottomOffsetValue, ei.VerticalInfo.TopOffset - ei.VerticalInfo.Bottom, ei.DesignInfo.DesignInfoAfter.StandardDiameters[0]);
        }
        public void SetImplant(List<double> lImplants, List<double> lPlusImplants, int anchorMulti, int devMulti, double botOff, double lenFromTop, double diameter)
        {
            double lenBot1 = botOff + (devMulti * 2 + anchorMulti) * diameter;
            double lenBot2 = botOff + (devMulti + anchorMulti) * diameter;
            double lenTop1 = lenFromTop + anchorMulti * diameter;
            double lenTop2 = lenFromTop;

            LImplants = lImplants;
            List<double> limplantResiduals = new List<double> { lenBot1, lenBot2, lenTop1, lenTop2 };
            LImplants.AddRange(limplantResiduals);

            L1Implants = new List<double>();
            L2Implants = new List<double>();
            foreach (double l in LImplants)
            {
                L1Implants.Add(l);
                L2Implants.Add(l);
            }

            L1ImplantStandards = new List<double>();
            L2ImplantStandards = new List<double>();
            L1ImplantResiduals = new List<double>();
            L2ImplantResiduals = new List<double>();

            L1StandardImplants = new List<double>();
            L2StandardImplants = new List<double>();
            L1ResidualImplants = new List<double>();
            L2ResidualImplants = new List<double>();
            for (int j = 0; j < LImplants.Count; j++)
            {
                for (int i = 0; i < LStandards.Count; i++)
                {
                    L1ImplantStandards.Add(LImplants[j]);
                    L2ImplantStandards.Add(LStandards[i]);

                    L1StandardImplants.Add(LStandards[i]);
                    L2StandardImplants.Add(LImplants[j]);
                }

                for (int i = 0; i < LResiduals.Count; i++)
                {
                    L1ImplantResiduals.Add(LImplants[j]);
                    L2ImplantResiduals.Add(LResiduals[i]);

                    L1ResidualImplants.Add(LResiduals[i]);
                    L2ResidualImplants.Add(LImplants[j]);
                }
            }
        }
    }

    public enum TurnShortenType
    {
        Normal, NormalI, Shorten, LockHead, LockHeadFull
    }
}
