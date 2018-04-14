using Autodesk.Revit.DB;
using AutoRebaring.Constant;
using AutoRebaring.Database;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class VerticalInfo : IVerticalInfo
    {
        #region VerticalInfo
        public Level StartLevel { get; set; }
        public Level EndLevel { get; set; }
        public double Top { get; set; }
        public double TopBeam { get; set; }
        public double TopFloor { get; set; }
        public double TopOffset { get; set; }
        public double TopLimit { get; set; }
        public double TopLockHead { get; set; }
        public double TopSmall { get; set; }
        public List<double> TopAnchorAfters { get; set; }
        public double TopStirrup1 { get; set; }
        public double TopStirrup2 { get; set; }
        public double Bottom { get; set; }
        public double BottomOffset { get; set; }
        public double BottomStirrup1 { get; set; }
        public double BottomStirrup2 { get; set; }
        public List<double> RebarDevelopmentLengths { get; set; }
        public List<StirrupDistribution> StirrupDistributions { get; set; }
        #endregion

        public VerticalInfo(IRevitInfo revitInfo)
        {
            Element e = revitInfo.Element;
            Document doc = revitInfo.Document;

            BoundingBoxXYZ bb = e.get_BoundingBox(null);
            Outline ol = new Outline(bb.Min, bb.Max);
            XYZ midPnt = new XYZ((bb.Min.X + bb.Max.X) / 2, (bb.Min.Y + bb.Max.Y) / 2, (bb.Min.Z + bb.Max.Z) / 2);

            Options opt = new Options();
            GeometryElement geoElem = e.get_Geometry(opt);
            Solid s = null;
            foreach (GeometryObject geoObj in geoElem)
            {
                if (geoObj is Solid)
                {
                    s = geoObj as Solid;
                    if (s != null)
                    {
                        if (s.Faces.Size != 0 && s.Edges.Size != 0)
                        {
                            break;
                        }
                    }
                }
            }

            XYZ centralPnt = s.ComputeCentroid();
            Transform tf = Transform.Identity;
            tf.BasisX = XYZ.BasisX * 1.025;
            tf.BasisY = XYZ.BasisY * 1.025;
            tf.BasisZ = XYZ.BasisZ * 1.025;
            s = SolidUtils.CreateTransformed(s, tf);

            tf = Transform.CreateTranslation(centralPnt - s.ComputeCentroid());
            s = SolidUtils.CreateTransformed(s, tf);
            XYZ cen2 = s.ComputeCentroid();

            BoundingBoxIntersectsFilter bbiFilter = new BoundingBoxIntersectsFilter(ol);
            ElementIntersectsSolidFilter eisFilter = new ElementIntersectsSolidFilter(s);
            ElementClassFilter flFilter = new ElementClassFilter(typeof(Floor));
            ElementClassFilter fiFilter = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter beamCateFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            LogicalAndFilter beamFilter = new LogicalAndFilter(new List<ElementFilter> { fiFilter, beamCateFilter });
            LogicalOrFilter floorOrBeamFilter = new LogicalOrFilter(new List<ElementFilter> { flFilter, beamFilter });
            List<Element> elems = new FilteredElementCollector(doc).WherePasses(floorOrBeamFilter).WherePasses(bbiFilter).WherePasses(eisFilter).ToList();

            double middle = centralPnt.Z;
            double min = 0, maxLim = 0, maxFloor = 0, maxBeam =0;
            bool firstSetMin = true, firstSetMaxLim = true, firstSetMaxFloor = true, firstSetMaxBeam = true;
            for (int i = 0; i < elems.Count; i++)
            {
                BoundingBoxXYZ bbe = elems[i].get_BoundingBox(null);
                double minZ = bbe.Max.Z, maxLimZ = bbe.Max.Z, maxFloorZ = 0, maxBeamZ = 0;

                if (maxLimZ > middle)
                {
                    if (firstSetMaxLim)
                    {
                        maxLim = maxLimZ;
                        firstSetMaxLim = false;
                    }
                    else
                    {
                        if (maxLim < maxLimZ)
                            maxLim = maxLimZ;
                    }
                }
                if (elems[i] is FamilyInstance)
                {
                    if (maxBeamZ > middle)
                    {
                        if (firstSetMaxBeam)
                        {
                            maxBeam = maxBeamZ;
                            firstSetMaxBeam = false;
                        }
                        else
                        {
                            if (maxBeam > maxBeamZ)
                                maxBeam = maxBeamZ;
                        }
                    }
                }
                if (elems[i] is Floor)
                {
                    if (maxFloorZ > middle)
                    {
                        if (firstSetMaxFloor)
                        {
                            maxFloor = maxFloorZ;
                            firstSetMaxFloor = false;
                        }
                        else
                        {
                            if (maxFloor > maxFloorZ)
                                maxFloor = maxFloorZ;
                        }
                    }
                }
            }

            TopBeam = maxBeam; TopFloor = maxFloor; TopLimit = maxLim; Bottom = min;
            if (firstSetMin) Bottom = bb.Min.Z;
            if (firstSetMaxBeam) TopBeam = bb.Max.Z;
            if (firstSetMaxFloor) TopFloor = bb.Max.Z;
            if (firstSetMaxLim) TopLimit = bb.Max.Z;

            if (e is Wall)
            {
                StartLevel = doc.GetElement(e.LookupParameter(ConstantValue.StartLevelWall).AsElementId()) as Level;
                EndLevel = doc.GetElement(e.LookupParameter(ConstantValue.EndLevelWall).AsElementId()) as Level;
            }
            else
            {
                StartLevel = doc.GetElement(e.LookupParameter(ConstantValue.StartLevelColumn).AsElementId()) as Level;
                EndLevel = doc.GetElement(e.LookupParameter(ConstantValue.EndLevelColumn).AsElementId()) as Level;
            }
        }
        public void GetInformation(ARRebarVerticalParameter rvpStand, ARRebarVerticalParameter rvpStirr, ARLockheadParameter lpp)
        {
            Top = rvpStand.IsInsideBeam ? TopFloor : TopBeam;

            double d = rvpStand.OffsetInclude ? rvpStand.TopOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStand.OffsetRatioInclude ? Math.Max(d, (Top - Bottom) * rvpStand.TopOffsetRatio) : d;
            TopOffset = Top - d;

            TopLockHead = Top - lpp.LockheadConcreteCover * ConstantValue.milimeter2Feet;
            TopSmall = Top - lpp.SmallConcreteCover * ConstantValue.milimeter2Feet;

            d = rvpStand.OffsetInclude ? rvpStand.BottomOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStand.OffsetRatioInclude ? Math.Max(d, (Top - Bottom) * rvpStand.BottomOffsetRatio) : d;
            BottomOffset = Bottom + d;

            TopStirrup2 = rvpStirr.IsInsideBeam ? TopFloor : TopBeam;
            BottomStirrup1 = Bottom;

            d = rvpStirr.OffsetInclude ? rvpStirr.TopOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStirr.OffsetRatioInclude ? Math.Max(d, (TopStirrup2 - BottomStirrup1) * rvpStirr.TopOffsetRatio) : d;
            TopStirrup2 = TopStirrup1 - d;

            d = rvpStirr.OffsetInclude ? rvpStirr.BottomOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStirr.OffsetRatioInclude ? Math.Max(d, (TopStirrup2 - BottomStirrup1) * rvpStirr.BottomOffsetRatio) : d;
            BottomStirrup2 = BottomStirrup1 + d;
        }
        public void GetRebarInformation(IDesignInfo di, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            TopAnchorAfters = di.DesignInfoAfter.StandardDiameters.
                Select(x => ap.AnchorMultiply * x).ToList();
            RebarDevelopmentLengths = di.StandardDiameters.
                Select(x => dp.DevelopmentMultiply * x).ToList();
        }
    }
    public class ColumnVerticalInfo : VerticalInfo
    {
        public ColumnVerticalInfo(IRevitInfo revitInfo) : base(revitInfo)
        {
        }
    }
    public class WallVerticalInfo : VerticalInfo
    {
        public WallVerticalInfo(IRevitInfo revitInfo) : base(revitInfo)
        {
        }
    }
}
