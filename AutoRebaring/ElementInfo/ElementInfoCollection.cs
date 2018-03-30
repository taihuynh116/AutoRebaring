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
        public ElementInfoCollection(Document doc, Element e, GeneralParameterInput gpi)
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
    }

    public class ElementInfoSorter : IComparer<IElementInfo>
    {
        public ElementInfoSorter() { }
        public int Compare(IElementInfo first, IElementInfo second)
        {
            return first.RevitInfo.Elevation.CompareTo(second.RevitInfo.Elevation);
        }
    }
}
