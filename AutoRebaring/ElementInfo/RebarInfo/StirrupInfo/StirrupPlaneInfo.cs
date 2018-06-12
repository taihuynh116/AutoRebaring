using Autodesk.Revit.DB;
using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using AutoRebaring.ElementInfo.RebarInfo.StirrupInfo.SingleInfo;
using AutoRebaring.Single;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StirrupInfo
{
    public class ColumnStirrupPlaneInfo : IStirrupPlaneInfo
    {
        public int ID { get; set; }
        public List<IStirrupPlaneSingleInfo> EdgeCoverStirrupPlaneInfo { get; set; } = new List<IStirrupPlaneSingleInfo>();
        public List<IStirrupPlaneSingleInfo> CoverStirrupPlaneInfos { get; set; } = new List<IStirrupPlaneSingleInfo>();
        public List<IStirrupPlaneSingleInfo> CStirrupPlaneInfos { get; set; } = new List<IStirrupPlaneSingleInfo>();
        public ColumnStirrupPlaneInfo(int id)
        {
            ID = id;
            GetCoverStirrupPlaneInfos();
            GetCStirrupPlaneInfos();
        }
        private void GetCoverStirrupPlaneInfos()
        {
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            List<UV> pnts = planeInfo.StirrupRebarPointLists[0];
            ARCoverParameter cp = Singleton.Instance.CoverParameter;
            double concCover = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            UV vecU = planeInfo.VectorU, vecV = planeInfo.VectorV;
            double xValue = planeInfo.B1s[0] - concCover * 2;
            double yValue = planeInfo.B2s[0] - concCover * 2;
            double stirDia = designInfo.StirrupDiameters[0];

            StirrupPlaneSingleInfo stPlSinInfo = new StirrupPlaneSingleInfo()
            {
                StartPoint = pnts[0],
                VectorX = planeInfo.VectorX,
                VectorY = planeInfo.VectorY,
                ParameterValues = new List<double> { xValue, xValue, yValue, yValue },
                StirrupType = StirrupTypeEnum.PStirrup,
                CenterPoint = (pnts[0] + pnts[2]) / 2 + (-vecU + vecV) * stirDia / 4
            };
            CoverStirrupPlaneInfos.Add(stPlSinInfo);
        }
        private void GetCStirrupPlaneInfos()
        {
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            ARCoverParameter cp = Singleton.Instance.CoverParameter;

            List<UV> stanPnts = planeInfo.StandardRebarPointLists[0];

            StirrupPlaneSingleInfo stPlSinInfo = null;
            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            double concCover = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            UV stanPnt1 = new UV((stanPnts[0].U + stanPnts[3].U) / 2, (stanPnts[0].V + stanPnts[3].V) / 2);
            UV stanPnt2 = new UV((stanPnts[1].U + stanPnts[2].U) / 2, (stanPnts[1].V + stanPnts[2].V) / 2);
            UV stanPnt3 = new UV((stanPnts[0].U + stanPnts[1].U) / 2, (stanPnts[0].V + stanPnts[1].V) / 2);
            UV stanPnt4 = new UV((stanPnts[2].U + stanPnts[3].U) / 2, (stanPnts[2].V + stanPnts[3].V) / 2);
            double spacb1 = designInfo.StandardSpacings[0];
            double spacb2 = designInfo.StandardSpacings[1];
            double xValue = planeInfo.B1s[0] - concCover * 2;
            double yValue = planeInfo.B2s[0] - concCover * 2;
            int n1 = designInfo.StandardNumbers[0];
            int n2 = designInfo.StandardNumbers[1];
            int n1S = designInfo.StandardNumbers[0] - 4;
            int n2S = designInfo.StandardNumbers[1] - 4;
            double dia = designInfo.StandardDiameters[0];

            if (n1S > 0)
            {
                for (int i = 0; i < n1S / 2; i++)
                {
                    stPlSinInfo = new StirrupPlaneSingleInfo()
                    {
                        StartPoint = stanPnt1 + ((i + 1) * spacb1 + dia) * vecU,
                        VectorX = vecY,
                        VectorY = vecY.CrossProduct(XYZ.BasisZ),
                        ParameterValues = new List<double> { yValue },
                        StirrupType = StirrupTypeEnum.CStirrup_UDir
                    };
                    CStirrupPlaneInfos.Add(stPlSinInfo);
                }
                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    StartPoint = stanPnt1 + ((n1 - 3) * spacb1 / 2 + dia) * vecU,
                    VectorX = vecY,
                    VectorY = vecY.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { yValue },
                    StirrupType = StirrupTypeEnum.CStirrup_UDir
                };
                CStirrupPlaneInfos.Add(stPlSinInfo);
            }

            if (n2S > 0)
            {
                for (int i = 0; i < n2S / 2; i++)
                {
                    stPlSinInfo = new StirrupPlaneSingleInfo()
                    {
                        StartPoint = stanPnt3 + ((i + 1) * spacb2 + dia) * vecV,
                        VectorX = -vecX,
                        VectorY = -vecX.CrossProduct(XYZ.BasisZ),
                        ParameterValues = new List<double> { xValue },
                        StirrupType = StirrupTypeEnum.CStirrup_VDir
                    };
                    CStirrupPlaneInfos.Add(stPlSinInfo);
                }
                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    StartPoint = stanPnt3 + ((n2 - 3) * spacb2 / 2 + dia) * vecV,
                    VectorX = -vecX,
                    VectorY = -vecX.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { xValue },
                    StirrupType = StirrupTypeEnum.CStirrup_VDir
                };
                CStirrupPlaneInfos.Add(stPlSinInfo);
            }
        }
        public void CreateRebar(int idStirDis)
        {
            CoverStirrupPlaneInfos.ForEach(x => x.CreateRebars(ID, idStirDis));
            CStirrupPlaneInfos.ForEach(x => x.CreateRebars(ID, idStirDis));
        }
    }

    public class WallStirrupPlaneInfo : IStirrupPlaneInfo
    {
        public int ID { get; set; }
        public List<IStirrupPlaneSingleInfo> EdgeCoverStirrupPlaneInfo { get; set; } = new List<IStirrupPlaneSingleInfo>();
        public List<IStirrupPlaneSingleInfo> CoverStirrupPlaneInfos { get; set; } = new List<IStirrupPlaneSingleInfo>();
        public List<IStirrupPlaneSingleInfo> CStirrupPlaneInfos { get; set; } = new List<IStirrupPlaneSingleInfo>();
        public WallStirrupPlaneInfo(int id)
        {
            ID = id;
            GetEdgeCoverStirrupPlaneInfos();
            //GetCoverStirrupPlaneInfos();
            GetCStirrupPlaneInfos();
        }
        private void GetEdgeCoverStirrupPlaneInfos()
        {
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            List<UV> pnts1 = planeInfo.StirrupRebarPointLists[1];
            List<UV> pnts2 = planeInfo.StirrupRebarPointLists[3];
            ARCoverParameter cp = Singleton.Instance.CoverParameter;
            double concCover = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            double stirDia = designInfo.StirrupDiameters[1];
            double xValue = planeInfo.B1s[0] - concCover + stirDia/2;
            double yValue = planeInfo.B2s[0] - concCover * 2;

            UV vecU = planeInfo.VectorU, vecV = planeInfo.VectorV;

            StirrupPlaneSingleInfo stPlSinInfo = new StirrupPlaneSingleInfo()
            {
                StartPoint = pnts1[0],
                VectorX = planeInfo.VectorX,
                VectorY = planeInfo.VectorY,
                ParameterValues = new List<double> { xValue, xValue, yValue, yValue },
                StirrupType = StirrupTypeEnum.PStirrup,
                CenterPoint = (pnts1[0] + pnts1[2]) / 2 + (-vecU + vecV) * stirDia / 4
            };
            EdgeCoverStirrupPlaneInfo.Add(stPlSinInfo);

            stPlSinInfo = new StirrupPlaneSingleInfo()
            {
                StartPoint = pnts2[0],
                VectorX = planeInfo.VectorX,
                VectorY = planeInfo.VectorY,
                ParameterValues = new List<double> { xValue, xValue, yValue, yValue },
                StirrupType = StirrupTypeEnum.PStirrup,
                CenterPoint = (pnts2[0] + pnts2[2]) / 2 + (-vecU + vecV) * stirDia / 4
            };
            EdgeCoverStirrupPlaneInfo.Add(stPlSinInfo);
        }
        private void GetCoverStirrupPlaneInfos()
        {
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            WallPlaneInfo wpI = planeInfo as WallPlaneInfo;
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            List<UV> pnts = planeInfo.StirrupRebarPointLists[0];
            ARCoverParameter cp = Singleton.Instance.CoverParameter;
            ARDevelopmentParameter dp = Singleton.Instance.DevelopmentParameter;
            double concCover = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            int devMutli = dp.DevelopmentMultiply;
            UStirrupLapEnum uLapType = Singleton.Instance.UStirrupLapType;
            double stirDia = designInfo.StirrupDiameters[0];

            UV vecU = planeInfo.VectorU, vecV = planeInfo.VectorV;
            UV p1 = null, p2 = null;
            UV centerPnt1 = null, centerPnt2 = null;
            double bungDim = 0, canhDim = 0;
            XYZ vecX = null, vecY = null;
            switch (uLapType)
            {
                case UStirrupLapEnum.Horizontal:
                    p1 = pnts[0];
                    p2 = pnts[2];
                    bungDim = wpI.B1 - concCover * 2;
                    canhDim = wpI.B2 / 2 - concCover + stirDia * devMutli / 2;
                    vecX = wpI.VectorX;
                    vecY = wpI.VectorY;
                    centerPnt1 = (pnts[0] + pnts[1]) / 2 + vecV * (canhDim / 2 - stirDia / 4);
                    centerPnt2 = (pnts[2] + pnts[3]) / 2 - vecV * (canhDim / 2 - stirDia / 4);
                    break;
                case UStirrupLapEnum.Vertical:
                    p1 = pnts[0];
                    p2 = pnts[1];
                    bungDim = wpI.B2 - concCover * 2;
                    canhDim = wpI.B1 / 2 - concCover + stirDia * devMutli / 2;
                    vecX = wpI.VectorY;
                    vecY = wpI.VectorX;
                    centerPnt1 = (pnts[0] + pnts[3]) / 2 + vecU * (canhDim / 2 - stirDia / 4);
                    centerPnt2 = (pnts[1] + pnts[2]) / 2 - vecU * (canhDim / 2 - stirDia / 4);
                    break;
            }
            StirrupPlaneSingleInfo stPlSinInfo = new StirrupPlaneSingleInfo()
            {
                StartPoint = p1,
                VectorX = vecX,
                VectorY = vecY,
                ParameterValues = new List<double> { canhDim, bungDim, canhDim },
                StirrupType = StirrupTypeEnum.UStirrup_UDir,
                CenterPoint = centerPnt1
            };
            CoverStirrupPlaneInfos.Add(stPlSinInfo);

            stPlSinInfo = new StirrupPlaneSingleInfo()
            {
                StartPoint = p2,
                VectorX = -vecX,
                VectorY = -vecY,
                ParameterValues = new List<double> { canhDim, bungDim, canhDim },
                StirrupType = StirrupTypeEnum.UStirrup_UDir,
                CenterPoint = centerPnt2
            };
            CoverStirrupPlaneInfos.Add(stPlSinInfo);
        }
        private void GetCStirrupPlaneInfos()
        {
            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(ID);
            ARCoverParameter cp = Singleton.Instance.CoverParameter;
            WallStandardPlaneInfo wspi = Singleton.Instance.GetStandardPlaneInfo(ID) as WallStandardPlaneInfo;
            List<int> ieIndexs = wspi.NE12Indexs;

            List<UV> standPnts1 = planeInfo.StandardRebarPointLists[0];
            List<UV> standPnts2 = planeInfo.StandardRebarPointLists[2];

            StirrupPlaneSingleInfo stPlSinInfo = null;
            XYZ vecX = planeInfo.VectorX;
            XYZ vecY = planeInfo.VectorY;
            UV vecU = planeInfo.VectorU;
            UV vecV = planeInfo.VectorV;
            double concCover = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            UV standPnt11 = new UV((standPnts1[0].U + standPnts1[3].U) / 2, (standPnts1[0].V + standPnts1[3].V) / 2);
            UV standPnt12 = new UV((standPnts2[0].U + standPnts2[3].U) / 2, (standPnts2[0].V + standPnts2[3].V) / 2);
            UV standPnt21 = new UV((standPnts1[0].U + standPnts1[1].U) / 2, (standPnts1[0].V + standPnts1[1].V) / 2);
            UV standPnt22 = new UV((standPnts2[0].U + standPnts2[1].U) / 2, (standPnts2[0].V + standPnts2[1].V) / 2);

            double stirDia = designInfo.StirrupDiameters[2];
            double spacb1 = designInfo.StandardSpacings[0];
            double spacb2 = designInfo.StandardSpacings[2];
            double xValue = planeInfo.B1s[0] - concCover + stirDia / 2;
            double yValue = planeInfo.B2s[0] - concCover * 2;
            int n1 = designInfo.StandardNumbers[0];
            int n1S = designInfo.StandardNumbers[0] - 4;
            double dia = designInfo.StandardDiameters[0];

            if (n1S > 0)
            {
                for (int i = 0; i < n1S/2; i++)
                {
                    stPlSinInfo = new StirrupPlaneSingleInfo()
                    {
                        StartPoint = standPnt11 + ((i + 1) * spacb1 + dia) * vecU,
                        VectorX = vecY,
                        VectorY = vecY.CrossProduct(XYZ.BasisZ),
                        ParameterValues = new List<double> { yValue},
                        StirrupType = StirrupTypeEnum.CStirrup_UDir
                    };
                    CStirrupPlaneInfos.Add(stPlSinInfo);

                    stPlSinInfo = new StirrupPlaneSingleInfo()
                    {
                        StartPoint = standPnt12 + ((i + 1) * spacb1 + dia) * vecU,
                        VectorX = vecY,
                        VectorY = vecY.CrossProduct(XYZ.BasisZ),
                        ParameterValues = new List<double> { yValue },
                        StirrupType = StirrupTypeEnum.CStirrup_UDir
                    };
                    CStirrupPlaneInfos.Add(stPlSinInfo);
                }
                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    StartPoint = standPnt11 + ((n1 - 3) * spacb1 / 2 + dia) * vecU,
                    VectorX = vecY,
                    VectorY = vecY.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { yValue },
                    StirrupType = StirrupTypeEnum.CStirrup_UDir
                };
                CStirrupPlaneInfos.Add(stPlSinInfo);

                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    StartPoint = standPnt12 + ((n1 - 3) * spacb1 / 2 + dia) * vecU,
                    VectorX = vecY,
                    VectorY = vecY.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { yValue },
                    StirrupType = StirrupTypeEnum.CStirrup_UDir
                };
                CStirrupPlaneInfos.Add(stPlSinInfo);
            }
            for (int i = 0; i < ieIndexs.Count; i++)
            {
                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    StartPoint = standPnt21 + ((ieIndexs[i]) * spacb2/2 + dia) * vecV,
                    VectorX = -vecX,
                    VectorY = -vecX.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { xValue },
                    StirrupType = StirrupTypeEnum.CStirrup_VDir
                };
                CStirrupPlaneInfos.Add(stPlSinInfo);

                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    StartPoint = standPnt22 + ((ieIndexs[i]) * spacb2/2 + dia) * vecV,
                    VectorX = -vecX,
                    VectorY = -vecX.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { xValue },
                    StirrupType = StirrupTypeEnum.CStirrup_VDir
                };
                CStirrupPlaneInfos.Add(stPlSinInfo);
            }
        }
        public void CreateRebar(int idStirDis)
        {
            EdgeCoverStirrupPlaneInfo.ForEach(x => x.CreateRebars(ID, idStirDis));
            CoverStirrupPlaneInfos.ForEach(x => x.CreateRebars(ID, idStirDis));
            CStirrupPlaneInfos.ForEach(x => x.CreateRebars(ID, idStirDis));
        }
    }
}
