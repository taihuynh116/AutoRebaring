using Autodesk.Revit.DB;
using AutoRebaring.Constant;
using AutoRebaring.Database;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo.SingleInfo;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo
{
    public class ColumnStandardPlaneInfo
    {
        public IPlaneInfo PlaneInfo { get; set; }
        public IDesignInfo DesignInfo { get; set; }
        public GeneralParameterInput GeneralParameterInput { get; set; }
        public IRebarGeneralInfo StandardGeneralInfo { get; set; }
        public IRebarGeneralInfo StirrupGeneralInfo { get; set; }
        public List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ShortenStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ImplantStandardPlaneInfos { get; set; }
        public ColumnStandardPlaneInfo(IPlaneInfo planeInfo, IDesignInfo designInfo, IRebarGeneralInfo standGenInfo, IRebarGeneralInfo stirGenInfo, GeneralParameterInput gpi)
        {
            PlaneInfo = planeInfo;
            DesignInfo = designInfo;
            StandardGeneralInfo = standGenInfo;
            StirrupGeneralInfo = stirGenInfo;
            GeneralParameterInput = gpi;

            GetNormalStandardPlaneInfos();
        }
        private void GetNormalStandardPlaneInfos()
        {
            int n1 = DesignInfo.StandardNumbers[0];
            int n2 = DesignInfo.StandardNumbers[1];
            double spac1 = DesignInfo.StandardSpacings[0];
            double spac2 = DesignInfo.StandardSpacings[1];
            List<UV> pnts = PlaneInfo.BoundaryPointLists[0];
            XYZ vecX = PlaneInfo.VectorX;
            XYZ vecY = PlaneInfo.VectorY;
            UV vecU = PlaneInfo.VectorU;
            UV vecV = PlaneInfo.VectorV;

            NormalStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();

            IStandardPlaneSingleInfo stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0],
                Spacing = spac1,
                Number = (n1%2==0)?n1/2:n1/2+1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[3],
                Spacing = spac1,
                Number = (n1 % 2 == 0) ? n1 / 2 : n1 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecU*spac1/2,
                Spacing = spac1,
                Number = n1/2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[3] + vecU * spac1 / 2,
                Spacing = spac1,
                Number = n1 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * spac2,
                Spacing = spac2,
                Number = (n2-2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[1] + vecV * spac2,
                Spacing = spac2,
                Number = (n2-2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * spac2/2,
                Spacing = spac2,
                Number = (n2 % 2 == 0) ? (n2-2) / 2 : (n2 - 2) / 2+1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[1] + vecV * spac2/2,
                Spacing = spac2,
                Number = (n2 % 2 == 0) ? (n2 - 2) / 2 : (n2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);
        }
        private void GetShortenStandardPlaneInfos()
        {
            ShortenStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();

        }
        private void GetShortenV(int index, ShortenEnum shorten)
        {
            int n1 = DesignInfo.StandardNumbers[0];
            int n2 = DesignInfo.StandardNumbers[1];
            int nA1 = DesignInfo.DesingInfoAfter.StandardNumbers[0];
            int nA2 = DesignInfo.DesingInfoAfter.StandardNumbers[1];
            double spac1 = DesignInfo.StandardSpacings[0];
            double spac2 = DesignInfo.StandardSpacings[1];
            double spacA1 = DesignInfo.DesingInfoAfter.StandardSpacings[0];
            double spacA2 = DesignInfo.DesingInfoAfter.StandardSpacings[1];
            List<UV> pnts = PlaneInfo.BoundaryPointLists[0];
            List<UV> pntAs = PlaneInfo.PlaneInfoAfter.BoundaryPointLists[0];
            XYZ vecX = PlaneInfo.VectorX;
            XYZ vecY = PlaneInfo.VectorY;
            UV vecU = PlaneInfo.VectorU;
            UV vecV = PlaneInfo.VectorV;
            double dia = StandardGeneralInfo.Diameters[0];
            double diaAfter = StandardGeneralInfo.DiameterAfters[0];
            double shortenLimit = GeneralParameterInput.ShortenLimit * ConstantValue.milimeter2Feet;

            XYZ vecExpBig = index == 0 ? -vecY : vecY;
            XYZ vecExpSmall = index == 0 ? vecY : -vecY;
            double delV = (index == 0) ? PlaneInfo.ShortenTypes[0].DeltaV1 : PlaneInfo.ShortenTypes[0].DeltaV2;
            double delU1 = PlaneInfo.ShortenTypes[0].DeltaU1;
            double delU2 = PlaneInfo.ShortenTypes[0].DeltaU2;
            double dimExpSmall = delV + (diaAfter - dia) / 2;

            ShortenStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();
            switch (shorten)
            {
                case ShortenEnum.Big:
                    {
                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index],
                            Spacing = spac1,
                            Number = (n1 % 2 == 0) ? n1 / 2 : n1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU*spac1/2,
                            Spacing = spac1,
                            Number = n1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacA1,
                            Number = (nA1 % 2 == 0) ? nA1 / 2 : nA1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacA1,
                            Number = nA1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.Small:
                    {
                        int numBigU1 = 0, numSmallU1 = 0;
                        for (int i = 0; i < n1; i++)
                        {
                            double del= delU1 - spac1 / 2 * i;
                            if (GeomUtil.IsEqual(del, shortenLimit) || del > shortenLimit)
                            {
                                numBigU1 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallU1 = i + 1 - numBigU1;
                            }
                            else if (GeomUtil.IsEqual(del, 0) || del < 0)
                            {
                                break;
                            }
                        }

                        int numBigU2 = 0, numSmallU2 = 0;
                        for (int i = 0; i < n1; i++)
                        {
                            double del = delU2 - spac1 / 2 * i;
                            if (GeomUtil.IsEqual(del, shortenLimit) || del > shortenLimit)
                            {
                                numBigU2 = i + 1;
                            }
                            else if (GeomUtil.IsEqual(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallU2 = i + 1 - numBigU2;
                            }
                            else if (GeomUtil.IsEqual(del, 0) || del < 0)
                            {
                                break;
                            }
                        }

                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index],
                            Spacing = spac1,
                            Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.None:
                    {

                    }
                    break;
            }
        }
    }
    public class WallStandardPlaneInfo
    {
        public List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ShortenStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ImplantStandardPlaneInfos { get; set; }
    }
}
