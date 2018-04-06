using Autodesk.Revit.DB;
using AutoRebaring.Database;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class ElementInfoCollection
    {
        private List<IElementInfo> elemInfos;
        public int Count { get { return elemInfos.Count; } }
        public IEnumerator<IElementInfo> GetEnumerator() { return elemInfos.GetEnumerator(); }
        public void Sort()
        {
            elemInfos.Sort(new ElementInfoSorter());
        }
        public IElementInfo this[int i]
        {
            get { return elemInfos[i]; }
            set { elemInfos[i] = value; }
        }
        public ElementInfoCollection(Document doc, Element e, GeneralParameterInput gpi, IInputForm inputForm)
        {
            ElementTypeEnum elemType = (gpi.ElementType == "Column") ? ElementTypeEnum.Column : ElementTypeEnum.Wall;

            // F0
            GetRelatedElements(doc, e, gpi);

            // F1
            GetAllParameters(inputForm, gpi, elemType);
        }
        public void GetRelatedElements(Document doc, Element e, GeneralParameterInput gpi)
        {
            Polygon pl = new PlaneInfo(doc, e, gpi).Polygon;
            FilteredElementCollector col = new FilteredElementCollector(doc).WhereElementIsNotElementType();
            List<Element> elems = new List<Element>();
            foreach (Element eA in col)
            {
                if (eA == null) continue;
                if (eA is Wall || (eA is FamilyInstance && eA.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns))
                {
                    PlaneInfo pi = new PlaneInfo(doc, eA, gpi);
                    Polygon plA = pi.Polygon;
                    plA = CheckGeometry.GetProjectPolygon(pl.Plane, plA);
                    PolygonComparePolygonResult res = new PolygonComparePolygonResult(pl, plA);
                    if (res.IntersectType == PolygonComparePolygonIntersectType.AreaOverlap)
                    {
                        elemInfos.Add(
                            new ElementInfo()
                            {
                                RevitInfo = new RevitInfo(doc, eA)
                            });
                    }
                }
            }
            elemInfos.Sort(new ElementInfoSorter());
        }
        public void GetAllParameters(IInputForm inputForm, GeneralParameterInput gpi, ElementTypeEnum elemType)
        {
            // F1
            for (int i = 0; i < elemInfos.Count; i++)
            {
                // F1.1
                elemInfos[i].GetPlaneInfo(elemType, gpi);
                // F1.2
                elemInfos[i].GetDesignInfo(inputForm);
                // F1.3
                elemInfos[i].GetVerticalInfo(elemType, gpi);
                // F1.4
                elemInfos[i].GetStandardSpacing(gpi);
                // F1.5
                elemInfos[i].GetRebarLocation();
            }

            // F2
            for (int i = 0; i < elemInfos.Count; i++)
            {
                // F2.1 + F2.2
                if (i == 0)
                {
                    elemInfos[i].GetShortenType(elemInfos[i + 1].PlaneInfo);
                    elemInfos[i].GetDesignInfoAB(elemInfos[i + 1].DesignInfo, elemInfos[0].DesignInfo);
                }
                else if (i == elemInfos.Count - 1)
                {
                    elemInfos[i].GetShortenType(elemInfos[i].PlaneInfo);
                    elemInfos[i].GetDesignInfoAB(elemInfos[i].DesignInfo, elemInfos[i - 1].DesignInfo);
                }
                else
                {
                    elemInfos[i].GetShortenType(elemInfos[i + 1].PlaneInfo);
                    elemInfos[i].GetDesignInfoAB(elemInfos[i + 1].DesignInfo, elemInfos[i -1].DesignInfo);
                }

                // F2.3
                elemInfos[i].GetRebarInformation();
                // F2.4
                elemInfos[i].GetStandardPlaneInfo(elemType, gpi);
            }
        }
    }

    public class ElementInfoSorter : IComparer<IElementInfo>
    {
        public ElementInfoSorter() { }
        public int Compare(IElementInfo first, IElementInfo second)
        {
            return first.RevitInfo.Elevation.CompareTo(second.RevitInfo.Elevation);
        }
    }
    public enum ElementTypeEnum { Column, Wall}
}
