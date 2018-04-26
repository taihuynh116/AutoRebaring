using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.Single;
using AutoRebaring.RebarLogistic;
using Geometry;
using AutoRebaring.Database.AutoRebaring.EF;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo.SingleInfo
{
    public class StraightStandardPlaneSingleInfo : IStandardPlaneSingleInfo
    {
        public int ID { get; set; } = 0;
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public int LocationIndex { get; set; }
        public Rebar CreateRebar(int idTurn, int locIndex)
        {
            if (Number <= 0) return null;
            StandardTurn st = Singleton.Instance.GetStandardTurn(idTurn, locIndex);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(st.IDElement);
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(st.IDElement);

            XYZ p1 = null, p2 = null;
            switch (RebarLocation)
            {
                case RebarLocation.L1:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, st.Start1);
                    p2 = new XYZ(StartPoint.U, StartPoint.V, st.End1);
                    break;
                case RebarLocation.L2:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, st.Start2);
                    p2 = new XYZ(StartPoint.U, StartPoint.V, st.End2);
                    break;
            }
            List<Curve> curves = new List<Curve> { Line.CreateBound(p1, p2) };
            Rebar rb = Rebar.CreateFromCurves(Single.Singleton.Instance.Document, RebarStyle.Standard, designInfo.StandardTypes[LocationIndex], designInfo.StandardHookTypes[LocationIndex],
                designInfo.StandardHookTypes[LocationIndex], revitInfo.Element, Normal, curves, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            RebarShapeDrivenAccessor rsda = rb.GetShapeDrivenAccessor();
            if (GeomUtil.IsEqual(ArrayLength, 0))
            {
                rsda.SetLayoutAsSingle();
            }
            else
            {
                try
                {
                    rsda.SetLayoutAsFixedNumber(Number, ArrayLength, true, true, true);
                }
                catch
                {
                    throw;
                }
            }

            rb.LookupParameter("Comments").Set("add-in");
            rb.LookupParameter("Location").Set(RebarLocation.ToString());
            return rb;
        }
    }
    public class ImplantStandardPlaneSingleInfo : IStandardPlaneSingleInfo
    {
        public int ID { get; set; } = 0;
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public int LocationIndex { get; set; }
        public Rebar CreateRebar(int idTurn, int locIndex)
        {
            if (Number <= 0) return null;
            StandardTurn st = Singleton.Instance.GetStandardTurn(idTurn, locIndex);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(st.IDElement);
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(st.IDElement);
            IVerticalInfo verticalInfo = Singleton.Instance.GetVerticalInfo(st.IDElement);

            XYZ p1 = null, p2 = null;
            switch (RebarLocation)
            {
                case RebarLocation.L1:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, verticalInfo.TopAnchorAfters[LocationIndex]);
                    p2 = new XYZ(StartPoint.U, StartPoint.V, st.End1);
                    break;
                case RebarLocation.L2:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, verticalInfo.TopAnchorAfters[LocationIndex]);
                    p2 = new XYZ(StartPoint.U, StartPoint.V, st.End2);
                    break;
            }
            List<Curve> curves = new List<Curve> { Line.CreateBound(p1, p2) };
            Rebar rb = Rebar.CreateFromCurves(Single.Singleton.Instance.Document, RebarStyle.Standard, designInfoAfter.StandardTypes[LocationIndex], designInfoAfter.StandardHookTypes[LocationIndex],
                designInfoAfter.StandardHookTypes[LocationIndex], revitInfo.Element, Normal, curves, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            RebarShapeDrivenAccessor rsda = rb.GetShapeDrivenAccessor();
            if (GeomUtil.IsEqual(ArrayLength, 0))
            {
                rsda.SetLayoutAsSingle();
            }
            else
            {
                rsda.SetLayoutAsFixedNumber(Number, ArrayLength, true, true, true);
            }

            rb.LookupParameter("Comments").Set("add-in");
            rb.LookupParameter("Location").Set(RebarLocation.ToString());
            return rb;
        }
    }
    public class LockheadStandardPlaneSingleInfo : IStandardPlaneSingleInfo
    {
        public int ID { get; set; } = 0;
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public XYZ LockheadDirection { get; set; }
        public int LocationIndex { get; set; }
        public Rebar CreateRebar(int idTurn, int locIndex)
        {
            if (Number <= 0) return null;
            StandardTurn st = Singleton.Instance.GetStandardTurn(idTurn, locIndex);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(st.IDElement);
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(st.IDElement);
            IVerticalInfo verticalInfo = Singleton.Instance.GetVerticalInfo(st.IDElement);
            ARLockheadParameter lp = Singleton.Instance.LockheadParameter;

            XYZ p1 = null, p2 = null, p3 = null;
            switch (RebarLocation)
            {
                case RebarLocation.L1:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, st.Start1);
                    p2 = new XYZ(StartPoint.U, StartPoint.V, verticalInfo.TopLockHead);
                    break;
                case RebarLocation.L2:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, st.Start2);
                    p2 = new XYZ(StartPoint.U, StartPoint.V, verticalInfo.TopLockHead);
                    break;
            }
            p3 = p2 + LockheadDirection * designInfo.StandardDiameters[LocationIndex] * lp.LockheadMutiply;
            List<Curve> curves = new List<Curve> { Line.CreateBound(p1, p2), Line.CreateBound(p2, p3) };
            Rebar rb = Rebar.CreateFromCurves(Single.Singleton.Instance.Document, RebarStyle.Standard, designInfo.StandardTypes[LocationIndex], designInfo.StandardHookTypes[LocationIndex],
                designInfo.StandardHookTypes[LocationIndex], revitInfo.Element, Normal, curves, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            RebarShapeDrivenAccessor rsda = rb.GetShapeDrivenAccessor();
            if (GeomUtil.IsEqual(ArrayLength, 0))
            {
                rsda.SetLayoutAsSingle();
            }
            else
            {
                rsda.SetLayoutAsFixedNumber(Number, ArrayLength, true, true, true);
            }

            rb.LookupParameter("Comments").Set("add-in");
            rb.LookupParameter("Location").Set(RebarLocation.ToString());
            return rb;
        }
    }
    public class CrackingStandardPlaneSingleInfo : IStandardPlaneSingleInfo
    {
        public int ID { get; set; } = 0;
        public XYZ Normal { get; set; }
        public int Number { get; set; }
        public double Spacing { get; set; }
        public double ArrayLength { get { return Spacing * (Number - 1); } }
        public UV StartPoint { get; set; }
        public RebarLocation RebarLocation { get; set; }
        public XYZ CrackingDirection { get; set; }
        public double CrackingLength { get; set; }
        public int LocationIndex { get; set; }
        public Rebar CreateRebar(int idTurn, int locIndex)
        {
            if (Number <= 0) return null;
            StandardTurn st = Singleton.Instance.GetStandardTurn(idTurn, locIndex);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(st.IDElement);
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(st.IDElement);
            IVerticalInfo verticalInfo = Singleton.Instance.GetVerticalInfo(st.IDElement);
            ARLockheadParameter lp = Singleton.Instance.LockheadParameter;

            XYZ p1 = null, p2 = null, p3 = null, p4 = null;
            switch (RebarLocation)
            {
                case RebarLocation.L1:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, st.Start1);
                    p3 = p1 + CrackingDirection * CrackingLength;
                    p3 = new XYZ(p3.X, p3.Y, verticalInfo.TopSmall);
                    p2 = p3 - CrackingDirection * CrackingLength - XYZ.BasisZ * CrackingLength * lp.LHRatio;
                    p4 = new XYZ(p3.X, p3.Y, st.End1);
                    break;
                case RebarLocation.L2:
                    if (GeomUtil.IsEqual(st.L1, 0)) return null;
                    p1 = new XYZ(StartPoint.U, StartPoint.V, st.Start2);
                    p3 = p1 + CrackingDirection * CrackingLength;
                    p3 = new XYZ(p3.X, p3.Y, verticalInfo.TopSmall);
                    p2 = p3 - CrackingDirection * CrackingLength - XYZ.BasisZ * CrackingLength * lp.LHRatio;
                    p4 = new XYZ(p3.X, p3.Y, st.End2);
                    break;
            }

            List<Curve> curves = new List<Curve> { Line.CreateBound(p1, p2), Line.CreateBound(p2, p3), Line.CreateBound(p3, p4) };
            Rebar rb = Rebar.CreateFromCurves(Single.Singleton.Instance.Document, RebarStyle.Standard, designInfo.StandardTypes[LocationIndex], designInfo.StandardHookTypes[LocationIndex],
                designInfo.StandardHookTypes[LocationIndex], revitInfo.Element, Normal, curves, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            RebarShapeDrivenAccessor rsda = rb.GetShapeDrivenAccessor();
            if (GeomUtil.IsEqual(ArrayLength, 0))
            {
                rsda.SetLayoutAsSingle();
            }
            else
            {
                rsda.SetLayoutAsFixedNumber(Number, ArrayLength, true, true, true);
            }

            rb.LookupParameter("Comments").Set("add-in");
            rb.LookupParameter("Location").Set(RebarLocation.ToString());
            return rb;
        }
    }
}
