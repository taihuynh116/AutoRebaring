﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo.SingleInfo
{
    public enum RebarLocation
    {
        L1, L2
    }
    public interface IStandardPlaneSingleInfo
    {
        XYZ Normal { get; set; }
        int Number { get; set; }
        double Spacing { get; set; }
        double ArrayLength { get; }
        UV StartPoint { get; set; }
        RebarLayoutRule LayoutRule { get; }
        RebarHookType HookType { get; set; }
        RebarBarType BarType { get; set; }
        RebarLocation RebarLocation { get; set; }
    }
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
    }
    public class CrackingStandardPlaneSingleInfo
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
    }
}
