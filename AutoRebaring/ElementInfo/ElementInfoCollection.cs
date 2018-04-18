using Autodesk.Revit.DB;
using AutoRebaring.Constant;
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
        public ElementInfoCollection(IInputForm form) : this(form.Document, form.Element, form.StartLevel, form.EndLevel, form.ElementType, form.WallParameter, form.CoverParameter, form.AnchorParameter, form.DevelopmentParameter, form.LockheadParameter, form.DesignInfos)
        {
        }
        public ElementInfoCollection(Document doc, Element e, ARLevel startLevel, ARLevel endLevel, ARElementType elemType, ARWallParameter wp, ARCoverParameter cp, ARAnchorParameter ap, ARDevelopmentParameter dp, ARLockheadParameter lp, List<IDesignInfo> designInfos)
        {
            // F0
            GetRelatedElements(doc, e, startLevel, endLevel);

            // F1
            GetAllParameters(elemType, wp, cp, ap, dp, lp, designInfos);
        }
        public void GetRelatedElements(Document doc, Element e, ARLevel startLevel, ARLevel endLevel)
        {
            PlaneInfo pi = new PlaneInfo(doc, e);
            UV p1 = pi.CentralPoint - pi.VectorU * pi.B1 / 2 - pi.VectorV * pi.B2 / 2;
            UV p2 = pi.CentralPoint - pi.VectorU * pi.B1 / 2 - pi.VectorV * pi.B2 / 2;
            List<Element> elemCols = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x != null).Where(x => (x is Wall) || (x is FamilyInstance && x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns)).ToList();
            List<Element> elems = new List<Element>();
            foreach (Element eA in elemCols)
            {
                Level sLevel = null, eLevel = null;
                if (eA is Wall)
                {
                    sLevel = doc.GetElement(eA.LookupParameter(ConstantValue.StartLevelWall).AsElementId()) as Level;
                    eLevel = doc.GetElement(eA.LookupParameter(ConstantValue.EndLevelWall).AsElementId()) as Level;
                }
                else
                {
                    sLevel = doc.GetElement(eA.LookupParameter(ConstantValue.StartLevelColumn).AsElementId()) as Level;
                    eLevel = doc.GetElement(eA.LookupParameter(ConstantValue.EndLevelColumn).AsElementId()) as Level;
                }
                string startLvl = startLevel.Name, endLvl = endLevel.Name;
                double startEle = startLevel.Elevation * ConstantValue.milimeter2Feet, endEle = endLevel.Elevation * ConstantValue.milimeter2Feet;
                string sLvl = sLevel.Name, eLvl = eLevel.Name;
                double sEle = sLevel.Elevation, eEle = eLevel.Elevation;
                if (GeomUtil.IsEqualOrBigger(sLevel.Elevation, startEle))
                {
                    if (GeomUtil.IsEqualOrSmaller(eLevel.Elevation, endEle))
                    {
                        PlaneInfo piA = new PlaneInfo(doc, eA);
                        UV p1A = piA.CentralPoint - piA.VectorU * piA.B1 / 2 - piA.VectorV * piA.B2 / 2;
                        UV p2A = piA.CentralPoint - piA.VectorU * piA.B1 / 2 - piA.VectorV * piA.B2 / 2;
                        if (GeomUtil.IsEqualOrSmaller(p1.U, p2A.U) && GeomUtil.IsEqualOrSmaller(p1A.U, p2.U) && GeomUtil.IsEqualOrSmaller(p1.V, p2A.V) && GeomUtil.IsEqualOrSmaller(p1A.V, p2.V))
                        {
                            elemInfos.Add(
                                new ElementInfo()
                                {
                                    RevitInfo = new RevitInfo(doc, eA)
                                });
                        }
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
                elemInfos[i].Index = i;
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
                    elemInfos[i].GetShortenType(elemInfos[i + 1].PlaneInfo, lp);
                    elemInfos[i].GetDesignInfoAB(elemInfos[i + 1].DesignInfo, elemInfos[0].DesignInfo);
                }
                else if (i == elemInfos.Count - 1)
                {
                    elemInfos[i].GetShortenType(elemInfos[i].PlaneInfo, lp);
                    elemInfos[i].GetDesignInfoAB(elemInfos[i].DesignInfo, elemInfos[i - 1].DesignInfo);
                }
                else
                {
                    elemInfos[i].GetShortenType(elemInfos[i + 1].PlaneInfo, lp);
                    elemInfos[i].GetDesignInfoAB(elemInfos[i + 1].DesignInfo, elemInfos[i - 1].DesignInfo);
                }

                // F2.3
                elemInfos[i].GetRebarInformation(ap, dp);

                // F2.4
                elemInfos[i].GetStandardPlaneInfo(elemType, lp);
            }

            // F3
            for (int i = 0; i < elemInfos.Count; i++)
            {
                elemInfos[i].VerticalInfo.EndLimit0s = elemInfos[i].VerticalInfo.RebarDevelopmentLengths.Select(x => elemInfos[i].VerticalInfo.TopOffset).ToList();
                if (i < elemInfos.Count - 1)
                {
                    elemInfos[i].VerticalInfo.StartLimit1s = elemInfos[i + 1].VerticalInfo.RebarDevelopmentLengths.Select(x => elemInfos[i + 1].VerticalInfo.BottomOffset + x).ToList();
                    elemInfos[i].VerticalInfo.EndLimit1s = elemInfos[i + 1].VerticalInfo.RebarDevelopmentLengths.Select(x => elemInfos[i + 1].VerticalInfo.TopOffset).ToList();
                }
                else
                {
                    elemInfos[i].VerticalInfo.StartLimit1s = elemInfos[i].VerticalInfo.EndLimit0s.Select(x => x + GeomUtil.milimeter2Feet(20000)).ToList();
                    elemInfos[i].VerticalInfo.EndLimit1s = elemInfos[i].VerticalInfo.StartLimit1s.Select(x => x + GeomUtil.milimeter2Feet(5000)).ToList();
                }
                if (i < elemInfos.Count - 2)
                {
                    elemInfos[i].VerticalInfo.StartLimit2s = elemInfos[i + 2].VerticalInfo.RebarDevelopmentLengths.Select(x => elemInfos[i + 2].VerticalInfo.BottomOffset + x).ToList();
                    elemInfos[i].VerticalInfo.EndLimit2s = elemInfos[i + 2].VerticalInfo.RebarDevelopmentLengths.Select(x => elemInfos[i + 2].VerticalInfo.TopOffset).ToList();
                }
                else
                {
                    elemInfos[i].VerticalInfo.StartLimit2s = elemInfos[i].VerticalInfo.EndLimit1s.Select(x => x + GeomUtil.milimeter2Feet(20000)).ToList();
                    elemInfos[i].VerticalInfo.EndLimit2s = elemInfos[i].VerticalInfo.StartLimit2s.Select(x => x + GeomUtil.milimeter2Feet(5000)).ToList();
                }
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
    public enum ElementTypeEnum { Column, Wall }
}
