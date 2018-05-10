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
using AutoRebaring.Single;

namespace AutoRebaring.ElementInfo
{
    public class VerticalInfo : IVerticalInfo
    {
        #region VerticalInfo
        public int ID { get; set; }
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
        public double BottomOffsetValue { get; set; }
        public List<double> EndLimit0s { get; set; }
        public List<double> StartLimit1s { get; set; }
        public List<double> EndLimit1s { get; set; }
        public List<double> StartLimit2s { get; set; }
        public List<double> EndLimit2s { get; set; }
        public List<StandardCreatingEnum> StandardCreatingTypes { get; set; } = new List<StandardCreatingEnum>();
        #endregion

        public VerticalInfo(int id)
        {
            ID = id;
            GetGeometry();
            GetInformation();
        }
        public void GetGeometry()
        {
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(ID);
            Document doc = revitInfo.Document;
            Element e = revitInfo.Element;

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
                if (geoObj is GeometryInstance)
                {
                    GeometryInstance geoIns = geoObj as GeometryInstance;
                    GeometryElement geoElem2 = geoIns.GetInstanceGeometry();
                    foreach (GeometryObject geoObj2 in geoElem2)
                    {
                        s = geoObj2 as Solid;
                        if (s != null)
                        {
                            if (s.Faces.Size != 0 && s.Edges.Size != 0)
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
            double min = 0, maxLim = 0, maxFloor = 0, maxBeam = 0;
            bool firstSetMin = true, firstSetMaxLim = true, firstSetMaxFloor = true, firstSetMaxBeam = true;
            for (int i = 0; i < elems.Count; i++)
            {
                BoundingBoxXYZ bbe = elems[i].get_BoundingBox(null);
                double minZ = bbe.Max.Z, maxLimZ = bbe.Max.Z, maxFloorZ = bbe.Min.Z, maxBeamZ = bbe.Min.Z;

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
        public void GetInformation()
        {
            ARRebarVerticalParameter rvpStand = Singleton.Instance.StandardVeticalParameter;
            ARRebarVerticalParameter rvpStirr = Singleton.Instance.StirrupVerticalParameter;
            ARLockheadParameter lp = Singleton.Instance.LockheadParameter;

            Top = rvpStand.IsInsideBeam ? TopFloor : TopBeam;

            double d = rvpStand.OffsetInclude ? rvpStand.TopOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStand.OffsetRatioInclude ? Math.Max(d, (Top - Bottom) * rvpStand.TopOffsetRatio) : d;
            TopOffset = Top - d;

            TopLockHead = TopLimit - lp.LockheadConcreteCover * ConstantValue.milimeter2Feet;
            TopSmall = TopLimit - lp.SmallConcreteCover * ConstantValue.milimeter2Feet;

            d = rvpStand.OffsetInclude ? rvpStand.BottomOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStand.OffsetRatioInclude ? Math.Max(d, (Top - Bottom) * rvpStand.BottomOffsetRatio) : d;
            BottomOffset = Bottom + d;
            BottomOffsetValue = d;

            TopStirrup2 = rvpStirr.IsInsideBeam ? TopFloor : TopBeam;
            BottomStirrup1 = Bottom;

            d = rvpStirr.OffsetInclude ? rvpStirr.TopOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStirr.OffsetRatioInclude ? Math.Max(d, (TopStirrup2 - BottomStirrup1) * rvpStirr.TopOffsetRatio) : d;
            TopStirrup2 = TopStirrup1 - d;

            d = rvpStirr.OffsetInclude ? rvpStirr.BottomOffset * ConstantValue.milimeter2Feet : 0;
            d = rvpStirr.OffsetRatioInclude ? Math.Max(d, (TopStirrup2 - BottomStirrup1) * rvpStirr.BottomOffsetRatio) : d;
            BottomStirrup2 = BottomStirrup1 + d;

            //StirrupDistributions = new List<StirrupDistribution> { new StirrupDistribution(0,BottomStirrup1, BottomStirrup2), new StirrupDistribution(1,TopStirrup1, TopStirrup2) };
            StirrupDistribution sd1 = new StirrupDistribution()
            {
                ID = 0, IDElement = this.ID, Z1 = BottomStirrup1, Z2= BottomStirrup2
            };
            StirrupDistribution sd2 = new StirrupDistribution()
            {
                ID = 1, IDElement = this.ID, Z1 = TopStirrup1, Z2 = TopStirrup2
            };
            Singleton.Instance.AddStirrupDistribution(sd1);
            Singleton.Instance.AddStirrupDistribution(sd2);
        }
        public void GetRebarInformation()
        {
            IDesignInfo di = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo diA = Singleton.Instance.GetDesignInfoAfter(ID);
            ARAnchorParameter ap = Singleton.Instance.AnchorParameter;
            ARDevelopmentParameter dp = Singleton.Instance.DevelopmentParameter;

            TopAnchorAfters = diA.StandardDiameters.
                Select(x => TopLimit - ap.AnchorMultiply * x).ToList();
            RebarDevelopmentLengths = di.StandardDiameters.
                Select(x => dp.DevelopmentMultiply * x).ToList();
        }
        public void CheckLockheadInformation()
        {
            IDesignInfo di = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo diA = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo pi = Singleton.Instance.GetPlaneInfo(ID);

            for (int i = 0; i < di.StandardDiameters.Count; i++)
            {
                if (pi.ShortenTypes[i].IsLockhealAll || (ID == Singleton.Instance.GetElementCount() - 1) || GeomUtil.IsBigger(diA.StandardDiameters[i], di.StandardDiameters[i]))
                {
                    StandardCreatingTypes.Add(StandardCreatingEnum.Lockhead);
                }
                else
                {
                    StandardCreatingTypes.Add(StandardCreatingEnum.Normal);
                }
            }
        }

        //public void MergeStirrupDistribution(double z1, double z2)
        //{
        //    IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
        //    double midSpac1 = designInfo.MiddleSpacings[0];

        //    StirrupDistributions.Add(new StirrupDistribution(z1, z2));
        //    List<StirrupDistribution> stirDiss1 = StirrupDistributions;
        //    stirDiss1.Sort();
        //    while (true)
        //    {
        //        List<StirrupDistribution> stirDiss2 = new List<StirrupDistribution>();
        //        bool isMergeAll = false;
        //        for (int i = 0; i < stirDiss1.Count-1; i+=2)
        //        {
        //            bool isMerge = false;
        //            stirDiss2.AddRange(StirrupDistribution.CheckMerge(stirDiss1[i], stirDiss1[i + 1], midSpac1, out isMerge));
        //            if (isMerge) isMergeAll = true;
        //        }
        //        if (stirDiss1.Count % 2 == 1)
        //        {
        //            bool isMerge = false;
        //            List<StirrupDistribution> temDiss = StirrupDistribution.CheckMerge(stirDiss2[stirDiss2.Count - 1], stirDiss1[stirDiss1.Count - 1], midSpac1, out isMerge);
        //            if (isMerge)
        //            {
        //                if (stirDiss2.Count > 1) isMergeAll = true;
        //                else isMergeAll = false;
        //                stirDiss2[stirDiss2.Count - 1] = temDiss[0];
        //            }
        //            else stirDiss2.Add(temDiss[1]);
        //        }
        //        stirDiss1 = stirDiss2;
        //        if (!isMergeAll) break;
        //    }
        //    StirrupDistributions = stirDiss1;
        //}
    }
    public class ColumnVerticalInfo : VerticalInfo
    {
        public ColumnVerticalInfo(int id) : base(id)
        {
        }
    }
    public class WallVerticalInfo : VerticalInfo
    {
        public WallVerticalInfo(int id) : base(id)
        {
        }
    }
}
