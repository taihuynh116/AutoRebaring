using Autodesk.Revit.DB;
using AutoRebaring.Constant;
using AutoRebaring.Database;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo.SingleInfo;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.Single;
using AutoRebaring.RebarLogistic;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo
{
    public class ColumnStandardPlaneInfo : IStandardPlaneInfo
    {
        public int ID { get; set; }
        public List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> LockheadStandardPlaneInfos { get; set; }
        public ColumnStandardPlaneInfo(int id)
        {
            ID = id;

            GetShortenStandardPlaneInfos();
            GetLockheadStandardPlaneInfos();
        }
        private void GetShortenStandardPlaneInfos()
        {
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);

            IShortenType shortenType = planeInfo.ShortenTypes[0];
            NormalStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();
            GetShortenV(0, shortenType.ShortenV1);
            GetShortenV(3, shortenType.ShortenV2);
            GetShortenU(0, shortenType.ShortenU1);
            GetShortenU(1, shortenType.ShortenU2);
        }
        private void GetShortenV(int index, ShortenEnum shorten)
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);
            ARLockheadParameter lockheadParameter = Singleton.Instance.LockheadParameter;

            int n1 = designInfo.StandardNumbers[0];
            int n2 = designInfo.StandardNumbers[1];
            int nA1 = designInfoAfter.StandardNumbers[0];
            int nA2 = designInfoAfter.StandardNumbers[1];
            int n1Des = -1;
            int n2Des = -1;
            double spac1 = designInfo.StandardSpacings[0];
            double spac2 = designInfo.StandardSpacings[1];
            double spacA1 = designInfoAfter.StandardSpacings[0];
            double spacA2 = designInfoAfter.StandardSpacings[1];
            double spac1Des = -1;
            double spac2Des = -1;

            if (n1 < nA1)
            {
                n1Des = n1;
                spac1Des = spac1;
            }
            else
            {
                n1Des = nA1;
                spac1Des = spacA1;
            }

            if (n2 < nA2)
            {
                n2Des = n2;
                spac2Des = spac2;
            }
            else
            {
                n2Des = nA2;
                spac2Des = spacA2;
            }

            List<UV> pnts = planeInfo.StandardRebarPointLists[0];
            List<UV> pntAs = planeInfoAfter.StandardRebarPointLists[0];
            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            double dia = designInfo.StandardDiameters[0];
            double diaAfter = designInfoAfter.StandardDiameters[0];
            double shortenLimit = lockheadParameter.ShortenLimit * ConstantValue.milimeter2Feet;

            XYZ vecExpBig = index == 0 ? -vecY : vecY;
            XYZ vecExpSmall = index == 0 ? vecY : -vecY;
            double delV = (index == 0) ? planeInfo.ShortenTypes[0].DeltaV1 : planeInfo.ShortenTypes[0].DeltaV2;
            double delU1 = planeInfo.ShortenTypes[0].DeltaU1;
            double delU2 = planeInfo.ShortenTypes[0].DeltaU2;
            double dimExpSmall = delV + (diaAfter - dia) / 2;

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
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spac1 / 2,
                            Spacing = spac1,
                            Number = n1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacA1,
                            Number = (nA1 % 2 == 0) ? nA1 / 2 : nA1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacA1,
                            Number = nA1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.Small:
                    {
                        int numBigU1 = 0, numSmallU1 = 0;
                        for (int i = 0; i < n1Des; i++)
                        {
                            double del = delU1 - spac1Des / 2 * i;
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigU1 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallU1 = i + 1 - numBigU1;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        int numBigU2 = 0, numSmallU2 = 0;
                        for (int i = 0; i < n1Des; i++)
                        {
                            double del = delU2 - spac1Des / 2 * i;
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigU2 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallU2 = i + 1 - numBigU2;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        IStandardPlaneSingleInfo stPlSinInfo = null;
                        if (numBigU1 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index],
                                Spacing = spac1Des,
                                Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                                Normal = vecX,
                                RebarLocation = RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigU1 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2,
                                Spacing = spac1Des,
                                Number = numBigU1 / 2,
                                Normal = vecX,
                                RebarLocation = RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        int num = numBigU1, num2 = numSmallU1;
                        for (int i = 0; i < numSmallU1; i++)
                        {
                            UV uvExpSmall2 = vecU * (delU1 + (diaAfter - dia) / 2 - spac1Des / 2 * (numBigU1 + i)) + vecV * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + i),
                                Spacing = spac1Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n1 - numBigU1 - numBigU2 >= nA1)
                        {
                            num2 = n1Des - numSmallU1 - numSmallU2;
                        }
                        else
                        {
                            num2 = n1Des - numBigU1 - numSmallU1 - numBigU2 - numSmallU2;
                        }
                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spac1Des / 2 * num,
                            Spacing = spac1Des,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            CrackingDirection = vecExpSmall * dimExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + 1),
                            Spacing = spac1Des,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            CrackingDirection = vecExpSmall * dimExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallU2;
                        for (int i = 0; i < numSmallU2; i++)
                        {
                            UV uvExpSmall2 = -vecU * (delU2 + (diaAfter - dia) / 2 - spac1Des / 2 * (numBigU2 + (numSmallU2 - 1 - i))) + vecV * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + i),
                                Spacing = spac1Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigU2;
                        if (numBigU2 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * num,
                                Spacing = spac1Des,
                                Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                Normal = vecX,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigU2 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + 1),
                                Spacing = spac1Des,
                                Number = num2 / 2,
                                Normal = vecX,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n1 - numBigU1 - numBigU2 >= nA1)
                        {
                            num2 = n1 - nA1 - numBigU1 - numBigU2;
                            if (num2 >=1)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.25,
                                    Spacing = spac1Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.75,
                                    Spacing = spac1Des,
                                    Number = num2 / 2,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                        else
                        {
                            num2 = nA1 - n1 + numBigU1 + numBigU2;
                            if (num2 >= 1)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.25 + vecV * (delV + (diaAfter - dia) / 2) * (index == 0 ? 1 : -1),
                                    Spacing = spac1Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.75 + vecV * (delV + (diaAfter - dia) / 2) * (index == 0 ? 1 : -1),
                                    Spacing = spac1Des,
                                    Number = num2 / 2,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                    }
                    break;
                case ShortenEnum.None:
                    {
                        int numBigU1 = 0, numSmallU1 = 0;
                        for (int i = 0; i < n1Des; i++)
                        {
                            double del = delU1 - spac1Des / 2 * i;
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigU1 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallU1 = i + 1 - numBigU1;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        int numBigU2 = 0, numSmallU2 = 0;
                        for (int i = 0; i < n1Des; i++)
                        {
                            double del = delU2 - spac1Des / 2 * i;
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigU2 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallU2 = i + 1 - numBigU2;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        IStandardPlaneSingleInfo stPlSinInfo = null;
                        if (numBigU1 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index],
                                Spacing = spac1Des,
                                Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                                Normal = vecX,
                                RebarLocation = RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigU1 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2,
                                Spacing = spac1Des,
                                Number = numBigU1 / 2,
                                Normal = vecX,
                                RebarLocation = RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        int num = numBigU1, num2 = numSmallU1;
                        for (int i = 0; i < numSmallU1; i++)
                        {
                            UV uvExpSmall2 = vecU * (delU1 + (diaAfter - dia) / 2 - spac1Des / 2 * (numBigU1 + i));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + i),
                                Spacing = spac1Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n1 - numBigU1 - numBigU2 >= nA1)
                        {
                            num2 = n1Des - numSmallU1 - numSmallU2;
                        }
                        else
                        {
                            num2 = n1Des - numBigU1 - numSmallU1 - numBigU2 - numSmallU2;
                        }
                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spac1Des / 2 * num,
                            Spacing = spac1Des,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + 1),
                            Spacing = spac1Des,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallU2;
                        for (int i = 0; i < numSmallU2; i++)
                        {
                            UV uvExpSmall2 = -vecU * (delU2 + (diaAfter - dia) / 2 - spac1Des / 2 * (numBigU2 + (numSmallU2 - 1 - i)));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + i),
                                Spacing = spac1Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigU2;
                        if (numBigU2 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * num,
                                Spacing = spac1Des,
                                Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                Normal = vecX,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigU2 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spac1Des / 2 * (num + 1),
                                Spacing = spac1Des,
                                Number = num2 / 2,
                                Normal = vecX,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n1 - numBigU1 - numBigU2 >= nA1)
                        {
                            num2 = n1 - nA1 - numBigU1 - numBigU2;
                            if (num2 >= 1)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.25,
                                    Spacing = spac1Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.75,
                                    Spacing = spac1Des,
                                    Number = num2 / 2,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                        else
                        {
                            num2 = nA1 - n1 + numBigU1 + numBigU2;
                            if (num2 >= 1)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.25,
                                    Spacing = spac1Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecU * spac1Des * 0.75,
                                    Spacing = spac1Des,
                                    Number = num2 / 2,
                                    Normal = vecX,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                    }
                    break;
            }
        }
        private void GetShortenU(int index, ShortenEnum shorten)
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);
            ARLockheadParameter lockheadParameter = Singleton.Instance.LockheadParameter;

            int n1 = designInfo.StandardNumbers[0];
            int n2 = designInfo.StandardNumbers[1];
            int nA1 = designInfoAfter.StandardNumbers[0];
            int nA2 = designInfoAfter.StandardNumbers[1];
            int n1Des = -1;
            int n2Des = -1;
            double spac1 = designInfo.StandardSpacings[0];
            double spac2 = designInfo.StandardSpacings[1];
            double spacA1 = designInfoAfter.StandardSpacings[0];
            double spacA2 = designInfoAfter.StandardSpacings[1];
            double spac1Des = -1;
            double spac2Des = -1;

            if (n1 < nA1)
            {
                n1Des = n1;
                spac1Des = spac1;
            }
            else
            {
                n1Des = nA1;
                spac1Des = spacA1;
            }

            if (n2 < nA2)
            {
                n2Des = n2;
                spac2Des = spac2;
            }
            else
            {
                n2Des = nA2;
                spac2Des = spacA2;
            }

            List<UV> pnts = planeInfo.StandardRebarPointLists[0];
            List<UV> pntAs = planeInfoAfter.StandardRebarPointLists[0];
            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            double dia = designInfo.StandardDiameters[0];
            double diaAfter = designInfoAfter.StandardDiameters[0];
            double shortenLimit = lockheadParameter.ShortenLimit * ConstantValue.milimeter2Feet;

            XYZ vecExpBig = index == 0 ? -vecX : vecX;
            XYZ vecExpSmall = index == 0 ? vecX : -vecX;
            double delU = (index == 0) ? planeInfo.ShortenTypes[0].DeltaU1 : planeInfo.ShortenTypes[0].DeltaU2;
            double delV1 = planeInfo.ShortenTypes[0].DeltaV1;
            double delV2 = planeInfo.ShortenTypes[0].DeltaV2;
            double dimExpSmall = delU + (diaAfter - dia) / 2;

            switch (shorten)
            {
                case ShortenEnum.Big:
                    {
                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spac2 / 2,
                            Spacing = spac2,
                            Number = (n2 % 2 == 0) ? (n2 - 2) / 2 : (n2 - 2) / 2 + 1,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spac2,
                            Spacing = spac2,
                            Number = (n2 - 2) / 2,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index] + vecV * spacA2 / 2,
                            Spacing = spacA2,
                            Number = (nA2 % 2 == 0) ? (nA2 - 2) / 2 : (nA2 - 2) / 2 + 1,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L2,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index] + vecV * spacA2,
                            Spacing = spacA2,
                            Number = (nA2 - 2) / 2,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L1,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.Small:
                    {
                        int numBigV1 = 0, numSmallV1 = 0;
                        for (int i = 0; i < n2Des; i++)
                        {
                            double del = delV1 - spac2Des / 2 * (i + 1);
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigV1 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV1 = i + 1 - numBigV1;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        int numBigV2 = 0, numSmallV2 = 0;
                        for (int i = 0; i < n2Des; i++)
                        {
                            double del = delV2 - spac2Des / 2 * (i + 1);
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigV2 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV2 = i + 1 - numBigV2;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        IStandardPlaneSingleInfo stPlSinInfo = null;
                        if (numBigV1 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2,
                                Spacing = spac2Des,
                                Number = (numBigV1 % 2 == 0) ? numBigV1 / 2 : numBigV1 / 2 + 1,
                                Normal = vecY,
                                RebarLocation = RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigV1 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des,
                                Spacing = spac2Des,
                                Number = numBigV1 / 2,
                                Normal = vecY,
                                RebarLocation = RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        int num = numBigV1, num2 = numSmallV1;
                        for (int i = 0; i < numSmallV1; i++)
                        {
                            UV uvExpSmall2 = vecV * (delV1 + (diaAfter - dia) / 2 - spac2Des / 2 * (numBigV1 + i + 1)) + vecU * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1 + i),
                                Spacing = spac2Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n2 - numBigV1 - numBigV2 >= nA2)
                        {
                            num2 = n2Des - numSmallV1 - numSmallV2-2;
                        }
                        else
                        {
                            num2 = n2Des - numBigV1 - numSmallV1 - numBigV2 - numSmallV2-2;
                        }
                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1),
                            Spacing = spac2Des,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            CrackingDirection = vecExpSmall * dimExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 2),
                            Spacing = spac2Des,
                            Number = num2 / 2,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            CrackingDirection = vecExpSmall * dimExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallV2;
                        for (int i = 0; i < numSmallV2; i++)
                        {
                            UV uvExpSmall2 = -vecV * (delV2 + (diaAfter - dia) / 2 - spac2Des / 2 * (numBigV2 + (numSmallV2 - 1 - i) + 1)) + vecU * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1 + i),
                                Spacing = spac2Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigV2;
                        if (numBigV2 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1),
                                Spacing = spac2Des,
                                Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                Normal = vecY,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigV2 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 2),
                                Spacing = spac2Des,
                                Number = num2 / 2,
                                Normal = vecY,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n2 - numBigV1 - numBigV2 >= nA2)
                        {
                            num2 = n2 - nA2 - numBigV1 - numBigV2;
                            if (num2 >= 1)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.25,
                                    Spacing = spac2Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.75,
                                    Spacing = spac2Des,
                                    Number = num2 / 2,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                        else
                        {
                            num2 = nA2 - n2 + numBigV1 + numBigV2;
                            if (num2 >= 1)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.25 + vecU * (delU + (diaAfter - dia) / 2) * (index == 0 ? 1 : -1),
                                    Spacing = spac2Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.75 + vecU * (delU + (diaAfter - dia) / 2) * (index == 0 ? 1 : -1),
                                    Spacing = spac2Des,
                                    Number = num2 / 2,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                    }
                    break;
                case ShortenEnum.None:
                    {
                        int numBigV1 = 0, numSmallV1 = 0;
                        for (int i = 0; i < n2Des; i++)
                        {
                            double del = delV1 - spac2Des / 2 * (i + 1);
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigV1 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV1 = i + 1 - numBigV1;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        int numBigV2 = 0, numSmallV2 = 0;
                        for (int i = 0; i < n2Des; i++)
                        {
                            double del = delV2 - spac2Des / 2 * (i + 1);
                            if (GeomUtil.IsEqualOrBigger(del, shortenLimit))
                            {
                                numBigV2 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV2 = i + 1 - numBigV2;
                            }
                            else if (GeomUtil.IsEqualOrSmaller(del, 0))
                            {
                                break;
                            }
                        }

                        IStandardPlaneSingleInfo stPlSinInfo = null;
                        if (numBigV1 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2,
                                Spacing = spac2Des,
                                Number = (numBigV1 % 2 == 0) ? numBigV1 / 2 : numBigV1 / 2 + 1,
                                Normal = vecY,
                                RebarLocation = RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigV1 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des,
                                Spacing = spac2Des,
                                Number = numBigV1 / 2,
                                Normal = vecY,
                                RebarLocation = RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        int num = numBigV1, num2 = numSmallV1;
                        for (int i = 0; i < numSmallV1; i++)
                        {
                            UV uvExpSmall2 = vecV * (delV1 + (diaAfter - dia) / 2 - spac2Des / 2 * (numBigV1 + i + 1));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1 + i),
                                Spacing = spac2Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n2 - numBigV1 - numBigV2 >= nA2)
                        {
                            num2 = n2Des - numSmallV1 - numSmallV2-2;
                        }
                        else
                        {
                            num2 = n2Des - numBigV1 - numSmallV1 - numBigV2 - numSmallV2-2;
                        }
                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1),
                            Spacing = spac2Des,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 2),
                            Spacing = spac2Des,
                            Number = num2 / 2,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LocationIndex = 0
                        };
                        NormalStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallV2;
                        for (int i = 0; i < numSmallV2; i++)
                        {
                            UV uvExpSmall2 = -vecV * (delV2 + (diaAfter - dia) / 2 - spac2Des / 2 * (numBigV2 + (numSmallV2 - 1 - i) + 1));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1 + i),
                                Spacing = spac2Des,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigV2;
                        if (numBigV2 >= 1)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 1),
                                Spacing = spac2Des,
                                Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                Normal = vecY,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        if (numBigV2 >= 2)
                        {
                            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spac2Des / 2 * (num + 2),
                                Spacing = spac2Des,
                                Number = num2 / 2,
                                Normal = vecY,
                                RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                LockheadDirection = vecExpBig,
                                LocationIndex = 0
                            };
                            NormalStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2;
                        if (n2 - numBigV1 - numBigV2 >= nA2)
                        {
                            num2 = n2 - nA2 - numBigV1 - numBigV2;
                            if (num2 >= 1)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.25,
                                    Spacing = spac2Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.75,
                                    Spacing = spac2Des,
                                    Number = num2 / 2,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LockheadDirection = vecExpBig,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                        else
                        {
                            num2 = nA2 - n2 + numBigV1 + numBigV2;
                            if (num2 >= 1)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.25,
                                    Spacing = spac2Des,
                                    Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                            if (num2 >= 2)
                            {
                                stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                                {
                                    StartPoint = pnts[index] + vecV * spac2Des * 0.75,
                                    Spacing = spac2Des,
                                    Number = num2 / 2,
                                    Normal = vecY,
                                    RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                                    LocationIndex = 0
                                };
                                NormalStandardPlaneInfos.Add(stPlSinInfo);
                            }
                        }
                    }
                    break;
            }
        }
        private void GetLockheadStandardPlaneInfos()
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);

            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            List<UV> pnts = planeInfo.StandardRebarPointLists[0];
            List<UV> pntAs = planeInfoAfter.StandardRebarPointLists[0];
            int n1 = designInfo.StandardNumbers[0];
            int n2 = designInfo.StandardNumbers[1];
            int nA1 = designInfoAfter.StandardNumbers[0];
            int nA2 = designInfoAfter.StandardNumbers[1];
            double spac1 = designInfo.StandardSpacings[0];
            double spac2 = designInfo.StandardSpacings[1];
            double spacA1 = designInfoAfter.StandardSpacings[0];
            double spacA2 = designInfoAfter.StandardSpacings[1];

            XYZ vecExpBigU1 = -vecX, vecExpBigU2 = vecX;
            XYZ vecExpBigV1 = -vecY, vecExpBigV2 = vecY;

            LockheadStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();
            #region UDirection
            #region Lockhead
            IStandardPlaneSingleInfo ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0],
                Spacing = spac1,
                Number = (n1 % 2 == 0) ? n1 / 2 : n1 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[3],
                Spacing = spac1,
                Number = (n1 % 2 == 0) ? n1 / 2 : n1 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecU * spac1 / 2,
                Spacing = spac1,
                Number = n1 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[3] + vecU * spac1 / 2,
                Spacing = spac1,
                Number = n1 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion

            #region Implant
            if (ID < Singleton.Instance.GetElementCount() - 1)
            {
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntAs[0],
                    Spacing = spacA1,
                    Number = (nA1 % 2 == 0) ? nA1 / 2 : nA1 / 2 + 1,
                    Normal = vecX,
                    RebarLocation = RebarLocation.L1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntAs[3],
                    Spacing = spacA1,
                    Number = (nA1 % 2 == 0) ? nA1 / 2 : nA1 / 2 + 1,
                    Normal = vecX,
                    RebarLocation = RebarLocation.L1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntAs[0] + vecU * spac1 / 2,
                    Spacing = spacA1,
                    Number = nA1 / 2,
                    Normal = vecX,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntAs[3] + vecU * spac1 / 2,
                    Spacing = spacA1,
                    Number = nA1 / 2,
                    Normal = vecX,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }
            #endregion
            #endregion

            #region VDirection
            #region Lockhead
            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * spac2 / 2,
                Spacing = spac2,
                Number = (n2 % 2 == 0) ? (n2 - 2) / 2 : (n2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigU1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[1] + vecV * spac2 / 2,
                Spacing = spac2,
                Number = (n2 % 2 == 0) ? (n2 - 2) / 2 : (n2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigU2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * spac2,
                Spacing = spac2,
                Number = (n2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigU1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[1] + vecV * spac2,
                Spacing = spac2,
                Number = (n2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigU2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion

            #region Implant
            if (ID < Singleton.Instance.GetElementCount() - 1)
            {
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pnts[0] + vecV * spac2 / 2,
                    Spacing = spac2,
                    Number = (n2 % 2 == 0) ? (n2 - 2) / 2 : (n2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pnts[1] + vecV * spac2 / 2,
                    Spacing = spac2,
                    Number = (n2 % 2 == 0) ? (n2 - 2) / 2 : (n2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pnts[0] + vecV * spac2,
                    Spacing = spac2,
                    Number = (n2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pnts[1] + vecV * spac2,
                    Spacing = spac2,
                    Number = (n2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }
            #endregion
            #endregion
        }
        public void CreateRebar(int idTurn, int locIndex)
        {
            StandardTurn st = Singleton.Instance.GetStandardTurn(idTurn, locIndex);
            IVerticalInfo verticalInfo = Singleton.Instance.GetVerticalInfo(st.IDElement);
            switch (verticalInfo.StandardCreatingTypes[locIndex])
            {
                case StandardCreatingEnum.Normal:
                    NormalStandardPlaneInfos.ForEach(x => x.CreateRebar(idTurn, locIndex));
                    break;
                case StandardCreatingEnum.Lockhead:
                    LockheadStandardPlaneInfos.ForEach(x => x.CreateRebar(idTurn, locIndex));
                    break;
            }
            
        }
    }
    public class WallStandardPlaneInfo : IStandardPlaneInfo
    {
        public int ID { get; set; }
        public List<IStandardPlaneSingleInfo> NormalStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> ShortenStandardPlaneInfos { get; set; }
        public List<IStandardPlaneSingleInfo> LockheadStandardPlaneInfos { get; set; }
        public WallStandardPlaneInfo(int id)
        {
            ID = id;

            GetNormalStandardPlaneInfos();
            GetShortenStandardPlaneInfos();
            GetLockheadStandardPlaneInfos();
        }
        private void GetNormalStandardPlaneInfos()
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);

            int ne11 = designInfo.StandardNumbers[0];
            int ne12 = designInfo.StandardNumbers[1];
            int ce12 = designInfo.StandardNumbers[2];
            int ne2 = designInfo.StandardNumbers[3];
            bool isDoubleNE2 = designInfo.StandardNumbers[4] == 1 ? true : false;
            int nm = designInfo.StandardNumbers[5];

            double spacE11 = designInfo.StandardSpacings[0];
            double spacE12 = designInfo.StandardSpacings[1];
            double spacE2 = designInfo.StandardSpacings[2];
            double spacM = designInfo.StandardSpacings[3];
            List<UV> pntE1s = planeInfo.StandardRebarPointLists[0];
            List<UV> pntMs = planeInfo.StandardRebarPointLists[1];
            List<UV> pntE2s = planeInfo.StandardRebarPointLists[2];
            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;

            List<int> ie12 = new List<int>();
            double jumpe12 = (ne2 - 1) / (double)(ce12 + 1);
            for (int i = 0; i < ce12; i++)
            {
                ie12.Add((int)Math.Round(jumpe12 * (i + 1)));
            }

            NormalStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();

            #region EdgeLeft
            #region Edge11
            IStandardPlaneSingleInfo stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[3],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[3] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);
            #endregion

            #region Edge2
            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0] + vecV * spacE2 / 2,
                Spacing = spacE2,
                Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0] + vecV * spacE2,
                Spacing = spacE2,
                Number = (ne2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            if (isDoubleNE2)
            {
                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[1] + vecV * spacE2 / 2,
                    Spacing = spacE2,
                    Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);

                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[1] + vecV * spacE2,
                    Spacing = spacE2,
                    Number = (ne2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L1,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);
            }
            #endregion

            #region Edge12
            for (int i = 0; i < ie12.Count; i++)
            {
                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[0] + vecU * spacE11 / 2 * ie12[i] + vecV * spacE12 / 2,
                    Spacing = spacE12,
                    Number = ne12 % 2 == 0 ? (ne12 - 2) / 2 : (ne12 - 2) / 2 + 1,
                    Normal = vecX,
                    RebarLocation = ie12[i] % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);

                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[0] + vecU * spacE11 / 2 * ie12[i] + vecV * spacE12,
                    Spacing = spacE12,
                    Number = (ne12 - 2) / 2,
                    Normal = vecX,
                    RebarLocation = ie12[i] % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);
            }
            #endregion
            #endregion

            #region Middle
            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[0],
                Spacing = spacM,
                Number = nm % 2 == 0 ? nm / 2 : nm / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[0] + vecU * spacM / 2,
                Spacing = spacM,
                Number = nm / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[3],
                Spacing = spacM,
                Number = nm % 2 == 0 ? nm / 2 : nm / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[3] + vecU * spacM / 2,
                Spacing = spacM,
                Number = nm / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 1
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);
            #endregion

            #region EdgeRight
            #region Edge11
            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[0],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[3],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[0] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[3] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);
            #endregion

            #region Edge2
            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[1] + vecV * spacE2 / 2,
                Spacing = spacE2,
                Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[1] + vecV * spacE2,
                Spacing = spacE2,
                Number = (ne2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            NormalStandardPlaneInfos.Add(stPlSinInfo);

            if (isDoubleNE2)
            {
                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecV * spacE2 / 2,
                    Spacing = spacE2,
                    Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);

                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecV * spacE2,
                    Spacing = spacE2,
                    Number = (ne2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L1,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);
            }
            #endregion

            #region Edge12
            for (int i = 0; i < ie12.Count; i++)
            {
                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecU * spacE11 / 2 * ie12[i] + vecV * spacE12 / 2,
                    Spacing = spacE12,
                    Number = ne12 % 2 == 0 ? (ne12 - 2) / 2 : (ne12 - 2) / 2 + 1,
                    Normal = vecX,
                    RebarLocation = ie12[i] % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);

                stPlSinInfo = new StraightStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecU * spacE11 / 2 * ie12[i] + vecV * spacE12,
                    Spacing = spacE12,
                    Number = (ne12 - 2) / 2,
                    Normal = vecX,
                    RebarLocation = ie12[i] % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                    LocationIndex = 0
                };
                NormalStandardPlaneInfos.Add(stPlSinInfo);
            }
            #endregion
            #endregion
        }
        private void GetShortenStandardPlaneInfos()
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);

            int ne11 = designInfo.StandardNumbers[0];
            int ne12 = designInfo.StandardNumbers[1];
            int ce12 = designInfo.StandardNumbers[2];
            int ne2 = designInfo.StandardNumbers[3];
            bool isDoubleNE2 = designInfo.StandardNumbers[4] == 1 ? true : false;
            int nm = designInfo.StandardNumbers[5];

            List<int> ie12 = new List<int>();
            double jumpe12 = (ne2 - 1) / (double)(ce12 + 1);
            for (int i = 0; i < ce12; i++)
            {
                ie12.Add((int)Math.Round(jumpe12 * (i + 1)));
            }

            Shorten.ShortenType stE1 = planeInfo.ShortenTypes[0];
            Shorten.ShortenType stM = planeInfo.ShortenTypes[1];
            Shorten.ShortenType stE2 = planeInfo.ShortenTypes[2];

            ShortenStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();

            GetShortenVEdge(0, 0, stE1.ShortenV1);
            GetShortenVEdge(0, 3, stE1.ShortenV2);
            GetShortenUEdge(0, 0, stE1.ShortenU1);
            if (isDoubleNE2) GetShortenUEdge(0, 1, stE1.ShortenU2);
            for (int i = 0; i < ie12.Count; i++)
            {
                GetShortenVInside(0, ie12[0]);
            }

            GetShortenVMiddle(0, stM.ShortenV1);
            GetShortenVMiddle(3, stM.ShortenV2);

            GetShortenVEdge(1, 0, stE1.ShortenV1);
            GetShortenVEdge(1, 3, stE1.ShortenV2);
            if (isDoubleNE2) GetShortenUEdge(1, 0, stE1.ShortenU1);
            GetShortenUEdge(1, 1, stE1.ShortenU2);
            for (int i = 0; i < ie12.Count; i++)
            {
                GetShortenVInside(1, ie12[0]);
            }
        }
        private void GetShortenVEdge(int locIndex, int index, ShortenEnum shorten)
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);
            ARLockheadParameter lockheadParameter = Singleton.Instance.LockheadParameter;

            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            int ne11 = designInfo.StandardNumbers[0];
            int ne12 = designInfo.StandardNumbers[1];
            int ce12 = designInfo.StandardNumbers[2];
            int ne2 = designInfo.StandardNumbers[3];
            bool isDoubleNE2 = designInfo.StandardNumbers[4] == 1 ? true : false;
            int nm = designInfo.StandardNumbers[5];
            int neA11 = designInfoAfter.StandardNumbers[0];
            int neA12 = designInfoAfter.StandardNumbers[1];
            int ceA12 = designInfoAfter.StandardNumbers[2];
            int neA2 = designInfoAfter.StandardNumbers[3];
            bool isDoubleNEA2 = designInfoAfter.StandardNumbers[4] == 1 ? true : false;
            int nmA = designInfoAfter.StandardNumbers[5];

            double spacE11 = designInfo.StandardSpacings[0];
            double spacE12 = designInfo.StandardSpacings[1];
            double spacE2 = designInfo.StandardSpacings[2];
            double spacM = designInfo.StandardSpacings[3];
            double spacEA11 = designInfoAfter.StandardSpacings[0];
            double spacEA12 = designInfoAfter.StandardSpacings[1];
            double spacEA2 = designInfoAfter.StandardSpacings[2];
            double spacMA = designInfoAfter.StandardSpacings[3];
            List<UV> pnts = planeInfo.BoundaryPointLists[locIndex];
            List<UV> pntAs = planeInfoAfter.BoundaryPointLists[locIndex];
            double dia = designInfo.StandardDiameters[0];
            double diaAfter = designInfoAfter.StandardDiameters[0];
            double shortenLimit = lockheadParameter.ShortenLimit * ConstantValue.milimeter2Feet;

            XYZ vecExpBig = index == 0 ? -vecY : vecY;
            XYZ vecExpSmall = index == 0 ? vecY : -vecY;
            Shorten.ShortenType shortenType = planeInfo.ShortenTypes[locIndex];
            double delV = (index == 0) ? shortenType.DeltaV1 : shortenType.DeltaV2;
            double delU1 = shortenType.DeltaU1;
            double delU2 = shortenType.DeltaU2;
            double dimExpSmall = delV + (diaAfter - dia) / 2;

            switch (shorten)
            {
                case ShortenEnum.Big:
                    {
                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index],
                            Spacing = spacE11,
                            Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index],
                            Spacing = spacE11,
                            Number = ne11 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacEA11,
                            Number = (neA11 % 2 == 0) ? neA11 / 2 : neA11 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacEA11,
                            Number = neA11 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.Small:
                    {
                        int numBigU1 = 0, numSmallU1 = 0;
                        for (int i = 0; i < neA11; i++)
                        {
                            double del = delU1 - spacE11 / 2 * i;
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
                        for (int i = 0; i < neA11; i++)
                        {
                            double del = delU2 - spacE11 / 2 * i;
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
                            Spacing = spacE11,
                            Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2,
                            Spacing = spacE11,
                            Number = numBigU1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        int num = numBigU1, num2 = numSmallU1;
                        for (int i = 0; i < numSmallU1; i++)
                        {
                            UV uvExpSmall2 = vecU * (delU1 + (diaAfter - dia) / 2 - spacE11 / 2 * (numBigU1 + i)) + vecV * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + i),
                                Spacing = spacE11,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = ne11 - numBigU1 - numSmallU1 - numBigU2 - numSmallU2;
                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * num,
                            Spacing = spacE11,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + 1),
                            Spacing = spacE11,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallU2;
                        for (int i = 0; i < numSmallU2; i++)
                        {
                            UV uvExpSmall2 = -vecU * (delU2 + (diaAfter - dia) / 2 - spacE11 / 2 * (numBigU2 + (numSmallU2 - 1 - i))) + vecV * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + i),
                                Spacing = spacE11,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigU2;
                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * num,
                            Spacing = spacE11,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + 1),
                            Spacing = spacE11,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.None:
                    {
                        int numBigU1 = 0, numSmallU1 = 0;
                        for (int i = 0; i < neA11; i++)
                        {
                            double del = delU1 - spacE11 / 2 * i;
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
                        for (int i = 0; i < neA11; i++)
                        {
                            double del = delU2 - spacE11 / 2 * i;
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
                            Spacing = spacE11,
                            Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2,
                            Spacing = spacE11,
                            Number = numBigU1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        int num = numBigU1, num2 = numSmallU1;
                        for (int i = 0; i < numSmallU1; i++)
                        {
                            UV uvExpSmall2 = vecU * (delU1 + (diaAfter - dia) / 2 - spacE11 / 2 * (numBigU1 + i));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + i),
                                Spacing = spacE11,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = ne11 - numBigU1 - numSmallU1 - numBigU2 - numSmallU2;
                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * num,
                            Spacing = spacE11,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + 1),
                            Spacing = spacE11,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallU2;
                        for (int i = 0; i < numSmallU2; i++)
                        {
                            UV uvExpSmall2 = -vecU * (delU2 + (diaAfter - dia) / 2 - spacE11 / 2 * (numBigU2 + (numSmallU2 - 1 - i)));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + i),
                                Spacing = spacE11,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigU2;
                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * num,
                            Spacing = spacE11,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2 * (num + 1),
                            Spacing = spacE11,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
            }
        }
        private void GetShortenUEdge(int locIndex, int index, ShortenEnum shorten)
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);
            ARLockheadParameter lockheadParameter = Singleton.Instance.LockheadParameter;

            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            int ne11 = designInfo.StandardNumbers[0];
            int ne12 = designInfo.StandardNumbers[1];
            int ce12 = designInfo.StandardNumbers[2];
            int ne2 = designInfo.StandardNumbers[3];
            bool isDoubleNE2 = designInfo.StandardNumbers[4] == 1 ? true : false;
            int nm = designInfo.StandardNumbers[5];
            int neA11 = designInfoAfter.StandardNumbers[0];
            int neA12 = designInfoAfter.StandardNumbers[1];
            int ceA12 = designInfoAfter.StandardNumbers[2];
            int neA2 = designInfoAfter.StandardNumbers[3];
            bool isDoubleNEA2 = designInfoAfter.StandardNumbers[4] == 1 ? true : false;
            int nmA = designInfoAfter.StandardNumbers[5];

            double spacE11 = designInfo.StandardSpacings[0];
            double spacE12 = designInfo.StandardSpacings[1];
            double spacE2 = designInfo.StandardSpacings[2];
            double spacM = designInfo.StandardSpacings[3];
            double spacEA11 = designInfoAfter.StandardSpacings[0];
            double spacEA12 = designInfoAfter.StandardSpacings[1];
            double spacEA2 = designInfoAfter.StandardSpacings[2];
            double spacMA = designInfoAfter.StandardSpacings[3];
            List<UV> pnts = planeInfo.BoundaryPointLists[locIndex];
            List<UV> pntAs = planeInfoAfter.BoundaryPointLists[locIndex];
            double dia = designInfo.StandardDiameters[0];
            double diaAfter = designInfoAfter.StandardDiameters[0];
            double shortenLimit = lockheadParameter.ShortenLimit * ConstantValue.milimeter2Feet;

            XYZ vecExpBig = index == 0 ? -vecX : vecX;
            XYZ vecExpSmall = index == 0 ? vecX : -vecX;
            Shorten.ShortenType shortenType = planeInfo.ShortenTypes[locIndex];
            double delU = (index == 0) ? shortenType.DeltaU1 : shortenType.DeltaU2;
            double delV1 = shortenType.DeltaV1;
            double delV2 = shortenType.DeltaV2;
            double dimExpSmall = delU + (diaAfter - dia) / 2;

            switch (shorten)
            {
                case ShortenEnum.Big:
                    {
                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2,
                            Spacing = spacE2,
                            Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2,
                            Spacing = spacE2,
                            Number = (ne2 - 2) / 2,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index] + vecV * spacEA2 / 2,
                            Spacing = spacEA2,
                            Number = (neA2 % 2 == 0) ? (neA2 - 2) / 2 : (neA2 - 2) / 2 + 1,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L2,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index] + vecV * spacEA2,
                            Spacing = spacEA2,
                            Number = (neA2 - 2) / 2,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L1,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.Small:
                    {
                        int numBigV1 = 0, numSmallV1 = 0;
                        for (int i = 0; i < ne2; i++)
                        {
                            double del = delV1 - spacE2 / 2 * (i + 1);
                            if (GeomUtil.IsEqual(del, shortenLimit) || del > shortenLimit)
                            {
                                numBigV1 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV1 = i + 1 - numBigV1;
                            }
                            else if (GeomUtil.IsEqual(del, 0) || del < 0)
                            {
                                break;
                            }
                        }

                        int numBigV2 = 0, numSmallV2 = 0;
                        for (int i = 0; i < ne2; i++)
                        {
                            double del = delV2 - spacE2 / 2 * (i + 1);
                            if (GeomUtil.IsEqual(del, shortenLimit) || del > shortenLimit)
                            {
                                numBigV2 = i + 1;
                            }
                            else if (GeomUtil.IsEqual(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV2 = i + 1 - numBigV2;
                            }
                            else if (GeomUtil.IsEqual(del, 0) || del < 0)
                            {
                                break;
                            }
                        }

                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2,
                            Spacing = spacE2,
                            Number = (numBigV1 % 2 == 0) ? numBigV1 / 2 : numBigV1 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2,
                            Spacing = spacE2,
                            Number = numBigV1 / 2,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        int num = numBigV1, num2 = numSmallV1;
                        for (int i = 0; i < numSmallV1; i++)
                        {
                            UV uvExpSmall2 = vecV * (delV1 + (diaAfter - dia) / 2 - spacE2 / 2 * (numBigV1 + i + 1)) + vecU * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1 + i),
                                Spacing = spacE2,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = ne2 - numBigV1 - numSmallV1 - numBigV2 - numSmallV2;
                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1),
                            Spacing = spacE2,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 2),
                            Spacing = spacE2,
                            Number = num2 / 2,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallV2;
                        for (int i = 0; i < numSmallV2; i++)
                        {
                            UV uvExpSmall2 = -vecV * (delV2 + (diaAfter - dia) / 2 - spacE2 / 2 * (numBigV2 + (numSmallV2 - 1 - i) + 1)) + vecU * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1 + i),
                                Spacing = spacE2,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigV2;
                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1),
                            Spacing = spacE2,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 2),
                            Spacing = spacE2,
                            Number = num2 / 2,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.None:
                    {
                        int numBigV1 = 0, numSmallV1 = 0;
                        for (int i = 0; i < ne2; i++)
                        {
                            double del = delV1 - spacE2 / 2 * (i + 1);
                            if (GeomUtil.IsEqual(del, shortenLimit) || del > shortenLimit)
                            {
                                numBigV1 = i + 1;
                            }
                            else if (GeomUtil.IsBigger(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV1 = i + 1 - numBigV1;
                            }
                            else if (GeomUtil.IsEqual(del, 0) || del < 0)
                            {
                                break;
                            }
                        }

                        int numBigV2 = 0, numSmallV2 = 0;
                        for (int i = 0; i < ne2; i++)
                        {
                            double del = delV2 - spacE2 / 2 * (i + 1);
                            if (GeomUtil.IsEqual(del, shortenLimit) || del > shortenLimit)
                            {
                                numBigV2 = i + 1;
                            }
                            else if (GeomUtil.IsEqual(del, 0) && GeomUtil.IsSmaller(del, shortenLimit))
                            {
                                numSmallV2 = i + 1 - numBigV2;
                            }
                            else if (GeomUtil.IsEqual(del, 0) || del < 0)
                            {
                                break;
                            }
                        }

                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2,
                            Spacing = spacE2,
                            Number = (numBigV1 % 2 == 0) ? numBigV1 / 2 : numBigV1 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2,
                            Spacing = spacE2,
                            Number = numBigV1 / 2,
                            Normal = vecY,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        int num = numBigV1, num2 = numSmallV1;
                        for (int i = 0; i < numSmallV1; i++)
                        {
                            UV uvExpSmall2 = vecV * (delV1 + (diaAfter - dia) / 2 - spacE2 / 2 * (numBigV1 + i + 1));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1 + i),
                                Spacing = spacE2,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = ne2 - numBigV1 - numSmallV1 - numBigV2 - numSmallV2;
                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1),
                            Spacing = spacE2,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 2),
                            Spacing = spacE2,
                            Number = num2 / 2,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallV2;
                        for (int i = 0; i < numSmallV2; i++)
                        {
                            UV uvExpSmall2 = -vecV * (delV2 + (diaAfter - dia) / 2 - spacE2 / 2 * (numBigV2 + (numSmallV2 - 1 - i) + 1));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL2 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1 + i),
                                Spacing = spacE2,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL2 ? RebarLocation.L2 : RebarLocation.L1,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 0
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigV2;
                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 1),
                            Spacing = spacE2,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecV * spacE2 / 2 * (num + 2),
                            Spacing = spacE2,
                            Number = num2 / 2,
                            Normal = vecY,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 0
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
            }
        }
        private void GetShortenVInside(int locIndex, int index12)
        {
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            ARLockheadParameter lockheadParameter = Singleton.Instance.LockheadParameter;

            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            int ne11 = designInfo.StandardNumbers[0];
            int ne12 = designInfo.StandardNumbers[1];
            int ce12 = designInfo.StandardNumbers[2];
            int ne2 = designInfo.StandardNumbers[3];
            bool isDoubleNE2 = designInfo.StandardNumbers[4] == 1 ? true : false;
            int nm = designInfo.StandardNumbers[5];
            int neA11 = designInfoAfter.StandardNumbers[0];
            int neA12 = designInfoAfter.StandardNumbers[1];
            int ceA12 = designInfoAfter.StandardNumbers[2];
            int neA2 = designInfoAfter.StandardNumbers[3];
            bool isDoubleNEA2 = designInfoAfter.StandardNumbers[4] == 1 ? true : false;
            int nmA = designInfoAfter.StandardNumbers[5];

            double spacE11 = designInfo.StandardSpacings[0];
            double spacE12 = designInfo.StandardSpacings[1];
            double spacE2 = designInfo.StandardSpacings[2];
            double spacM = designInfo.StandardSpacings[3];
            double spacEA11 = designInfoAfter.StandardSpacings[0];
            double spacEA12 = designInfoAfter.StandardSpacings[1];
            double spacEA2 = designInfoAfter.StandardSpacings[2];
            double spacMA = designInfoAfter.StandardSpacings[3];
            List<UV> pnts = planeInfo.BoundaryPointLists[locIndex];
            List<UV> pntAs = planeInfoAfter.BoundaryPointLists[locIndex];
            double dia = designInfo.StandardDiameters[0];
            double diaAfter = designInfoAfter.StandardDiameters[0];
            double shortenLimit = lockheadParameter.ShortenLimit * ConstantValue.milimeter2Feet;

            XYZ vecExpBig = vecY;
            Shorten.ShortenType shortenType = planeInfo.ShortenTypes[locIndex];
            double delU1 = shortenType.DeltaU1;
            double delU2 = shortenType.DeltaU2;

            #region ShortenV=None
            int numBigU1 = 0, numSmallU1 = 0;
            for (int i = 0; i < neA12; i++)
            {
                double del = delU1 - spacE12 / 2 * (i + 1);
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
            for (int i = 0; i < neA12; i++)
            {
                double del = delU2 - spacE12 / 2 * (i + 1);
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
                StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12 / 2,
                Spacing = spacE12,
                Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                Normal = vecX,
                RebarLocation = index12 % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                LockheadDirection = vecExpBig,
                LocationIndex = 0
            };
            ShortenStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12,
                Spacing = spacE12,
                Number = numBigU1 / 2,
                Normal = vecX,
                RebarLocation = index12 % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                LockheadDirection = vecExpBig,
                LocationIndex = 0
            };
            ShortenStandardPlaneInfos.Add(stPlSinInfo);

            int num = numBigU1, num2 = numSmallU1;
            for (int i = 0; i < numSmallU1; i++)
            {
                UV uvExpSmall2 = vecU * (delU1 + (diaAfter - dia) / 2 - spacE2 / 2 * (numBigU1 + i + 1));
                XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                {
                    StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12 / 2 * (num + i + 1),
                    Spacing = spacE12,
                    Number = 1,
                    Normal = normExpSmall2,
                    RebarLocation = (index12 + num + i) % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                    CrackingDirection = vecExpSmall2,
                    CrackingLength = vecExpSmall2.GetLength(),
                    LocationIndex = 0
                };
                ShortenStandardPlaneInfos.Add(stPlSinInfo);
            }

            num = num + num2; num2 = ne11 - numBigU1 - numSmallU1 - numBigU2 - numSmallU2;
            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12 / 2 * (num + 1),
                Spacing = spacE12,
                Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                Normal = vecX,
                RebarLocation = (index12 + num) % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                LocationIndex = 0
            };
            ShortenStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StraightStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12 / 2 * (num + 2),
                Spacing = spacE12,
                Number = num2 / 2,
                Normal = vecX,
                RebarLocation = (index12 + num) % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                LocationIndex = 0
            };
            ShortenStandardPlaneInfos.Add(stPlSinInfo);

            num = num + num2; num2 = numSmallU2;
            for (int i = 0; i < numSmallU2; i++)
            {
                UV uvExpSmall2 = -vecU * (delU2 + (diaAfter - dia) / 2 - spacE12 / 2 * (numBigU2 + (numSmallU2 - 1 - i) + 1));
                XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                {
                    StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12 / 2 * (num + i + 1),
                    Spacing = spacE12,
                    Number = 1,
                    Normal = normExpSmall2,
                    RebarLocation = (index12 + num + i) % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                    CrackingDirection = vecExpSmall2,
                    CrackingLength = vecExpSmall2.GetLength(),
                    LocationIndex = 0
                };
                ShortenStandardPlaneInfos.Add(stPlSinInfo);
            }

            num = num + num2; num2 = numBigU2;
            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12 / 2 * (num + 1),
                Spacing = spacE12,
                Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                Normal = vecX,
                RebarLocation = (index12 + num) % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                LockheadDirection = vecExpBig,
                LocationIndex = 0
            };
            ShortenStandardPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pnts[0] + vecV * index12 * spacE2 / 2 + vecU * spacE12 / 2 * (num + 2),
                Spacing = spacE12,
                Number = num2 / 2,
                Normal = vecX,
                RebarLocation = (index12 + num) % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                LockheadDirection = vecExpBig,
                LocationIndex = 0
            };
            ShortenStandardPlaneInfos.Add(stPlSinInfo);
            #endregion
        }
        private void GetShortenVMiddle(int index, ShortenEnum shorten)
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);
            ARLockheadParameter lockheadParameter = Singleton.Instance.LockheadParameter;

            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            int ne11 = designInfo.StandardNumbers[0];
            int ne12 = designInfo.StandardNumbers[1];
            int ce12 = designInfo.StandardNumbers[2];
            int ne2 = designInfo.StandardNumbers[3];
            bool isDoubleNE2 = designInfo.StandardNumbers[4] == 1 ? true : false;
            int nm = designInfo.StandardNumbers[5];
            int neA11 = designInfoAfter.StandardNumbers[0];
            int neA12 = designInfoAfter.StandardNumbers[1];
            int ceA12 = designInfoAfter.StandardNumbers[2];
            int neA2 = designInfoAfter.StandardNumbers[3];
            bool isDoubleNEA2 = designInfoAfter.StandardNumbers[4] == 1 ? true : false;
            int nmA = designInfoAfter.StandardNumbers[5];

            double spacE11 = designInfo.StandardSpacings[0];
            double spacE12 = designInfo.StandardSpacings[1];
            double spacE2 = designInfo.StandardSpacings[2];
            double spacM = designInfo.StandardSpacings[3];
            double spacEA11 = designInfoAfter.StandardSpacings[0];
            double spacEA12 = designInfoAfter.StandardSpacings[1];
            double spacEA2 = designInfoAfter.StandardSpacings[2];
            double spacMA = designInfoAfter.StandardSpacings[3];
            List<UV> pnts = planeInfo.BoundaryPointLists[1];
            List<UV> pntAs = planeInfoAfter.BoundaryPointLists[1];
            double dia = designInfo.StandardDiameters[1];
            double diaAfter = designInfoAfter.StandardDiameters[1];
            double shortenLimit = lockheadParameter.ShortenLimit * ConstantValue.milimeter2Feet;

            XYZ vecExpBig = index == 0 ? -vecY : vecY;
            XYZ vecExpSmall = index == 0 ? vecY : -vecY;
            Shorten.ShortenType shortenType = planeInfo.ShortenTypes[1];
            double delV = (index == 0) ? shortenType.DeltaV1 : shortenType.DeltaV2;
            double delU1 = shortenType.DeltaU1;
            double delU2 = shortenType.DeltaU2;
            double dimExpSmall = delV + (diaAfter - dia) / 2;

            switch (shorten)
            {
                case ShortenEnum.Big:
                    {
                        IStandardPlaneSingleInfo stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index],
                            Spacing = spacM,
                            Number = (nm % 2 == 0) ? nm / 2 : ne11 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index],
                            Spacing = spacM,
                            Number = nm / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacMA,
                            Number = (nmA % 2 == 0) ? nmA / 2 : nmA / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new ImplantStandardPlaneSingleInfo()
                        {
                            StartPoint = pntAs[index],
                            Spacing = spacMA,
                            Number = nmA / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.Small:
                    {
                        int numBigU1 = 0, numSmallU1 = 0;
                        for (int i = 0; i < nm; i++)
                        {
                            double del = delU1 - spacM / 2 * i;
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
                        for (int i = 0; i < nm; i++)
                        {
                            double del = delU2 - spacM / 2 * i;
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
                            Spacing = spacM,
                            Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacE11 / 2,
                            Spacing = spacM,
                            Number = numBigU1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        int num = numBigU1, num2 = numSmallU1;
                        for (int i = 0; i < numSmallU1; i++)
                        {
                            UV uvExpSmall2 = vecU * (delU1 + (diaAfter - dia) / 2 - spacM / 2 * (numBigU1 + i)) + vecV * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacM / 2 * (num + i),
                                Spacing = spacM,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 1
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = ne11 - numBigU1 - numSmallU1 - numBigU2 - numSmallU2;
                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * num,
                            Spacing = spacM,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * (num + 1),
                            Spacing = spacM,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            CrackingDirection = vecExpSmall,
                            CrackingLength = dimExpSmall,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallU2;
                        for (int i = 0; i < numSmallU2; i++)
                        {
                            UV uvExpSmall2 = -vecU * (delU2 + (diaAfter - dia) / 2 - spacM / 2 * (numBigU2 + (numSmallU2 - 1 - i))) + vecV * dimExpSmall * (index == 0 ? 1 : -1);
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacM / 2 * (num + i),
                                Spacing = spacM,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 1
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigU2;
                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * num,
                            Spacing = spacM,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * (num + 1),
                            Spacing = spacM,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
                case ShortenEnum.None:
                    {
                        int numBigU1 = 0, numSmallU1 = 0;
                        for (int i = 0; i < nm; i++)
                        {
                            double del = delU1 - spacM / 2 * i;
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
                        for (int i = 0; i < neA11; i++)
                        {
                            double del = delU2 - spacM / 2 * i;
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
                            Spacing = spacM,
                            Number = (numBigU1 % 2 == 0) ? numBigU1 / 2 : numBigU1 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2,
                            Spacing = spacM,
                            Number = numBigU1 / 2,
                            Normal = vecX,
                            RebarLocation = RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        int num = numBigU1, num2 = numSmallU1;
                        for (int i = 0; i < numSmallU1; i++)
                        {
                            UV uvExpSmall2 = vecU * (delU1 + (diaAfter - dia) / 2 - spacM / 2 * (numBigU1 + i));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacM / 2 * (num + i),
                                Spacing = spacM,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 1
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = ne11 - numBigU1 - numSmallU1 - numBigU2 - numSmallU2;
                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * num,
                            Spacing = spacM,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new StraightStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * (num + 1),
                            Spacing = spacM,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        num = num + num2; num2 = numSmallU2;
                        for (int i = 0; i < numSmallU2; i++)
                        {
                            UV uvExpSmall2 = -vecU * (delU2 + (diaAfter - dia) / 2 - spacM / 2 * (numBigU2 + (numSmallU2 - 1 - i)));
                            XYZ vecExpSmall2 = new XYZ(uvExpSmall2.U, uvExpSmall2.V, 0);
                            XYZ normExpSmall2 = vecExpSmall2.Normalize().CrossProduct(XYZ.BasisZ);
                            bool isL1 = ((num % 2 == 0) == (i % 2 == 0)) ? true : false;
                            stPlSinInfo = new CrackingStandardPlaneSingleInfo()
                            {
                                StartPoint = pnts[index] + vecU * spacM / 2 * (num + i),
                                Spacing = spacM,
                                Number = 1,
                                Normal = normExpSmall2,
                                RebarLocation = isL1 ? RebarLocation.L1 : RebarLocation.L2,
                                CrackingDirection = vecExpSmall2,
                                CrackingLength = vecExpSmall2.GetLength(),
                                LocationIndex = 1
                            };
                            ShortenStandardPlaneInfos.Add(stPlSinInfo);
                        }

                        num = num + num2; num2 = numBigU2;
                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * num,
                            Spacing = spacM,
                            Number = num2 % 2 == 0 ? num2 / 2 : num2 / 2 + 1,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L1 : RebarLocation.L2,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);

                        stPlSinInfo = new LockheadStandardPlaneSingleInfo()
                        {
                            StartPoint = pnts[index] + vecU * spacM / 2 * (num + 1),
                            Spacing = spacM,
                            Number = num2 / 2,
                            Normal = vecX,
                            RebarLocation = num % 2 == 0 ? RebarLocation.L2 : RebarLocation.L1,
                            LockheadDirection = vecExpBig,
                            LocationIndex = 1
                        };
                        ShortenStandardPlaneInfos.Add(stPlSinInfo);
                    }
                    break;
            }
        }
        private void GetLockheadStandardPlaneInfos()
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            IDesignInfo designInfoAfter = Singleton.Instance.GetDesignInfoAfter(ID);
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IPlaneInfo planeInfoAfter = Singleton.Instance.GetPlaneInfoAfter(ID);

            int ne11 = designInfo.StandardNumbers[0];
            int ne12 = designInfo.StandardNumbers[1];
            int ce12 = designInfo.StandardNumbers[2];
            int ne2 = designInfo.StandardNumbers[3];
            bool isDoubleNE2 = designInfo.StandardNumbers[4] == 1 ? true : false;
            int nm = designInfo.StandardNumbers[5];
            int neA11 = designInfoAfter.StandardNumbers[0];
            int neA12 = designInfoAfter.StandardNumbers[1];
            int ceA12 = designInfoAfter.StandardNumbers[2];
            int neA2 = designInfoAfter.StandardNumbers[3];
            bool isDoubleNEA2 = designInfoAfter.StandardNumbers[4] == 1 ? true : false;
            int nmA = designInfoAfter.StandardNumbers[5];

            double spacE11 = designInfo.StandardSpacings[0];
            double spacE12 = designInfo.StandardSpacings[1];
            double spacE2 = designInfo.StandardSpacings[2];
            double spacM = designInfo.StandardSpacings[3];
            double spacEA11 = designInfoAfter.StandardSpacings[0];
            double spacEA12 = designInfoAfter.StandardSpacings[1];
            double spacEA2 = designInfoAfter.StandardSpacings[2];
            double spacMA = designInfoAfter.StandardSpacings[3];
            List<UV> pntE1s = planeInfo.StandardRebarPointLists[0];
            List<UV> pntMs = planeInfo.StandardRebarPointLists[1];
            List<UV> pntE2s = planeInfo.StandardRebarPointLists[2];
            List<UV> pntE1As = planeInfoAfter.StandardRebarPointLists[0];
            List<UV> pntMAs = planeInfoAfter.StandardRebarPointLists[1];
            List<UV> pntE2As = planeInfoAfter.StandardRebarPointLists[2];
            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;

            XYZ vecExpBigU1 = -vecY, vecExpBigU2 = vecY;
            XYZ vecExpBigV1 = -vecX, vecExpBigV2 = vecX;

            List<int> ie12 = new List<int>();
            double jumpe12 = (ne2 - 1) / (double)(ce12 + 1);
            for (int i = 0; i < ce12; i++)
            {
                ie12.Add((int)Math.Round(jumpe12 * (i + 1)));
            }

            List<int> ieA12 = new List<int>();
            double jumpeA12 = (neA2 - 1) / (double)(ceA12 + 1);
            for (int i = 0; i < ceA12; i++)
            {
                ieA12.Add((int)Math.Round(jumpeA12 * (i + 1)));
            }

            LockheadStandardPlaneInfos = new List<IStandardPlaneSingleInfo>();

            #region EdgeLeft
            #region Edge11
            #region Lockhead
            IStandardPlaneSingleInfo ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[3],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[3] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion

            #region Implant
            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE1As[0],
                Spacing = spacEA11,
                Number = (neA11 % 2 == 0) ? neA11 / 2 : neA11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE1As[3],
                Spacing = spacEA11,
                Number = (neA11 % 2 == 0) ? neA11 / 2 : neA11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE1As[0] + vecU * spacEA11 / 2,
                Spacing = spacEA11,
                Number = neA11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE1As[3] + vecU * spacEA11 / 2,
                Spacing = spacEA11,
                Number = neA11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion
            #endregion

            #region Edge2
            #region Lockhead
            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0] + vecV * spacE2 / 2,
                Spacing = spacE2,
                Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigU1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            if (isDoubleNE2)
            {
                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[1] + vecV * spacE2 / 2,
                    Spacing = spacE2,
                    Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LockheadDirection = vecExpBigU2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE1s[0] + vecV * spacE2,
                Spacing = spacE2,
                Number = (ne2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigU1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            if (isDoubleNE2)
            {
                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[1] + vecV * spacE2 / 2,
                    Spacing = spacE2,
                    Number = (ne2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LockheadDirection = vecExpBigU2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }
            #endregion

            #region Implant
            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE1As[0] + vecV * spacEA2 / 2,
                Spacing = spacEA2,
                Number = (neA2 % 2 == 0) ? (neA2 - 2) / 2 : (neA2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            if (isDoubleNEA2)
            {
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1As[1] + vecV * spacEA2 / 2,
                    Spacing = spacEA2,
                    Number = (neA2 % 2 == 0) ? (neA2 - 2) / 2 : (neA2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE1As[0] + vecV * spacEA2,
                Spacing = spacEA2,
                Number = (neA2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            if (isDoubleNEA2)
            {
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1As[1] + vecV * spacEA2 / 2,
                    Spacing = spacEA2,
                    Number = (neA2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }
            #endregion
            #endregion

            #region Edge12
            for (int i = 0; i < ie12.Count; i++)
            {
                #region Lockhead
                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[0] + vecV * spacE2 / 2 * ie12[i] + vecU * spacE12 / 2,
                    Spacing = spacE12,
                    Number = (ne12 % 2 == 0) ? (ne12 - 2) / 2 : (ne12 - 2) / 2 + 1,
                    Normal = vecX,
                    RebarLocation = (ie12[i] % 2 == 0) ? RebarLocation.L2 : RebarLocation.L1,
                    LockheadDirection = vecExpBigU1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1s[0] + vecV * spacE2 / 2 * ie12[i] + vecU * spacE12,
                    Spacing = spacE12,
                    Number = (ne12 - 2) / 2,
                    Normal = vecX,
                    RebarLocation = (ie12[i] % 2 == 0) ? RebarLocation.L1 : RebarLocation.L2,
                    LockheadDirection = vecExpBigU1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
                #endregion
            }

            for (int i = 0; i < ieA12.Count; i++)
            {
                #region Implant
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1As[0] + vecV * spacEA2 / 2 * ieA12[i] + vecU * spacEA12 / 2,
                    Spacing = spacEA12,
                    Number = (neA12 % 2 == 0) ? (neA12 - 2) / 2 : (neA12 - 2) / 2 + 1,
                    Normal = vecX,
                    RebarLocation = (ieA12[i] % 2 == 0) ? RebarLocation.L2 : RebarLocation.L1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE1As[0] + vecV * spacEA2 / 2 * ieA12[i] + vecU * spacEA12,
                    Spacing = spacEA12,
                    Number = (neA12 - 2) / 2,
                    Normal = vecX,
                    RebarLocation = (ieA12[i] % 2 == 0) ? RebarLocation.L1 : RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
                #endregion
            }
            #endregion
            #endregion

            #region Middle
            #region Lockhead
            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[0],
                Spacing = spacM,
                Number = (nm % 2 == 0) ? nm / 2 : nm / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[3],
                Spacing = spacM,
                Number = (nm % 2 == 0) ? nm / 2 : nm / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[0] + vecU * spacM / 2,
                Spacing = spacM,
                Number = nm / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntMs[3] + vecU * spacM / 2,
                Spacing = spacM,
                Number = nm / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion

            #region Implant
            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntMAs[0],
                Spacing = spacMA,
                Number = (nmA % 2 == 0) ? nmA / 2 : nmA / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntMAs[3],
                Spacing = spacMA,
                Number = (nmA % 2 == 0) ? nmA / 2 : nmA / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntMAs[0] + vecU * spacMA / 2,
                Spacing = spacMA,
                Number = nmA / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntMAs[3] + vecU * spacMA / 2,
                Spacing = spacMA,
                Number = nmA / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 1
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion
            #endregion

            #region EdgeRight
            #region Edge11
            #region Lockhead
            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[0],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[3],
                Spacing = spacE11,
                Number = (ne11 % 2 == 0) ? ne11 / 2 : ne11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[0] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[3] + vecU * spacE11 / 2,
                Spacing = spacE11,
                Number = ne11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigV2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion

            #region Implant
            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE2As[0],
                Spacing = spacEA11,
                Number = (neA11 % 2 == 0) ? neA11 / 2 : neA11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE2As[3],
                Spacing = spacEA11,
                Number = (neA11 % 2 == 0) ? neA11 / 2 : neA11 / 2 + 1,
                Normal = vecX,
                RebarLocation = RebarLocation.L1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE2As[0] + vecU * spacEA11 / 2,
                Spacing = spacEA11,
                Number = neA11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE2As[3] + vecU * spacEA11 / 2,
                Spacing = spacEA11,
                Number = neA11 / 2,
                Normal = vecX,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion
            #endregion

            #region Edge2
            #region Lockhead
            if (isDoubleNE2)
            {
                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecV * spacE2 / 2,
                    Spacing = spacE2,
                    Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LockheadDirection = vecExpBigU1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[1] + vecV * spacE2 / 2,
                Spacing = spacE2,
                Number = (ne2 % 2 == 0) ? (ne2 - 2) / 2 : (ne2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigU2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            if (isDoubleNE2)
            {
                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecV * spacE2,
                    Spacing = spacE2,
                    Number = (ne2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LockheadDirection = vecExpBigU1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }

            ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
            {
                StartPoint = pntE2s[1] + vecV * spacE2,
                Spacing = spacE2,
                Number = (ne2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LockheadDirection = vecExpBigU1,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion

            #region Implant
            if (isDoubleNEA2)
            {
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2As[0] + vecV * spacEA2 / 2,
                    Spacing = spacEA2,
                    Number = (neA2 % 2 == 0) ? (neA2 - 2) / 2 : (neA2 - 2) / 2 + 1,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE2As[1] + vecV * spacEA2 / 2,
                Spacing = spacEA2,
                Number = (neA2 % 2 == 0) ? (neA2 - 2) / 2 : (neA2 - 2) / 2 + 1,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);

            if (isDoubleNEA2)
            {
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2As[0] + vecV * spacEA2,
                    Spacing = spacEA2,
                    Number = (neA2 - 2) / 2,
                    Normal = vecY,
                    RebarLocation = RebarLocation.L2,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            }

            ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
            {
                StartPoint = pntE2As[1] + vecV * spacEA2,
                Spacing = spacEA2,
                Number = (neA2 - 2) / 2,
                Normal = vecY,
                RebarLocation = RebarLocation.L2,
                LocationIndex = 0
            };
            LockheadStandardPlaneInfos.Add(ipPlSinInfo);
            #endregion
            #endregion

            #region Edge12
            for (int i = 0; i < ie12.Count; i++)
            {
                #region Lockhead
                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecV * spacE2 / 2 * ie12[i] + vecU * spacE12 / 2,
                    Spacing = spacE12,
                    Number = (ne12 % 2 == 0) ? (ne12 - 2) / 2 : (ne12 - 2) / 2 + 1,
                    Normal = vecX,
                    RebarLocation = (ie12[i] % 2 == 0) ? RebarLocation.L2 : RebarLocation.L1,
                    LockheadDirection = vecExpBigU1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new LockheadStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2s[0] + vecV * spacE2 / 2 * ie12[i] + vecU * spacE12,
                    Spacing = spacE12,
                    Number = (ne12 - 2) / 2,
                    Normal = vecX,
                    RebarLocation = (ie12[i] % 2 == 0) ? RebarLocation.L1 : RebarLocation.L2,
                    LockheadDirection = vecExpBigU1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
                #endregion
            }

            for (int i = 0; i < ie12.Count; i++)
            {
                #region Implant
                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2As[0] + vecV * spacEA2 / 2 * ieA12[i] + vecU * spacEA12 / 2,
                    Spacing = spacEA12,
                    Number = (neA12 % 2 == 0) ? (neA12 - 2) / 2 : (neA12 - 2) / 2 + 1,
                    Normal = vecX,
                    RebarLocation = (ieA12[i] % 2 == 0) ? RebarLocation.L2 : RebarLocation.L1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);

                ipPlSinInfo = new ImplantStandardPlaneSingleInfo()
                {
                    StartPoint = pntE2As[0] + vecV * spacEA2 / 2 * ieA12[i] + vecU * spacEA12 / 2,
                    Spacing = spacEA12,
                    Number = (neA12 - 2) / 2,
                    Normal = vecX,
                    RebarLocation = (ieA12[i] % 2 == 0) ? RebarLocation.L2 : RebarLocation.L1,
                    LocationIndex = 0
                };
                LockheadStandardPlaneInfos.Add(ipPlSinInfo);
                #endregion
            }
            #endregion
            #endregion
        }
        public void CreateRebar(int idTurn, int locIndex)
        {
            NormalStandardPlaneInfos.ForEach(x => x.CreateRebar(idTurn, locIndex));
        }
    }
}
