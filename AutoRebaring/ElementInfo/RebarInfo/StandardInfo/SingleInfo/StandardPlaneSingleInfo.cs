using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.Single;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo.SingleInfo
{
    public class StraightStandardPlaneSingleInfo: IStandardPlaneSingleInfo
    {
        public int ID { get; set; } = 0;
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
        //public Rebar CreateRebar(double start, double end, Element hostElem)
        //{
        //    XYZ p1 = new XYZ(StartPoint.U, StartPoint.V, start);
        //    XYZ p2 = new XYZ(StartPoint.U, StartPoint.V, end);
        //    List<Curve> curves = new List<Curve> { Line.CreateBound(p1, p2) };
        //    Rebar rb = Rebar.CreateFromCurves(Singleton.Instance.Document, RebarStyle.Standard, BarType, HookType, HookType, hostElem, Normal, curves, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
        //    return rb;
        //}
    }
    public class ImplantStandardPlaneSingleInfo : IStandardPlaneSingleInfo
    {
        public int ID { get; set; } = 0;
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
        public Rebar CreateRebar(double start, double end, Element hostElem)
        {
            XYZ p1 = new XYZ(StartPoint.U, StartPoint.V, start);
            XYZ p2 = new XYZ(StartPoint.U, StartPoint.V, end);
            List<Curve> curves = new List<Curve> { Line.CreateBound(p1, p2) };
            Rebar rb = Rebar.CreateFromCurves(Singleton.Instance.Document, RebarStyle.Standard, BarType, HookType, HookType, hostElem, Normal, curves, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
            return rb;
        }
    }
    public class LockheadStandardPlaneSingleInfo:IStandardPlaneSingleInfo
    {
        public int ID { get; set; } = 0;
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
        public int ID { get; set; } = 0;
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
