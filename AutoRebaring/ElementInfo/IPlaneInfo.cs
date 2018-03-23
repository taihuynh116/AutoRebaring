using Autodesk.Revit.DB;
using AutoRebaring.Database;
using AutoRebaring.ElementInfo.Shorten;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.ElementInfo.Shorten;

namespace AutoRebaring.ElementInfo
{
    public interface IPlaneInfo
    {
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
    }
    public class ColumnPlaneInfo 
    {
        #region IPlaneInfo
        public List<double> B1s { get { return new List<double> { B1 }; } }
        public List<double> B2s { get { return new List<double> { B2 }; } }
        public List<UV> VectorUs { get { return new List<UV> { VectorU }; } }
        public List<UV> VectorVs { get { return new List<UV> { VectorV }; } }
        public List<List<UV>> BoundaryPointLists { get { return new List<List<UV>> { BoundaryPoints }; } }
        public List<ShortenType> ShortenTypes { get { return new List<ShortenType> { ShortenType }; } }
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
        public void CheckShotenType()
        {
            double d = 0;
            Shorten.Shorten st = Shorten.Shorten.None;

            st = GetShorten(BoundaryPoints[0].U, CPIAfter.BoundaryPoints[0].U, out d);
            ShortenType.ShortenU1 = st;
            ShortenType.DeltaU1 = d;

            st = GetShorten(BoundaryPoints[0].V, CPIAfter.BoundaryPoints[0].V, out d);
            ShortenType.ShortenV1 = st;
            ShortenType.DeltaV1 = d;
        }
        public Shorten.Shorten GetShorten(double u, double uAfter, out double d)
        {
            d = Math.Abs(uAfter - u);
            if (uAfter < u) return Shorten.Shorten.Big;
            return Shorten.Shorten.None;
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
