using Autodesk.Revit.DB;
using AutoRebaring.Database;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Main
{
    public interface IPlaneInfo
    {
    }
    public class ColumnPlaneInfo : IPlaneInfo
    {
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
        public ShortenType ShortenU1 { get; set; }
        public ShortenType ShortenU2 { get; set; }
        public ShortenType ShortenV1 { get; set; }
        public ShortenType ShortenV2 { get; set; }
        public double DeltaU1 { get; set; }
        public double DeltaU2 { get; set; }
        public double DeltaV1 { get; set; }
        public double DeltaV2 { get; set; }

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
    }
    public class WallPlaneInfo : IPlaneInfo
    {
        public double B1 { get; set; }
        public double B2 { get; set; }
        public UV VectorU { get; set; }
        public UV VectorV { get; set; }
        public UV CentralPoint { get; set; }
        public List<ColumnPlaneInfo> PlaneInfos { get; set; }
        public WallPlaneInfo(IRevitInfo revitInfo, ColumnParameter param)
        {
            Document doc = revitInfo.Document;
            Element e = revitInfo.Element;

            B1 = e.LookupParameter("Length").AsDouble();
            B2 = (e as Wall).WallType.LookupParameter("Width").AsDouble();

            Line l = (e.Location as LocationCurve).Curve as Line;
            XYZ vecX = l.Direction.Normalize();
            XYZ vecY = XYZ.BasisZ.CrossProduct(vecX).Normalize();
            vecX = GeomUtil.IsBigger(vecX, -vecX) ? vecX : -vecX;
            vecY = GeomUtil.IsBigger(vecY, -vecY) ? vecY : -vecY;
            XYZ p1 = l.GetEndPoint(0), p2 = l.GetEndPoint(1);

            CentralPoint = new UV((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            VectorU = new UV(vecX.X, vecX.Y); VectorV = new UV(vecY.X, vecY.Y);
            
            PlaneInfos = new List<ColumnPlaneInfo>();
            PlaneInfos.Add(new ColumnPlaneInfo(VectorU, VectorV, CentralPoint- VectorU*(0.5- 0.2/2)));
            PlaneInfos.Add(new ColumnPlaneInfo(VectorU, VectorV, CentralPoint));
            PlaneInfos.Add(new ColumnPlaneInfo(VectorU, VectorV, CentralPoint + VectorU * (0.5 - 0.2 / 2)));
        }
    }

    public enum ShortenType
    {
        None, Small, Big, LockHeadFull
    }
}
