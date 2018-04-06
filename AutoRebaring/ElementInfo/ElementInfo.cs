using Autodesk.Revit.DB;
using AutoRebaring.Database;
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
        public void GetPlaneInfo(ElementTypeEnum elemType, GeneralParameterInput gpi)
        {
            switch (elemType)
            {
                case ElementTypeEnum.Column:
                    PlaneInfo = new ColumnPlaneInfo(RevitInfo, gpi);
                    break;
                case ElementTypeEnum.Wall:
                    PlaneInfo = new ColumnPlaneInfo(RevitInfo, gpi);
                    break;
            }
        }
        public void GetDesignInfo(IInputForm inputForm)
        {
            bool isDesignAssigned = false;
            for (int j = 0; j < inputForm.DesignInfos.Count; j++)
            {
                if (GeomUtil.IsSmaller(RevitInfo.Elevation, inputForm.DesignInfos[j].Level.Elevation))
                {
                    DesignInfo = inputForm.DesignInfos[j - 1];
                    isDesignAssigned = true;
                    break;
                }
                else if (GeomUtil.IsEqual(RevitInfo.Elevation, inputForm.DesignInfos[j].Level.Elevation))
                {
                    DesignInfo = inputForm.DesignInfos[j];
                    isDesignAssigned = true;
                    break;
                }
            }
            if (!isDesignAssigned)
            {
                DesignInfo = inputForm.DesignInfos[inputForm.DesignInfos.Count];
            }
        }
        public void GetVerticalInfo(ElementTypeEnum elemType, GeneralParameterInput gpi)
        {
            switch (elemType)
            {
                case ElementTypeEnum.Column:
                    VerticalInfo = new ColumnVerticalInfo(RevitInfo);
                    break;
                case ElementTypeEnum.Wall:
                    VerticalInfo = new WallVerticalInfo(RevitInfo);
                    break;
            }
        }
        public void GetStandardSpacing(GeneralParameterInput gpi)
        {
            DesignInfo.GetStandardSpacing(PlaneInfo, gpi);
        }
        public void GetRebarLocation()
        {
            PlaneInfo.GetRebarLocation(DesignInfo);
        }
        public void GetRebarInformation()
        {
            VerticalInfo.GetRebarInformation(DesignInfo);
        }
        public void GetStandardPlaneInfo(ElementTypeEnum elemType, GeneralParameterInput gpi)
        {
            switch (elemType)
            {
                case ElementTypeEnum.Column:
                    StandardPlaneInfo = new ColumnStandardPlaneInfo(PlaneInfo, DesignInfo, gpi);
                    break;
                case ElementTypeEnum.Wall:
                    StandardPlaneInfo = new WallStandardPlaneInfo(PlaneInfo, DesignInfo, gpi);
                    break;
            }
        }
        public void GetShortenType(IPlaneInfo planeInfo)
        {
            PlaneInfo.PlaneInfoAfter = planeInfo;
        }
        public void GetDesignInfoAB(IDesignInfo diA, IDesignInfo diB)
        {
            DesignInfo.DesignInfoAfter = diA;
            DesignInfo.DesignInfoBefore = diB;
        }
    }
}
