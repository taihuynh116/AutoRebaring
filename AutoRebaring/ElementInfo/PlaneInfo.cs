using Autodesk.Revit.DB;
using AutoRebaring.Database;
using AutoRebaring.ElementInfo.Shorten;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.Constant;
using AutoRebaring.ElementInfo;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.Single;

namespace AutoRebaring.ElementInfo
{
    public class PlaneInfo
    {
        public int ID { get; set; }
        public double B1 { get; set; }
        public double B2 { get; set; }
        public UV VectorU { get; set; }
        public UV VectorV { get; set; }
        public XYZ VectorX { get; set; }
        public XYZ VectorY { get; set; }
        public UV CentralPoint { get; set; }
        public PlaneInfo(int id)
        {
            ID = id;
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(id);
            Document doc = revitInfo.Document;
            Element e = revitInfo.Element;
            GetPlaneInfo(doc, e);
        }   
        public PlaneInfo(Document doc, Element e)
        {
            GetPlaneInfo(doc, e);
        }
        public PlaneInfo(IRevitInfo revitInfo, ARWallParameter wp)
        {
            Document doc = revitInfo.Document;
            Element e = revitInfo.Element;
            GetPlaneInfo(doc, e);
        }
        public void GetPlaneInfo(Document doc, Element e)
        {
            Element etype = doc.GetElement(e.GetTypeId());

            if (!(e is Wall))
            {
                B1 = etype.LookupParameter(ConstantValue.B1Param_Column).AsDouble();
                B2 = etype.LookupParameter(ConstantValue.B2Param_Column).AsDouble();
                Transform tf = ((FamilyInstance)e).GetTransform();
                XYZ vecX = GeomUtil.IsBigger(tf.BasisX, -tf.BasisX) ? tf.BasisX : -tf.BasisX;
                XYZ vecY = GeomUtil.IsBigger(tf.BasisY, -tf.BasisY) ? tf.BasisY : -tf.BasisY;
                VectorU = new UV(vecX.X, vecX.Y); VectorV = new UV(vecY.X, vecY.Y);

                XYZ pnt = (e.Location as LocationPoint).Point;
                CentralPoint = new UV(pnt.X, pnt.Y);
            }
            else
            {
                B1 = e.LookupParameter(ConstantValue.B1Param_Wall).AsDouble();
                B2 = etype.LookupParameter(ConstantValue.B2Param_Wall).AsDouble();
                Line l = (e.Location as LocationCurve).Curve as Line;
                XYZ vecX = l.Direction.Normalize();
                XYZ vecY = XYZ.BasisZ.CrossProduct(vecX).Normalize();
                vecX = GeomUtil.IsBigger(vecX, -vecX) ? vecX : -vecX;
                vecY = GeomUtil.IsBigger(vecY, -vecY) ? vecY : -vecY;
                VectorU = new UV(vecX.X, vecX.Y); VectorV = new UV(vecY.X, vecY.Y);

                XYZ pnt1 = l.GetEndPoint(0), pnt2 = l.GetEndPoint(1);
                CentralPoint = new UV((pnt1.X + pnt2.X) / 2, (pnt1.Y + pnt2.Y) / 2);
            }

            List<UV> boundPnts = new List<UV>
            {
                CentralPoint - VectorU * B1 / 2 - VectorV * B2 / 2,
                CentralPoint + VectorU * B1 / 2 - VectorV * B2 / 2,
                CentralPoint + VectorU * B1 / 2 + VectorV * B2 / 2,
                CentralPoint - VectorU * B1 / 2 + VectorV * B2 / 2
            };

            VectorX = new XYZ(VectorU.U, VectorU.V, 0);
            VectorY = new XYZ(VectorV.U, VectorV.V, 0);
        }
    }
    public class ColumnPlaneInfo : PlaneInfo, IPlaneInfo
    {
        #region IPlaneInfo
        public List<double> B1s { get; set; }
        public List<double> B2s { get; set; }
        public List<List<UV>> BoundaryPointLists { get; set; }
        public List<ShortenType> ShortenTypes { get; set; }
        
        public List<List<UV>> StandardRebarPointLists { get; set; }
        public List<List<UV>> StirrupRebarPointLists { get; set; }
        #endregion

        public ColumnPlaneInfo(int id) : base(id)
        {
            GetFullPlaneInfo();
        }
        public void GetFullPlaneInfo()
        {
            List<UV> boundPoints = new List<UV>
            {
                CentralPoint - VectorU * B1 / 2 - VectorV * B2 / 2,
                CentralPoint + VectorU * B1 / 2 - VectorV * B2 / 2,
                CentralPoint + VectorU * B1 / 2 + VectorV * B2 / 2,
                CentralPoint - VectorU * B1 / 2 + VectorV * B2 / 2
            };

            B1s = new List<double> { B1 };
            B2s = new List<double> { B2 };
            BoundaryPointLists = new List<List<UV>> { boundPoints };
        }
        public void GetShortenType()
        {
            ShortenTypes = new List<ShortenType>
            {
                getShotenType()
            };
        }
        private ShortenType getShotenType()
        {
            double d = 0;
            List<UV> pnts = BoundaryPointLists[0];
            List<UV> pntAs = Singleton.Instance.GetPlaneInfoAfter(ID).BoundaryPointLists[0];

            return new ShortenType()
            {
                ShortenU1 = GetShorten(pnts[0].U, pntAs[0].U, out d),
                DeltaU1 = d,
                ShortenU2 = GetShorten(pnts[2].U, pntAs[2].U, out d),
                DeltaU2 = d,
                ShortenV1 = GetShorten(pnts[0].V, pntAs[0].V, out d),
                DeltaV1 = d,
                ShortenV2 = GetShorten(pnts[2].V, pntAs[2].V, out d),
                DeltaV2 = d
            };
        }
        private ShortenEnum GetShorten(double u, double uAfter, out double d)
        {
            ARLockheadParameter lp = Singleton.Instance.LockheadParameter;
            d = Math.Abs(u - uAfter);
            double shorten = ConstantValue.milimeter2Feet * lp.ShortenLimit;
            if (GeomUtil.IsEqual(shorten, d) || d > shorten)
                return ShortenEnum.Big;
            else if (GeomUtil.IsBigger(d, 0))
                return ShortenEnum.Small;
            return ShortenEnum.None;
        }
        public void GetRebarLocation()
        {
            IDesignInfo di = Singleton.Instance.GetDesignInfo(ID);
            ARCoverParameter cp = Singleton.Instance.CoverParameter;
            double offset = cp.ConcreteCover*ConstantValue.milimeter2Feet + di.StirrupDiameters[0] * ConstantValue.RebarStandardOffsetControl + di.StandardDiameters[0] / 2;
            List<UV> boundPnts = BoundaryPointLists[0];

            List<UV> standardPnts = new List<UV>()
            {
                boundPnts[0] + (VectorU+VectorV)*offset,
                boundPnts[1] + (-VectorU+VectorV)*offset,
                boundPnts[2] + (-VectorU-VectorV)*offset,
                boundPnts[3] + (VectorU-VectorV)*offset,
            };

            StandardRebarPointLists = new List<List<UV>>
            {
                standardPnts
            };

            offset = cp.ConcreteCover*ConstantValue.milimeter2Feet + di.StirrupDiameters[0]/2;
            List<UV> stirrPnts = new List<UV>()
            {
                boundPnts[0] + (VectorU+VectorV)*offset,
                boundPnts[1] + (-VectorU+VectorV)*offset,
                boundPnts[2] + (-VectorU-VectorV)*offset,
                boundPnts[3] + (VectorU-VectorV)*offset,
            };

            StirrupRebarPointLists = new List<List<UV>>
            {
                stirrPnts
            };
        }
    }
    public class WallPlaneInfo : PlaneInfo, IPlaneInfo
    {
        
        #region IPlaneInfo
        public List<double> B1s { get; set; }
        public List<double> B2s { get; set; }
        public List<List<UV>> BoundaryPointLists { get; set; }
        public List<ShortenType> ShortenTypes { get; set; }
        public List<List<UV>> StandardRebarPointLists { get; set; }
        public List<List<UV>> StirrupRebarPointLists { get; set; }
        #endregion

        public WallPlaneInfo(int id):base(id)
        {
            GetFullPlaneInfo();
        }
        public void GetFullPlaneInfo()
        {
            ARWallParameter wp = Singleton.Instance.WallParameter;
            double edge = 0;
            if (wp.EdgeWidthInclude) edge = wp.EdgeWidth * ConstantValue.milimeter2Feet;
            if (wp.EdgeRatioInclude) edge = Math.Max(edge, B1/wp.EdgeRatio);

            double middle = B1 - edge * 2;

            B1s = new List<double> { edge, middle, edge };
            B2s = new List<double> { B2, B2, B2 };
            BoundaryPointLists = new List<List<UV>>()
            {
                getBoundaryPoint(CentralPoint-VectorU*(edge+middle)/2, edge, B2),
                getBoundaryPoint(CentralPoint, middle, B2),
                getBoundaryPoint(CentralPoint+VectorU*(edge+middle)/2, edge, B2)
            };
        }
        private List<UV> getBoundaryPoint(UV centralPnt, double b1, double b2)
        {
            return new List<UV>
            {
                centralPnt - VectorU * b1 / 2 - VectorV * b2 / 2,
                centralPnt + VectorU * b1 / 2 - VectorV * b2 / 2,
                centralPnt + VectorU * b1 / 2 + VectorV * b2 / 2,
                centralPnt - VectorU * b1 / 2 + VectorV * b2 / 2
            };
        }
        public void GetShortenType()
        {
            ShortenTypes = new List<ShortenType>()
            {
                getShotenType(0),
                getShotenType(1),
                getShotenType(2)
            };
        }
        private ShortenType getShotenType(int index)
        {
            double d = 0;
            List<UV> pnts = BoundaryPointLists[index];
            List<UV> pntAs = Singleton.Instance.GetPlaneInfoAfter(ID).BoundaryPointLists[index];

            return new ShortenType()
            {
                ShortenU1 = GetShorten(pnts[0].U, pntAs[0].U, out d),
                DeltaU1 = d,
                ShortenU2 = GetShorten(pnts[2].U, pntAs[2].U, out d),
                DeltaU2 = d,
                ShortenV1 = GetShorten(pnts[0].V, pntAs[0].V, out d),
                DeltaV1 = d,
                ShortenV2 = GetShorten(pnts[2].V, pntAs[2].V, out d),
                DeltaV2 = d
            };
        }

        private ShortenEnum GetShorten(double u, double uAfter, out double d)
        {
            ARLockheadParameter lp = Singleton.Instance.LockheadParameter;
            d = Math.Abs(u - uAfter);
            double shorten = ConstantValue.milimeter2Feet * lp.ShortenLimit;
            if (GeomUtil.IsEqual(shorten, d) || d > shorten)
                return ShortenEnum.Big;
            else if (GeomUtil.IsBigger(d, 0))
                return ShortenEnum.Small;
            return ShortenEnum.None;
        }

        public void GetRebarLocation()
        {
            IDesignInfo di = Singleton.Instance.GetDesignInfo(ID);
            ARCoverParameter cp = Singleton.Instance.CoverParameter;

            StandardRebarPointLists = new List<List<UV>>();
            StirrupRebarPointLists = new List<List<UV>>();

            double offConc = cp.ConcreteCover * ConstantValue.milimeter2Feet;
            double offStirr =di.StirrupDiameters[0];
            double offCntr = di.StirrupDiameters[0]* ( ConstantValue.RebarStandardOffsetControl-1);
            double offStand= di.StandardDiameters[0];

            //cp.ConcreteCover* ConstantValue.milimeter2Feet + di.StirrupDiameters[0] * ConstantValue.RebarStandardOffsetControl + di.StandardDiameters[0] / 2;

            #region Standard
            List<UV> boundPnts = BoundaryPointLists[0];
            List<UV> pnts = new List<UV>()
            {
                boundPnts[0] + (VectorU+VectorV)* (offConc+ offStirr+offCntr+ offStand/2),
                boundPnts[1] + VectorV*(offConc+ offStirr+offCntr+ offStand/2) - VectorU*(offStirr/2 + offCntr+ offStand/2),
                boundPnts[2] - VectorV*(offConc+ offStirr+offCntr+ offStand/2) - VectorU*(offStirr/2 + offCntr+ offStand/2),
                boundPnts[3] + (VectorU-VectorV)* (offConc+ offStirr+offCntr+ offStand/2)
            };
            StandardRebarPointLists.Add(pnts);

            double offSpac = di.StandardSpacings[3]*0.5;
            offStand = di.StandardDiameters[1];
            boundPnts = BoundaryPointLists[1];
            pnts = new List<UV>()
            {
                boundPnts[0] + VectorV*(offConc + offStirr + offCntr + offStand/2)+ VectorU*(offSpac-offCntr - offStirr/2- offStand/2),
                boundPnts[1] + VectorV*(offConc + offStirr + offCntr + offStand/2)- VectorU*(offSpac-offCntr - offStirr/2- offStand/2),
                boundPnts[2] - VectorV*(offConc + offStirr + offCntr + offStand/2)- VectorU*(offSpac-offCntr - offStirr/2- offStand/2),
                boundPnts[3] - VectorV*(offConc + offStirr + offCntr + offStand/2)+ VectorU*(offSpac-offCntr - offStirr/2- offStand/2)
            };
            StandardRebarPointLists.Add(pnts);

            boundPnts = BoundaryPointLists[2];
            pnts = new List<UV>()
            {
                boundPnts[0] + VectorV*(offConc+ offStirr+offCntr+ offStand/2) + VectorU*(offStirr/2 + offCntr+ offStand/2),
                boundPnts[1] + (-VectorU+VectorV)* (offConc+ offStirr+offCntr+ offStand/2),
                boundPnts[2] - (VectorU+VectorV)* (offConc+ offStirr+offCntr+ offStand/2),
                boundPnts[3] - VectorV*(offConc+ offStirr+offCntr+ offStand/2) + VectorU*(offStirr/2 + offCntr+ offStand/2)
            };
            StandardRebarPointLists.Add(pnts);
            #endregion


            #region Stirrup
            boundPnts = BoundaryPointLists[0];
            pnts = new List<UV>()
            {
                boundPnts[0] + (VectorU+VectorV)*(offConc+ offStirr/2),
                boundPnts[1] + (VectorV)*(offConc+offStirr/2),
                boundPnts[2] + (-VectorV)*(offConc+offStirr/2),
                boundPnts[3] + (VectorU-VectorV)*(offConc+ offStirr/2)
            };
            StirrupRebarPointLists.Add(pnts);

            boundPnts = BoundaryPointLists[2];
            pnts = new List<UV>()
            {
                boundPnts[0] + (VectorV)*(offConc+offStirr/2),
                boundPnts[1] + (-VectorU+VectorV)*(offConc+ offStirr/2),
                boundPnts[2] + (-VectorU-VectorV)*(offConc+ offStirr/2),
                boundPnts[3] + (-VectorV)*(offConc+offStirr/2)
            };
            StirrupRebarPointLists.Add(pnts);
            #endregion
        }
    }
}
