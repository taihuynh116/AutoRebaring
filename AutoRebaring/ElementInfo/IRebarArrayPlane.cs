using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public interface IRebarArrayPlane
    {
        XYZ Normal { get; set; }
        RebarLayoutRule RebarLayoutRule { get; set; }
        RebarHookType RebarHookType { get; set; }
        RebarBarType RebarType { get; set; }
        int Number { get; set; }
        double Spacing { get; set; }
        double ArrayLength { get; }
        UV TopUV { get; set; }
        RebarStyle RebarStyle { get; set; }
    }
    public class NormalRebarArrayPlane : IRebarArrayPlane
    {
        public RebarLayoutRule RebarLayoutRule { get; set; }
        public RebarHookType RebarHookType { get; set; }
        public RebarBarType RebarType { get; set; }
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV TopUV { get; set; }
        public RebarStyle RebarStyle { get; set; }
        public NormalRebarArrayPlane(RebarLayoutRule layoutRule, RebarHookType hookType, RebarBarType barType, XYZ normal, int number, double spac, UV topUV)
        {
            RebarLayoutRule = layoutRule;
            RebarHookType = hookType;
            RebarType = barType;
            Normal = normal;
            Number = number;
            Spacing = spac;
            TopUV = topUV;
        }
    }
    public class LockheadRebarArrayPlane : IRebarArrayPlane
    {
        public XYZ Normal { get; set; }
        public RebarLayoutRule RebarLayoutRule { get; set; }
        public RebarHookType RebarHookType { get; set; }
        public RebarBarType RebarType { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV TopUV { get; set; }
        public RebarStyle RebarStyle { get; set; }
        public LockheadRebarArrayPlane(RebarLayoutRule layoutRule, RebarHookType hookType, RebarBarType barType, XYZ normal, int number, double spac, UV topUV)
        {
            RebarLayoutRule = layoutRule;
            RebarHookType = hookType;
            RebarType = barType;
            Normal = normal;
            Number = number;
            Spacing = spac;
            TopUV = topUV;
        }
    }
    public class ShortenRebarArrayPlane : IRebarArrayPlane
    {
        public XYZ Normal { get; set; }
        public RebarLayoutRule RebarLayoutRule { get; set; }
        public RebarHookType RebarHookType { get; set; }
        public RebarBarType RebarType { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV TopUV { get; set; }
        public RebarStyle RebarStyle { get; set; }
    }
}
