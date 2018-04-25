using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using AutoRebaring.Single;
using AutoRebaring.ElementInfo;

namespace AutoRebaring.RebarLogistic
{
    public class VariableImplant
    {
        public int ID { get; set; }
        public int LocationIndex { get; set; }
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
        public int CountStandardImplant { get { return L1StandardImplants.Count; } }
        public int CountResidualImplant { get { return L1ResidualImplants.Count; } }
        public int CountImplant12 { get { return L1Implants.Count; } }
        public int CountImplantEqualZero { get { return LImplants.Count; } }
        public VariableImplant(int id, int locIndex)
        {
            this.ID = id;
            this.LocationIndex = locIndex;

            List<double> lImplants = Singleton.Instance.FitImplants;
            List<double> lPlusImplants = Singleton.Instance.PairFitImplants;
            int anchorMulti = Singleton.Instance.AnchorParameter.AnchorMultiply;
            int devMulti = Singleton.Instance.DevelopmentParameter.DevelopmentMultiply;

            IVerticalInfo verInfo = Singleton.Instance.GetVerticalInfo(id);
            double botOff = verInfo.BottomOffsetValue;
            double lenFromTop = verInfo.TopOffset - verInfo.Bottom;

            IDesignInfo desInfo = Singleton.Instance.GetDesignInfo(id);
            double diameter = desInfo.StandardDiameters[locIndex];

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

            List<double> lstands = Singleton.Instance.VariableStandard.LStandards;
            List<double> lresiduals = Singleton.Instance.VariableStandard.LResiduals;
            for (int j = 0; j < LImplants.Count; j++)
            {
                for (int i = 0; i < lstands.Count; i++)
                {
                    L1ImplantStandards.Add(LImplants[j]);
                    L2ImplantStandards.Add(lstands[i]);

                    L1StandardImplants.Add(lstands[i]);
                    L2StandardImplants.Add(LImplants[j]);
                }

                for (int i = 0; i < lresiduals.Count; i++)
                {
                    L1ImplantResiduals.Add(LImplants[j]);
                    L2ImplantResiduals.Add(lresiduals[i]);

                    L1ResidualImplants.Add(lresiduals[i]);
                    L2ResidualImplants.Add(LImplants[j]);
                }
            }
        }
    }
}
