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
namespace AutoRebaring.ElementInfo
{
    public class ColumnPlaneInfo : IPlaneInfo
    {
        #region IPlaneInfo
        public List<double> B1s { get { return new List<double> { B1 }; } }
        public List<double> B2s { get { return new List<double> { B2 }; } }
        public List<List<UV>> BoundaryPointLists { get { return new List<List<UV>> { BoundaryPoints }; } }
        public List<ShortenType> ShortenTypes { get { return new List<ShortenType> { ShortenType }; } }
        public XYZ VectorX { get; set; }
        public XYZ VectorY { get; set; }
        public List<List<UV>> StandardRebarPointLists { get; set; }
        public List<List<UV>> StirrupRebarPointLists { get; set; }
        public IPlaneInfo PlaneInfoAfter { get; set; }
        #endregion

        public double b1;
        public double b2;
        public UV vecU;
        public UV vecV;
        public double B1 { get { if (!IsReversedDimension) return b1; return b2; } }
        public double B2 { get { if (!IsReversedDimension) return b2; return b1; } }
        public UV VectorU { get { if (!IsReversedDimension) return vecU; return vecV; } }
        public UV VectorV { get { if (!IsReversedDimension) return vecV; return vecU; } }
        public UV CentralPoint { get; set; }
        public bool IsReversedDimension { get; set; }
        public List<UV> BoundaryPoints
        {
            get
            {
                return new List<UV>
                {
                    CentralPoint - VectorU * B1 / 2 - VectorV * B2 / 2,
                    CentralPoint + VectorU * B1 / 2 - VectorV * B2 / 2,
                    CentralPoint + VectorU * B1 / 2 + VectorV * B2 / 2,
                    CentralPoint - VectorU * B1 / 2 + VectorV * B2 / 2
                };
            }
        }
        public ShortenType ShortenType { get; set; }

        private ColumnPlaneInfo cpiAfter;
        public ColumnPlaneInfo CPIAfter
        {
            get
            {
                return cpiAfter;
            }
            set
            {
                cpiAfter = value;
                CheckShotenType();
            }
        }
        public ColumnPlaneInfo(IRevitInfo revitInfo, ColumnParameter param)
        {
            IsReversedDimension = false;
            Document doc = revitInfo.Document;
            Element e = revitInfo.Element;
            Element etype = revitInfo.Document.GetElement(e.GetTypeId());
            b1 = etype.LookupParameter(param.B1_Param).AsDouble();
            b2 = etype.LookupParameter(param.B2_Param).AsDouble();

            Transform tf = ((FamilyInstance)e).GetTransform();
            XYZ vecX = GeomUtil.IsBigger(tf.BasisX, -tf.BasisX) ? tf.BasisX : -tf.BasisX;
            XYZ vecY = GeomUtil.IsBigger(tf.BasisY, -tf.BasisY) ? tf.BasisY : -tf.BasisY;
            vecU = new UV(vecX.X, vecX.Y); vecV = new UV(vecY.X, vecY.Y);

            XYZ pnt = (e.Location as LocationPoint).Point;
            CentralPoint = new UV(pnt.X, pnt.Y);
        }
        public ColumnPlaneInfo(UV vecU, UV vecV, UV centralPnt)
        {
            IsReversedDimension = false;
            this.vecU = vecU; this.vecV = vecV;
            CentralPoint = centralPnt;
        }
        public GeneralParameterInput GeneralParameterInput { get; set; }

        private void CheckShotenType()
        {
            double d = 0;
            List<UV> pnts = BoundaryPoints;
            List<UV> pntAs = cpiAfter.BoundaryPoints;

            ShortenType.ShortenU1 = GetShorten(pnts[0].U, pntAs[0].U, out d);
            ShortenType.DeltaU1 = d;

            ShortenType.ShortenU2 = GetShorten(pnts[2].U, pntAs[2].U, out d);
            ShortenType.DeltaU2 = d;

            ShortenType.ShortenV1 = GetShorten(pnts[0].V, pntAs[0].V, out d);
            ShortenType.DeltaV1 = d;

            ShortenType.ShortenV2 = GetShorten(pnts[2].V, pntAs[2].V, out d);
            ShortenType.DeltaV2 = d;
        }

        private ShortenEnum GetShorten(double u, double uAfter, out double d)
        {
            d = Math.Abs(u - uAfter);
            double shorten = ConstantValue.milimeter2Feet * GeneralParameterInput.ShortenLimit;
            if (GeomUtil.IsEqual(shorten, d) || d > shorten)
                return ShortenEnum.Big;
            else if (GeomUtil.IsBigger(d, 0))
                return ShortenEnum.Small;
            return ShortenEnum.None;
        }
    }
    public class WallPlaneInfo
    {
        #region IPlaneInfo
        public List<double> B1s { get; }
        public List<double> B2s { get; }
        public List<UV> VectorUs { get; }
        public List<UV> VectorVs { get; }
        public List<List<UV>> BoundaryPointLists { get; }
        public List<ShortenType> ShortenTypes { get; }
        #endregion

        public double B1 { get; set; }
        public double B2 { get; set; }
        public UV VectorU { get; set; }
        public UV VectorV { get; set; }
        public UV CentralPoint { get; set; }
        public List<ColumnPlaneInfo> PlaneInfos { get; set; }
        public WallPlaneInfo(IRevitInfo revitInfo, ColumnParameter param)
        {

        }
    }
}
