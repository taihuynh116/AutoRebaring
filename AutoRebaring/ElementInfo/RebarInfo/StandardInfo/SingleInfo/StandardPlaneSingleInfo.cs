using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo.SingleInfo
{
    public class StraightStandardPlaneSingleInfo: IStandardPlaneSingleInfo
    {
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLayoutRule LayoutRule { get { return RebarLayoutRule.FixedNumber; } }
        public RebarHookType HookType { get; set; }
        public RebarBarType BarType { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public StandardLocationRegion LocationRegion { get; set; }
    }
    public class ImplantStandardPlaneSingleInfo : IStandardPlaneSingleInfo
    {
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLayoutRule LayoutRule { get { return RebarLayoutRule.FixedNumber; } }
        public RebarHookType HookType { get; set; }
        public RebarBarType BarType { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public StandardLocationRegion LocationRegion { get; set; }
    }
    public class LockheadStandardPlaneSingleInfo:IStandardPlaneSingleInfo
    {
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLayoutRule LayoutRule { get { return RebarLayoutRule.FixedNumber; } }
        public RebarHookType HookType { get; set; }
        public RebarBarType BarType { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public XYZ LockheadDirection { get; set; }
        public StandardLocationRegion LocationRegion { get; set; }
    }
    public class CrackingStandardPlaneSingleInfo : IStandardPlaneSingleInfo
    {
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLayoutRule LayoutRule { get { return RebarLayoutRule.FixedNumber; } }
        public RebarHookType HookType { get; set; }
        public RebarBarType BarType { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public XYZ CrackingDirection { get; set; }
        public double CrackingLength { get; set; }
        public StandardLocationRegion LocationRegion { get; set; }
    }
}
