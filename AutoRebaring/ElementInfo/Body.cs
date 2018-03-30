using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.Database;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using AutoRebaring.ElementInfo.Shorten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public interface IElementInfo
    {
        IPlaneInfo PlaneInfo { get; set; }
        IVerticalInfo VerticalInfo { get; set; }
        IDesignInfo DesignInfo { get; set; }
        IRevitInfo RevitInfo { get; set; }
    }

    public interface IDesignInfo
    {
        Level Level { get; set; }
        List<RebarBarType> StandardTypes { get; set; }
        List<double> StandardDiameters { get; set; }
        List<RebarHookType> StandardHookTypes { get; set; }
        List<int> StandardNumbers { get; set; }
        List<double> StandardSpacings { get; set; }
        List<RebarBarType> StirrupTypes { get; set; }
        List<double> StirrupDiameters { get; set; }
        List<double> BotTopSpacings { get; set; }
        List<double> MiddleSpacings { get; set; }
        bool IsDoubleN2 { get; set; }
        IDesignInfo DesignInfoAfter { get; set; }
        IDesignInfo DesignInfoBefore { get; set; }
        void GetStandardSpacing(IPlaneInfo pi, GeneralParameterInput gpi);
        void GetDesignInfo(IDesignInfo diA, IDesignInfo diB);
    }
    public enum ShortenEnum
    {
        None, Small, Big
    }

    public interface IShortenType
    {
        ShortenEnum ShortenU1 { get; set; }
        ShortenEnum ShortenU2 { get; set; }
        ShortenEnum ShortenV1 { get; set; }
        ShortenEnum ShortenV2 { get; set; }
        double DeltaU1 { get; set; }
        double DeltaU2 { get; set; }
        double DeltaV1 { get; set; }
        double DeltaV2 { get; set; }
    }
    public interface IPlaneInfo
    {
        List<double> B1s { get; }
        List<double> B2s { get; }
        UV VectorU { get; }
        UV VectorV { get; }
        XYZ VectorX { get; }
        XYZ VectorY { get; }
        List<List<UV>> BoundaryPointLists { get; }
        List<ShortenType> ShortenTypes { get; }
        List<List<UV>> StandardRebarPointLists { get; }
        List<List<UV>> StirrupRebarPointLists { get; }
        IPlaneInfo PlaneInfoAfter { get; }
        void GetFullPlaneInfo(GeneralParameterInput gpi);
        void GetRebarLocation(IDesignInfo di);
        void GetShortenType(IPlaneInfo pia);
    }
    public interface IRevitInfo
    {
        Document Document { get; set; }
        Element Element { get; set; }
        double Elevation { get; set; }
    }
    public interface IVerticalInfo
    {
        Level StartLevel { get; set; }
        Level EndLevel { get; set; }
        double Top { get; }
        double TopBeam { get; set; }
        double TopFloor { get; set; }
        double TopOffset { get; }
        double TopLimit { get; set; }
        double TopLockHead { get; }
        double TopSmall { get; }
        List<double> TopAnchorAfters { get; set; }
        double TopStirrup1 { get; }
        double TopStirrup2 { get; }
        double Bottom { get; set; }
        double BottomOffset { get; }
        double BottomStirrup1 { get; }
        double BottomStirrup2 { get; }
        List<double> RebarDevelopmentLengths { get; set; }
        List<StirrupDistribution> StirrupDistributions { get; set; }
        void GetInformation(GeneralParameterInput gpi);
        void GetRebarInformation(IDesignInfo di);
    }
    public enum RebarLocation
    {
        L1, L2
    }
    public enum StandardLocationRegion
    {
        Column, Edge, Middle
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
        StandardLocationRegion LocationRegion { get; set; }
    }
    public interface IStandardParameter
    {
        List<string> ParameterNames { get; set; }
        List<object> ParameterValues { get; set; }
    }
    public interface IStandardPlaneInfo
    {
        List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        List<IStandardPlaneSingleInfo> ShortenStandardPlaneInfos { get; set; }
        List<IStandardPlaneSingleInfo> ImplantStandardPlaneInfos { get; set; }
    }

}
