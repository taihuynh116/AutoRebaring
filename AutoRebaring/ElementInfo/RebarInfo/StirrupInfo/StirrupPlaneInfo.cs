using Autodesk.Revit.DB;
using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo.RebarInfo.StirrupInfo.SingleInfo;
using AutoRebaring.Single;
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
        public List<IStirrupPlaneSingleInfo> CoverStirrupPlaneInfos { get; set; }
        public List<IStirrupPlaneSingleInfo> CStirrupPlaneInfos { get; set; }
        public ColumnStirrupPlaneInfo(int id)
        {
            ID = id;
            GetCoverStirrupPlaneInfos();
            GetCStirrupPlaneInfos();
        }
        private void GetCoverStirrupPlaneInfos()
        {
            CoverStirrupPlaneInfos = new List<IStirrupPlaneSingleInfo>();

            IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(ID);
            List<UV> pnts = planeInfo.StirrupRebarPointLists[0];
            ARCoverParameter cp = Singleton.Instance.CoverParameter;
            double concCover = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            double xValue = planeInfo.B1s[0] - concCover * 2;
            double yValue = planeInfo.B2s[0] - concCover * 2;

            StirrupPlaneSingleInfo stPlSinInfo = new StirrupPlaneSingleInfo()
            {
                IDStirrupShape = 0,
                StartPoint = new UV((pnts[0].U + pnts[2].U) / 2, (pnts[0].V + pnts[2].V) / 2),
                VectorX = planeInfo.VectorX,
                VectorY = planeInfo.VectorY,
                ParameterValues = new List<double> { xValue,xValue, yValue , yValue}
            };
            CoverStirrupPlaneInfos.Add(stPlSinInfo);
        }
        private void GetCStirrupPlaneInfos()
        {
            CStirrupPlaneInfos = new List<IStirrupPlaneSingleInfo>();

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
                        IDStirrupShape = 1,
                        StartPoint = stanPnt1 + ((i + 1) * spacb1 + dia) * vecU,
                        VectorX = vecY,
                        VectorY = vecY.CrossProduct(XYZ.BasisZ),
                        ParameterValues = new List<double> { yValue }
                    };
                    CStirrupPlaneInfos.Add(stPlSinInfo);
                }
                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    IDStirrupShape = 1,
                    StartPoint = stanPnt1 + ((n1 - 3) * spacb1 / 2 + dia) * vecU,
                    VectorX = vecY,
                    VectorY = vecY.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { yValue }
                };
                CStirrupPlaneInfos.Add(stPlSinInfo);
            }

            if (n2S > 0)
            {
                for (int i = 0; i < n2S / 2; i++)
                {
                    stPlSinInfo = new StirrupPlaneSingleInfo()
                    {
                        IDStirrupShape = 1,
                        StartPoint = stanPnt3 + ((i + 1) * spacb2 + dia) * vecV,
                        VectorX = vecX,
                        VectorY = vecX.CrossProduct(XYZ.BasisZ),
                        ParameterValues = new List<double> { xValue }
                    };
                    CStirrupPlaneInfos.Add(stPlSinInfo);
                }
                stPlSinInfo = new StirrupPlaneSingleInfo()
                {
                    IDStirrupShape = 1,
                    StartPoint = stanPnt3 + ((n2 - 3) * spacb2 / 2 + dia) * vecV,
                    VectorX = vecX,
                    VectorY = vecX.CrossProduct(XYZ.BasisZ),
                    ParameterValues = new List<double> { xValue }
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
}
