using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
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
using AutoRebaring.RebarLogistic;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.ElementInfo.RebarInfo.StirrupInfo;

namespace AutoRebaring.ElementInfo
{
    public static class ElementInfoUtils
    {
        public static void AddElementTypeInfo()
        {
            var obj = new ElementTypeInfo(ElementTypeEnum.Column, 1);
            Singleton.Instance.AddElementTypeInfo(obj);

            obj = new ElementTypeInfo(ElementTypeEnum.Wall, 2);
            Singleton.Instance.AddElementTypeInfo(obj);
        }
        public static void PickElement(Document doc, Selection sel)
        {
            Element e = doc.GetElement(sel.PickObject(ObjectType.Element, new WallAndColumnSelection()));
            Singleton.Instance.Document = doc;
            Singleton.Instance.Element = e;
        }
        public static void GetRelatedElements()
        {
            List<IRevitInfo> revitInfos = new List<IRevitInfo>();

            Element e = Singleton.Instance.Element;
            Document doc = Singleton.Instance.Document;
            ARLevel startLevel = Singleton.Instance.StartLevel;
            ARLevel endLevel = Singleton.Instance.EndLevel;

            PlaneInfo pi = new PlaneInfo(doc, e);
            UV p1 = pi.CentralPoint - pi.VectorU * pi.B1 / 2 - pi.VectorV * pi.B2 / 2;
            UV p2 = pi.CentralPoint + pi.VectorU * pi.B1 / 2 + pi.VectorV * pi.B2 / 2;
            List<Element> elemCols = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x != null).Where(x => (x is Wall) || (x is FamilyInstance && x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns)).ToList();
            List<Element> elems = new List<Element>();
            foreach (Element eA in elemCols)
            {
                Level sLevel = null, eLevel = null;
                if (eA is Wall)
                {
                    sLevel = doc.GetElement(eA.LookupParameter(ConstantValue.StartLevelWall).AsElementId()) as Level;
                    eLevel = doc.GetElement(eA.LookupParameter(ConstantValue.EndLevelWall).AsElementId()) as Level;
                }
                else
                {
                    sLevel = doc.GetElement(eA.LookupParameter(ConstantValue.StartLevelColumn).AsElementId()) as Level;
                    eLevel = doc.GetElement(eA.LookupParameter(ConstantValue.EndLevelColumn).AsElementId()) as Level;
                }
                string startLvl = startLevel.Name, endLvl = endLevel.Name;
                double startEle = startLevel.Elevation * ConstantValue.milimeter2Feet, endEle = endLevel.Elevation * ConstantValue.milimeter2Feet;
                if (sLevel == null || eLevel == null) continue;
                string sLvl = sLevel.Name, eLvl = eLevel.Name;
                double sEle = sLevel.Elevation, eEle = eLevel.Elevation;
                if (GeomUtil.IsEqualOrBigger(sLevel.Elevation, startEle) && GeomUtil.IsEqualOrSmaller(eLevel.Elevation, endEle))
                {
                    PlaneInfo piA = new PlaneInfo(doc, eA);
                    UV p1A = piA.CentralPoint - piA.VectorU * piA.B1 / 2 - piA.VectorV * piA.B2 / 2;
                    UV p2A = piA.CentralPoint + piA.VectorU * piA.B1 / 2 + piA.VectorV * piA.B2 / 2;
                    if (GeomUtil.IsEqualOrSmaller(p1.U, p2A.U) && GeomUtil.IsEqualOrSmaller(p1A.U, p2.U) && GeomUtil.IsEqualOrSmaller(p1.V, p2A.V) && GeomUtil.IsEqualOrSmaller(p1A.V, p2.V))
                    {
                        revitInfos.Add(new RevitInfo(doc, eA));
                    }
                }
            }

            revitInfos.Sort(new RevitInfoSorter());
            for (int i = 0; i < revitInfos.Count; i++)
            {
                revitInfos[i].ID = i;
                Singleton.Instance.AddRevitInfo(revitInfos[i]);
            }
        }
        public static void GetAllParameters()
        {
            ElementTypeEnum elemTypeEnum = Singleton.Instance.GetElementTypeEnum();

            // F1
            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                IPlaneInfo planeInfo = null;
                IDesignInfo designInfo = null;
                IVerticalInfo verticalInfo = null;
                IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(i);

                switch (elemTypeEnum)
                {
                    case ElementTypeEnum.Column:
                        planeInfo = new ColumnPlaneInfo(i);
                        verticalInfo = new ColumnVerticalInfo(i);
                        break;
                    case ElementTypeEnum.Wall:
                        planeInfo = new WallPlaneInfo(i);
                        verticalInfo = new WallVerticalInfo(i);
                        break;
                }

                bool isTop = true;
                for (int j = 0; j < Singleton.Instance.DesignInfos.Count; j++)
                {
                    IDesignInfo tempDesInfo = Singleton.Instance.DesignInfos[j];
                    if (GeomUtil.IsEqual(revitInfo.Elevation, tempDesInfo.Level.Elevation))
                    {
                        switch (elemTypeEnum)
                        {
                            case ElementTypeEnum.Column:
                                designInfo = new ColumnDesignInfo(tempDesInfo, revitInfo.Level);
                                break;
                            case ElementTypeEnum.Wall:
                                designInfo = new WallDesignInfo(tempDesInfo, revitInfo.Level);
                                break;
                        }
                        designInfo.ID = i;
                        isTop = false;
                        break;
                    }
                    else if (GeomUtil.IsSmaller(revitInfo.Elevation, tempDesInfo.Level.Elevation))
                    {
                        switch (elemTypeEnum)
                        {
                            case ElementTypeEnum.Column:
                                designInfo = new ColumnDesignInfo(Singleton.Instance.DesignInfos[j - 1], revitInfo.Level);
                                break;
                            case ElementTypeEnum.Wall:
                                designInfo = new WallDesignInfo(Singleton.Instance.DesignInfos[j - 1], revitInfo.Level);
                                break;
                        }
                        designInfo.ID = i;
                        isTop = false;
                        break;
                    }
                }
                if (isTop)
                {
                    switch (elemTypeEnum)
                    {
                        case ElementTypeEnum.Column:
                            designInfo = new ColumnDesignInfo(Singleton.Instance.DesignInfos[Singleton.Instance.DesignInfos.Count - 1], revitInfo.Level);
                            break;
                        case ElementTypeEnum.Wall:
                            designInfo = new WallDesignInfo(Singleton.Instance.DesignInfos[Singleton.Instance.DesignInfos.Count - 1], revitInfo.Level);
                            break;
                    }
                    designInfo.ID = i;
                }

                Singleton.Instance.AddPlaneInfo(planeInfo);
                Singleton.Instance.AddVerticalInfo(verticalInfo);
                Singleton.Instance.AddDesignInfo(designInfo);
            }

            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(i);
                IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(i);
                IVerticalInfo verticalInfo = Singleton.Instance.GetVerticalInfo(i);

                designInfo.GetStandardSpacing();
                planeInfo.GetRebarLocation();
                planeInfo.GetShortenType();
                verticalInfo.GetRebarInformation();
                verticalInfo.CheckLockheadInformation();

                Singleton.Instance.UpdatePlaneInfo(i, planeInfo);
                Singleton.Instance.UpdateDesignInfo(i, designInfo);
                Singleton.Instance.UpdateVerticalInfo(i, verticalInfo);
            }

            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                IStandardPlaneInfo standPlaneInfo = null;
                IStirrupPlaneInfo stirPlaneInfo = null;
                switch (elemTypeEnum)
                {
                    case ElementTypeEnum.Column:
                        standPlaneInfo = new ColumnStandardPlaneInfo(i);
                        stirPlaneInfo = new ColumnStirrupPlaneInfo(i);
                        break;
                    case ElementTypeEnum.Wall:
                        standPlaneInfo = new WallStandardPlaneInfo(i);
                        break;
                }
                Singleton.Instance.AddStandardPlaneInfo(standPlaneInfo);
                Singleton.Instance.AddStirrupPlaneInfo(stirPlaneInfo);
            }
        }
        public static void GetVariable()
        {
            VariableStandard vs = new VariableStandard();
            Singleton.Instance.VariableStandard = vs;

            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                for (int j = 0; j < Singleton.Instance.GetElementTypeInfo().LocationCount; j++)
                {
                    VariableImplant vi = new VariableImplant(i, j);
                    Singleton.Instance.AddVariableImplant(vi);
                }

            }
        }
        public static void GetDetailDistribution()
        {
            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                for (int j = 0; j < Singleton.Instance.GetStirrupDistribuitionsCount(i); j++)
                {
                    StirrupDistribution.GetDetailDistribution(j, i);
                }
            }
        }
        public static void AddTestInformationColumn(int first, int second)
        {
            Document doc = Singleton.Instance.Document;
            List<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
            List<RebarBarType> barTypes = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>().ToList();

            ARCoverParameter cp = new ARCoverParameter()
            {
                ConcreteCover = 35
            };
            ARAnchorParameter ap = new ARAnchorParameter()
            {
                AnchorMultiply = 40
            };
            ARDevelopmentParameter dp = new ARDevelopmentParameter()
            {
                DevelopmentMultiply = 40,
                DevelopmentLengthsDistance = 0,
                DeltaDevelopmentError = 0,
                NumberDevelopmentError = 0,
                DevelopmentErrorInclude = false,
                DevelopmentLevelOffsetAllowed = 0,
                DevelopmentLevelOffsetInclude = true,
                ReinforcementStirrupInclude = true
            };
            ARLockheadParameter lp = new ARLockheadParameter()
            {
                ShortenLimit = 100,
                LockheadMutiply = 20,
                LockheadConcreteCover = 100,
                SmallConcreteCover = 50,
                LHRatio = 6
            };
            RebarBarType stirType = barTypes.Where(x => x.Name == "T10").First();
            double tbSpac = 100 * ConstantValue.milimeter2Feet;
            double mSpac = 200 * ConstantValue.milimeter2Feet;
            List<IDesignInfo> desInfos = new List<IDesignInfo>()
            {
                new ColumnDesignInfo()
                {
                    Level = levels.Where(x=> x.Name=="Tầng 3").First(),
                    StandardTypes = barTypes.Where(x=> x.Name=="T20").ToList(),
                    StandardHookTypes = new List<RebarHookType>{null},
                    StandardNumbers = new List<int>{first, first},
                    StirrupTypes = new List<RebarBarType>{stirType, stirType},
                    BotTopSpacings = new List<double>{tbSpac, tbSpac},
                    MiddleSpacings = new List<double>{mSpac, mSpac}
                },
                new ColumnDesignInfo()
                {
                    Level = levels.Where(x=> x.Name=="Tầng 4").First(),
                    StandardTypes = barTypes.Where(x=> x.Name=="T18").ToList(),
                    StandardHookTypes = new List<RebarHookType>{null},
                    StandardNumbers = new List<int>{second, second},
                    StirrupTypes = new List<RebarBarType>{stirType, stirType},
                    BotTopSpacings = new List<double>{tbSpac, tbSpac},
                    MiddleSpacings = new List<double>{mSpac, mSpac}
                },
            };
            List<double> fitStands = new List<double>
            {
                5850 * ConstantValue.milimeter2Feet, 3900 *ConstantValue.milimeter2Feet,
                2925 * ConstantValue.milimeter2Feet, 2340 * ConstantValue.milimeter2Feet
            };
            List<double> pairFitStands = new List<double>
            {
                11700 *ConstantValue.milimeter2Feet, 7800 * ConstantValue.milimeter2Feet,
                5850 * ConstantValue.milimeter2Feet
            };
            List<double> tripFitStands = new List<double>
            {
                11700 *ConstantValue.milimeter2Feet, 7800 * ConstantValue.milimeter2Feet
            };
            List<double> fitImplants = new List<double>
            {
                3900 * ConstantValue.milimeter2Feet, 2925 *ConstantValue.milimeter2Feet,
                2340 * ConstantValue.milimeter2Feet, 1950 * ConstantValue.milimeter2Feet
            };
            List<double> pairFitImplants = new List<double>
            {
                5850 * ConstantValue.milimeter2Feet, 3900 *ConstantValue.milimeter2Feet,
                2925 * ConstantValue.milimeter2Feet
            };
            ARLevel startLevel = new ARLevel()
            {
                Name = "Tầng 3",
                Elevation = 6600,
                Title = "T3"
            };
            ARLevel endLevel = new ARLevel()
            {
                Name = "Tầng 5",
                Elevation = 13200,
                Title = "53"
            };
            ARStandardChosen sc = new ARStandardChosen()
            {
                Lmax = 7800,
                Lmin = 1900,
                Step = 100,
                LImplantMax = 3900
            };
            List<double> rebarZ1s = new List<double> {
                (startLevel.Elevation + 1200) * ConstantValue.milimeter2Feet };
            List<double> rebarZ2s = new List<double> {
                (startLevel.Elevation + 2200) * ConstantValue.milimeter2Feet };
            ARRebarVerticalParameter svp = new ARRebarVerticalParameter()
            {
                BottomOffset = 0,
                BottomOffsetRatio = 1,
                TopOffset = 0,
                TopOffsetRatio = 1,
                OffsetInclude = false,
                OffsetRatioInclude = false,
                IsInsideBeam = false
            };
            ARRebarVerticalParameter stvp = new ARRebarVerticalParameter()
            {
                BottomOffset = 0,
                BottomOffsetRatio = 6,
                TopOffset = 0,
                TopOffsetRatio = 6,
                OffsetInclude = false,
                OffsetRatioInclude = true,
                IsInsideBeam = false
            };

            Singleton.Instance.CoverParameter = cp;
            Singleton.Instance.AnchorParameter = ap;
            Singleton.Instance.DevelopmentParameter = dp;
            Singleton.Instance.LockheadParameter = lp;
            Singleton.Instance.DesignInfos = desInfos;
            Singleton.Instance.FitStandards = fitStands;
            Singleton.Instance.PairFitStandards = pairFitStands;
            Singleton.Instance.TripFitStandards = tripFitStands;
            Singleton.Instance.FitImplants = fitImplants;
            Singleton.Instance.PairFitImplants = pairFitImplants;
            Singleton.Instance.StartLevel = startLevel;
            Singleton.Instance.EndLevel = endLevel;
            Singleton.Instance.StandardChosen = sc;
            Singleton.Instance.RebarZ1s = rebarZ1s;
            Singleton.Instance.RebarZ2s = rebarZ2s;
            Singleton.Instance.StandardVeticalParameter = svp;
            Singleton.Instance.StirrupVerticalParameter = stvp;
        }
        public static void AddTestInformationWall(int ne11B, int ne12B, int ce12B, int ne2B, int de2B, int nmB,
            int ne11T, int ne12T, int ce12T, int ne2T, int de2T, int nmT)
        {
            Document doc = Singleton.Instance.Document;
            List<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
            List<RebarBarType> barTypes = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>().ToList();

            ARCoverParameter cp = new ARCoverParameter()
            {
                ConcreteCover = 35
            };
            ARAnchorParameter ap = new ARAnchorParameter()
            {
                AnchorMultiply = 40
            };
            ARDevelopmentParameter dp = new ARDevelopmentParameter()
            {
                DevelopmentMultiply = 40,
                DevelopmentLengthsDistance = 0,
                DeltaDevelopmentError = 0,
                NumberDevelopmentError = 0,
                DevelopmentErrorInclude = false,
                DevelopmentLevelOffsetAllowed = 0,
                DevelopmentLevelOffsetInclude = true,
                ReinforcementStirrupInclude = true
            };
            ARLockheadParameter lp = new ARLockheadParameter()
            {
                ShortenLimit = 100,
                LockheadMutiply = 20,
                LockheadConcreteCover = 100,
                SmallConcreteCover = 50,
                LHRatio = 6
            };
            RebarBarType stirType = barTypes.Where(x => x.Name == "T10").First();
            RebarBarType barType18 = barTypes.Where(x => x.Name == "T18").First();
            RebarBarType barType20 = barTypes.Where(x => x.Name == "T20").First();
            RebarBarType barType22 = barTypes.Where(x => x.Name == "T22").First();
            double tbSpac = 100 * ConstantValue.milimeter2Feet;
            double mSpac = 200 * ConstantValue.milimeter2Feet;
            List<IDesignInfo> desInfos = new List<IDesignInfo>()
            {
                new WallDesignInfo()
                {
                    Level = levels.Where(x=> x.Name=="Tầng 3").First(),
                    StandardTypes = new List<RebarBarType>{barType18, barType20 },
                    StandardHookTypes = new List<RebarHookType>{null, null},
                    StandardNumbers = new List<int>{ne11B, ne12B, ce12B, ne2B, de2B, nmB},
                    StirrupTypes = new List<RebarBarType>{stirType,stirType, stirType},
                    BotTopSpacings = new List<double>{tbSpac, tbSpac},
                    MiddleSpacings = new List<double>{mSpac, mSpac}
                },
                new WallDesignInfo()
                {
                    Level = levels.Where(x=> x.Name=="Tầng 4").First(),
                    StandardTypes = new List<RebarBarType>{barType18, barType20 },
                    StandardHookTypes = new List<RebarHookType>{null, null},
                    StandardNumbers = new List<int>{ne11T, ne12T, ce12T, ne2T, de2T, nmT},
                    StirrupTypes = new List<RebarBarType>{stirType, stirType, stirType},
                    BotTopSpacings = new List<double>{tbSpac, tbSpac},
                    MiddleSpacings = new List<double>{mSpac, mSpac}
                },
            };
            List<double> fitStands = new List<double>
            {
                5850 * ConstantValue.milimeter2Feet, 3900 *ConstantValue.milimeter2Feet,
                2925 * ConstantValue.milimeter2Feet, 2340 * ConstantValue.milimeter2Feet
            };
            List<double> pairFitStands = new List<double>
            {
                11700 *ConstantValue.milimeter2Feet, 7800 * ConstantValue.milimeter2Feet,
                5850 * ConstantValue.milimeter2Feet
            };
            List<double> tripFitStands = new List<double>
            {
                11700 *ConstantValue.milimeter2Feet, 7800 * ConstantValue.milimeter2Feet
            };
            List<double> fitImplants = new List<double>
            {
                3900 * ConstantValue.milimeter2Feet, 2925 *ConstantValue.milimeter2Feet,
                2340 * ConstantValue.milimeter2Feet, 1950 * ConstantValue.milimeter2Feet
            };
            List<double> pairFitImplants = new List<double>
            {
                5850 * ConstantValue.milimeter2Feet, 3900 *ConstantValue.milimeter2Feet,
                2925 * ConstantValue.milimeter2Feet
            };
            ARLevel startLevel = new ARLevel()
            {
                Name = "Tầng 3",
                Elevation = 6600,
                Title = "T3"
            };
            ARLevel endLevel = new ARLevel()
            {
                Name = "Tầng 5",
                Elevation = 13200,
                Title = "53"
            };
            ARStandardChosen sc = new ARStandardChosen()
            {
                Lmax = 7800,
                Lmin = 1900,
                Step = 100,
                LImplantMax = 3900
            };
            List<double> rebarZ1s = new List<double> {
                (startLevel.Elevation + 1200) * ConstantValue.milimeter2Feet , (startLevel.Elevation + 1500) * ConstantValue.milimeter2Feet};
            List<double> rebarZ2s = new List<double> {
                (startLevel.Elevation + 2200) * ConstantValue.milimeter2Feet , (startLevel.Elevation + 2500) * ConstantValue.milimeter2Feet};
            ARRebarVerticalParameter svp = new ARRebarVerticalParameter()
            {
                BottomOffset = 0,
                BottomOffsetRatio = 1,
                TopOffset = 0,
                TopOffsetRatio = 1,
                OffsetInclude = false,
                OffsetRatioInclude = false,
                IsInsideBeam = false
            };
            ARRebarVerticalParameter stvp = new ARRebarVerticalParameter()
            {
                BottomOffset = 0,
                BottomOffsetRatio = 6,
                TopOffset = 0,
                TopOffsetRatio = 6,
                OffsetInclude = false,
                OffsetRatioInclude = true,
                IsInsideBeam = false
            };
            ARWallParameter wp = new ARWallParameter()
            {
                EdgeWidth = 0,
                EdgeWidthInclude = false,
                EdgeRatio = 5,
                EdgeRatioInclude = true
            };

            Singleton.Instance.CoverParameter = cp;
            Singleton.Instance.AnchorParameter = ap;
            Singleton.Instance.DevelopmentParameter = dp;
            Singleton.Instance.LockheadParameter = lp;
            Singleton.Instance.DesignInfos = desInfos;
            Singleton.Instance.FitStandards = fitStands;
            Singleton.Instance.PairFitStandards = pairFitStands;
            Singleton.Instance.TripFitStandards = tripFitStands;
            Singleton.Instance.FitImplants = fitImplants;
            Singleton.Instance.PairFitImplants = pairFitImplants;
            Singleton.Instance.StartLevel = startLevel;
            Singleton.Instance.EndLevel = endLevel;
            Singleton.Instance.StandardChosen = sc;
            Singleton.Instance.RebarZ1s = rebarZ1s;
            Singleton.Instance.RebarZ2s = rebarZ2s;
            Singleton.Instance.StandardVeticalParameter = svp;
            Singleton.Instance.StirrupVerticalParameter = stvp;
            Singleton.Instance.WallParameter = wp;
            Singleton.Instance.SetElementTypeInfoID(ElementTypeEnum.Wall);
        }
        public static void SetupView3d(View3D view3d, Rebar rebar)
        {
            rebar.SetSolidInView(view3d, true);
            rebar.SetUnobscuredInView(view3d, true);
        }
    }

    public class RevitInfoSorter : IComparer<IRevitInfo>
    {
        public RevitInfoSorter() { }
        public int Compare(IRevitInfo first, IRevitInfo second)
        {
            return first.Elevation.CompareTo(second.Elevation);
        }
    }
}
