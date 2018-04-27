using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using AutoRebaring.Constant;
using AutoRebaring.Database;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRebaring.Single;
using AutoRebaring.RebarLogistic;

namespace AutoRebaring.ElementInfo
{
    public static class ElementInfoUtils
    {
        public static void AddElementTypeInfo()
        {
            var obj = new ElementTypeInfo(ElementTypeEnum.Column, 1);
            Singleton.Instance.AddElementTypeInfo(obj);

            obj = new ElementTypeInfo(ElementTypeEnum.Wall, 2);
            Singleton.Instance.AddElementTypeInfo(obj);
        }
        public static void PickElement(Document doc, Selection sel)
        {
            Element e = doc.GetElement(sel.PickObject(ObjectType.Element, new WallAndColumnSelection()));
            Singleton.Instance.Document = doc;
            Singleton.Instance.Element = e;
        }
        public static void GetRelatedElements()
        {
            List<IRevitInfo> revitInfos = new List<IRevitInfo>();

            Element e = Singleton.Instance.Element;
            Document doc = Singleton.Instance.Document;
            ARLevel startLevel = Singleton.Instance.StartLevel;
            ARLevel endLevel = Singleton.Instance.EndLevel;

            PlaneInfo pi = new PlaneInfo(doc, e);
            UV p1 = pi.CentralPoint - pi.VectorU * pi.B1 / 2 - pi.VectorV * pi.B2 / 2;
            UV p2 = pi.CentralPoint + pi.VectorU * pi.B1 / 2 + pi.VectorV * pi.B2 / 2;
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
                if (GeomUtil.IsEqualOrBigger(sLevel.Elevation, startEle) && GeomUtil.IsEqualOrSmaller(eLevel.Elevation, endEle))
                {
                    PlaneInfo piA = new PlaneInfo(doc, eA);
                    UV p1A = piA.CentralPoint - piA.VectorU * piA.B1 / 2 - piA.VectorV * piA.B2 / 2;
                    UV p2A = piA.CentralPoint + piA.VectorU * piA.B1 / 2 + piA.VectorV * piA.B2 / 2;
                    if (GeomUtil.IsEqualOrSmaller(p1.U, p2A.U) && GeomUtil.IsEqualOrSmaller(p1A.U, p2.U) && GeomUtil.IsEqualOrSmaller(p1.V, p2A.V) && GeomUtil.IsEqualOrSmaller(p1A.V, p2.V))
                    {
                        revitInfos.Add(new RevitInfo(doc, eA));
                    }
                }
            }

            revitInfos.Sort(new RevitInfoSorter());
            for (int i = 0; i < revitInfos.Count; i++)
            {
                revitInfos[i].ID = i;
                Singleton.Instance.AddRevitInfo(revitInfos[i]);
            }
        }
        public static void GetAllParameters()
        {
            ElementTypeEnum elemTypeEnum = Singleton.Instance.GetElementTypeEnum();

            // F1
            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                IPlaneInfo planeInfo = null;
                IDesignInfo designInfo = null;
                IVerticalInfo verticalInfo = null;
                IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(i);

                switch (elemTypeEnum)
                {
                    case ElementTypeEnum.Column:
                        planeInfo = new ColumnPlaneInfo(i);
                        verticalInfo = new ColumnVerticalInfo(i);
                        break;
                    case ElementTypeEnum.Wall:
                        planeInfo = new WallPlaneInfo(i);
                        verticalInfo = new WallVerticalInfo(i);
                        break;
                }

                bool isTop = true;
                for (int j = 0; j < Singleton.Instance.DesignInfos.Count; j++)
                {
                    IDesignInfo tempDesInfo = Singleton.Instance.DesignInfos[j];
                    if (GeomUtil.IsEqual(revitInfo.Elevation, tempDesInfo.Level.Elevation))
                    {
                        switch (elemTypeEnum)
                        {
                            case ElementTypeEnum.Column:
                                designInfo = new ColumnDesignInfo(tempDesInfo, revitInfo.Level);
                                break;
                            case ElementTypeEnum.Wall:
                                designInfo = new WallDesignInfo(tempDesInfo, revitInfo.Level);
                                break;
                        }
                        designInfo.ID = i;
                        isTop = false;
                        break;
                    }
                    else if (GeomUtil.IsSmaller(revitInfo.Elevation, tempDesInfo.Level.Elevation))
                    {
                        switch (elemTypeEnum)
                        {
                            case ElementTypeEnum.Column:
                                designInfo = new ColumnDesignInfo(Singleton.Instance.DesignInfos[j - 1], revitInfo.Level);
                                break;
                            case ElementTypeEnum.Wall:
                                designInfo = new WallDesignInfo(Singleton.Instance.DesignInfos[j - 1], revitInfo.Level);
                                break;
                        }
                        designInfo.ID = i;
                        isTop = false;
                        break;
                    }
                }
                if (isTop)
                {
                    switch (elemTypeEnum)
                    {
                        case ElementTypeEnum.Column:
                            designInfo = new ColumnDesignInfo(Singleton.Instance.DesignInfos[Singleton.Instance.DesignInfos.Count - 1], revitInfo.Level);
                            break;
                        case ElementTypeEnum.Wall:
                            designInfo = new WallDesignInfo(Singleton.Instance.DesignInfos[Singleton.Instance.DesignInfos.Count - 1], revitInfo.Level);
                            break;
                    }
                    designInfo.ID = i;
                }

                Singleton.Instance.AddPlaneInfo(planeInfo);
                Singleton.Instance.AddVerticalInfo(verticalInfo);
                Singleton.Instance.AddDesignInfo(designInfo);
            }

            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                IPlaneInfo planeInfo = Singleton.Instance.GetPlaneInfo(i);
                IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(i);
                IVerticalInfo verticalInfo = Singleton.Instance.GetVerticalInfo(i);

                designInfo.GetStandardSpacing();
                planeInfo.GetRebarLocation();
                planeInfo.GetShortenType();
                verticalInfo.GetRebarInformation();

                Singleton.Instance.UpdatePlaneInfo(i, planeInfo);
                Singleton.Instance.UpdateDesignInfo(i, designInfo);
                Singleton.Instance.UpdateVerticalInfo(i, verticalInfo);
            }

            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                IStandardPlaneInfo standPlaneInfo = null;
                switch (elemTypeEnum)
                {
                    case ElementTypeEnum.Column:
                        standPlaneInfo = new ColumnStandardPlaneInfo(i);
                        break;
                    case ElementTypeEnum.Wall:
                        standPlaneInfo = new WallStandardPlaneInfo(i);
                        break;
                }
                Singleton.Instance.AddStandardPlaneInfo(standPlaneInfo);
            }
        }
        public static void GetVariable()
        {
            VariableStandard vs = new VariableStandard();
            Singleton.Instance.VariableStandard = vs;

            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                for (int j = 0; j < Singleton.Instance.GetElementTypeInfo().LocationCount; j++)
                {
                    VariableImplant vi = new VariableImplant(i, j);
                    Singleton.Instance.AddVariableImplant(vi);
                }

            }
        }
    }

    public class RevitInfoSorter : IComparer<IRevitInfo>
    {
        public RevitInfoSorter() { }
        public int Compare(IRevitInfo first, IRevitInfo second)
        {
            return first.Elevation.CompareTo(second.Elevation);
        }
    }
}
