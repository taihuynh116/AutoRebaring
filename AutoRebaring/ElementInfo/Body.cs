using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.Database;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using AutoRebaring.ElementInfo.Shorten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public interface IRevitInfo
    {
        int ID { get; set; }
        Document Document { get; set; }
        Element Element { get; set; }
        double Elevation { get; set; }
        Level Level { get; set; }
    }
    public interface IPlaneInfo
    {
        int ID { get; set; }
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
        void GetRebarLocation();
        void GetShortenType();
    }
    public interface IVerticalInfo
    {
        int ID { get; set; }
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
        double BottomOffsetValue { get; }
        List<double> EndLimit0s { get; set; }
        List<double> StartLimit1s { get; set; }
        List<double> EndLimit1s { get; set; }
        List<double> StartLimit2s { get; set; }
        List<double> EndLimit2s { get; set; }

        List<double> RebarDevelopmentLengths { get; set; }
        List<StirrupDistribution> StirrupDistributions { get; set; }
        List<StandardCreatingEnum> StandardCreatingTypes { get; set; }
        void GetInformation();
        void GetRebarInformation();
        void CheckLockheadInformation();
    }
    public interface IDesignInfo
    {
        int ID { get; set; }
        Level Level { get; set; }
        List<RebarBarType> StandardTypes { get; set; }
        List<double> StandardDiameters { get; }
        List<RebarHookType> StandardHookTypes { get; set; }
        List<int> StandardNumbers { get; set; }
        List<double> StandardSpacings { get; set; }
        List<RebarBarType> StirrupTypes { get; set; }
        List<double> StirrupDiameters { get; }
        List<double> BotTopSpacings { get; set; }
        List<double> MiddleSpacings { get; set; }
        void GetStandardSpacing();
    }
    public enum ShortenEnum
    {
        None=0,
        Small=1,
        Big =2
    }

    public interface IShortenType
    {
        bool IsLockhealAll { get; }
        ShortenEnum ShortenU1 { get; set; }
        ShortenEnum ShortenU2 { get; set; }
        ShortenEnum ShortenV1 { get; set; }
        ShortenEnum ShortenV2 { get; set; }
        double DeltaU1 { get; set; }
        double DeltaU2 { get; set; }
        double DeltaV1 { get; set; }
        double DeltaV2 { get; set; }
    }
    
    public enum RebarLocation
    {
        L1, L2
    }
    public interface IStandardPlaneSingleInfo
    {
        int ID { get; set; }
        XYZ Normal { get; set; }
        int Number { get; set; }
        double Spacing { get; set; }
        double ArrayLength { get; }
        UV StartPoint { get; set; }
        RebarLocation RebarLocation { get; set; }
        int LocationIndex { get; set; }
        Rebar CreateRebar(int idTurn, int locIndex);
    }
    public interface IStandardParameter
    {
        List<string> ParameterNames { get; set; }
        List<object> ParameterValues { get; set; }
    }
    public interface IStandardPlaneInfo
    {
        int ID { get; set; }
        List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        List<IStandardPlaneSingleInfo> LockheadStandardPlaneInfos { get; set; }
        void CreateRebar(int idTurn, int locIndex);
    }
    public interface IElementTypeInfo
    {
        int ID { get; set; }
        ElementTypeEnum Type { get; set; }
        int LocationCount { get; set; }
    }
    public enum StandardCreatingEnum
    {
        Normal = 0,
        Lockhead = 1
    }
}
