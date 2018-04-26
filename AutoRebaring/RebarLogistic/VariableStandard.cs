using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo;
using AutoRebaring.Single;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.RebarLogistic
{
    public class VariableStandard
    {
        public List<double> LStandards { get; set; }
        public List<double> L1Standards { get; set; }
        public List<double> L2Standards { get; set; }
        public List<double> LResiduals { get; set; }
        public List<double> L1Residuals { get; set; }
        public List<double> L2Residuals { get; set; }
        public List<double> L1Standardmms { get { return L1Standards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2Standardmms { get { return L2Standards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> LStandardmms { get { return LStandards.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L1Residualmms { get { return L1Residuals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> L2Residualmms { get { return L2Residuals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public List<double> LResidualmms { get { return LResiduals.Select(x => GeomUtil.feet2Milimeter(x)).ToList(); } }
        public int CountStandard12 { get { return L1Standards.Count; } }
        public int CountStandardEqualZero { get { return LStandards.Count; } }
        public int CountResidual12 { get { return L2Residuals.Count; } }
        public int CountResidualEqualZero { get { return LResiduals.Count; } }
        public VariableStandard()
        {
            ARStandardChosen sc = Singleton.Instance.StandardChosen;
            double lmax = sc.Lmax * ConstantValue.milimeter2Feet;
            double lmin = sc.Lmin * ConstantValue.milimeter2Feet;
            double step = sc.Step * ConstantValue.milimeter2Feet;

            List<double> lstandards = Singleton.Instance.FitStandards;
            List<double> lplusStandards = Singleton.Instance.PairFitStandards;
            List<double> ltripStandards = Singleton.Instance.TripFitStandards;

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
                for (double l1 = lmax; l1 >= lplusStandards[i] / 2; l1 -= step)
                {
                    double l2 = lplusStandards[i] - l1;
                    if (l2 > l1) break;
                    if (l2 < lmin) continue;
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

            for (double l = lmax / 2; l <= lmax; l += step)
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
            for (double l = lmax / 2 - step; l >= lmin; l -= step)
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
        public void SetImplant(int idElem, int locIndex)
        {
            
        }
    }

    public enum TurnShortenType
    {
        Normal, NormalI, Shorten, LockHead, LockHeadFull
    }
}
