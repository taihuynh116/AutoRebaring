using Autodesk.Revit.DB;
using AutoRebaring.Database;
using AutoRebaring.Database.AutoRebaring.EF;
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
        private List<IElementInfo> elemInfos = new List<IElementInfo>();
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
        public ElementInfoCollection(IInputForm form):this(form.Document, form.Element, form.ElementType, form.WallParameter, form.CoverParameter, form.AnchorParameter, form.DevelopmentParameter, form.LockheadParameter, form.DesignInfos)
        {
        }
        public ElementInfoCollection(Document doc, Element e, ARElementType elemType, ARWallParameter wp, ARCoverParameter cp, ARAnchorParameter ap, ARDevelopmentParameter dp, ARLockheadParameter lp, List<IDesignInfo> designInfos)
        {
            // F0
            GetRelatedElements(doc, e);

            // F1
            GetAllParameters(elemType, wp, cp, ap, dp,lp, designInfos);
        }
        public void GetRelatedElements(Document doc, Element e)
        {
            Polygon pl = new PlaneInfo(doc, e).Polygon;
            FilteredElementCollector col = new FilteredElementCollector(doc).WhereElementIsNotElementType();
            List<Element> elems = new List<Element>();
            foreach (Element eA in col)
            {
                if (eA == null) continue;
                if (eA is Wall || (eA is FamilyInstance && eA.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns))
                {
                    PlaneInfo pi = new PlaneInfo(doc, e);
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
        public void GetAllParameters(ARElementType elemType, ARWallParameter wp, ARCoverParameter cp, ARAnchorParameter ap, ARDevelopmentParameter dp, ARLockheadParameter lp, List<IDesignInfo> designInfos)
        {
            // F1
            for (int i = 0; i < elemInfos.Count; i++)
            {
                // F1.1
                elemInfos[i].GetPlaneInfo(elemType, wp);
                // F1.2
                elemInfos[i].GetDesignInfo(designInfos);
                // F1.3
                elemInfos[i].GetVerticalInfo(elemType);
                // F1.4
                elemInfos[i].GetStandardSpacing(cp);
                // F1.5
                elemInfos[i].GetRebarLocation(cp);
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
                elemInfos[i].GetRebarInformation(ap, dp);
                // F2.4
                elemInfos[i].GetStandardPlaneInfo(elemType,lp);
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
