using Autodesk.Revit.DB;
using AutoRebaring.Database;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class ElementInfo : IElementInfo
    {
        public IPlaneInfo PlaneInfo { get; set; }
        public IVerticalInfo VerticalInfo { get; set; }
        public IDesignInfo DesignInfo { get; set; }
        public IRevitInfo RevitInfo { get; set; }
        public IStandardPlaneInfo StandardPlaneInfo { get; set; }

        public ElementInfo() { }
        public void GetPlaneInfo(ARElementType elemType, ARWallParameter wp)
        {
            switch (elemType.Type)
            {
                case "Column":
                    PlaneInfo = new ColumnPlaneInfo(RevitInfo);
                    break;
                case "Wall":
                    PlaneInfo = new WallPlaneInfo(RevitInfo, wp);
                    break;
            }
        }
        public void GetDesignInfo(List<IDesignInfo> designInfos)
        {
            bool isDesignAssigned = false;
            for (int j = 0; j < designInfos.Count; j++)
            {
                if (GeomUtil.IsSmaller(RevitInfo.Elevation, designInfos[j].Level.Elevation))
                {
                    DesignInfo = designInfos[j==0 ? j:j - 1];
                    isDesignAssigned = true;
                    break;
                }
                else if (GeomUtil.IsEqual(RevitInfo.Elevation, designInfos[j].Level.Elevation))
                {
                    DesignInfo = designInfos[j];
                    isDesignAssigned = true;
                    break;
                }
            }
            if (!isDesignAssigned)
            {
                DesignInfo = designInfos[designInfos.Count-1];
            }
        }
        public void GetVerticalInfo(ARElementType elemType)
        {
            switch (elemType.Type)
            {
                case "Column":
                    VerticalInfo = new ColumnVerticalInfo(RevitInfo);
                    break;
                case "Wall":
                    VerticalInfo = new WallVerticalInfo(RevitInfo);
                    break;
            }
        }
        public void GetStandardSpacing(ARCoverParameter cp)
        {
            DesignInfo.GetStandardSpacing(PlaneInfo, cp);
        }
        public void GetRebarLocation(ARCoverParameter cp)
        {
            PlaneInfo.GetRebarLocation(DesignInfo, cp);
        }
        public void GetRebarInformation(ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            VerticalInfo.GetRebarInformation(DesignInfo, ap, dp);
        }
        public void GetStandardPlaneInfo(ARElementType elemType, ARLockheadParameter lp)
        {
            switch (elemType.Type)
            {
                case "Column":
                    StandardPlaneInfo = new ColumnStandardPlaneInfo(PlaneInfo, DesignInfo,lp);
                    break;
                case "Wall":
                    StandardPlaneInfo = new WallStandardPlaneInfo(PlaneInfo, DesignInfo,lp);
                    break;
            }
        }
        public void GetShortenType(IPlaneInfo planeInfo, ARLockheadParameter lp)
        {
            PlaneInfo.GetShortenType(planeInfo, lp);
        }
        public void GetDesignInfoAB(IDesignInfo diA, IDesignInfo diB)
        {
            DesignInfo.DesignInfoAfter = diA;
            DesignInfo.DesignInfoBefore = diB;
        }
    }
}
