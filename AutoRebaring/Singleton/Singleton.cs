using Autodesk.Revit.DB;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Singleton
{
    public class Singleton
    {
        private static Singleton instance;
        private Singleton() { }
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }

        #region Data
        private List<IElementTypeInfo> elemTypeInfos = new List<IElementTypeInfo>();
        private List<IElementInfo> elemInfos = new List<IElementInfo>();
        private List<IRevitInfo> revitInfos = new List<IRevitInfo>();
        private List<IPlaneInfo> planeInfos = new List<IPlaneInfo>();
        private List<IVerticalInfo> verticalInfos = new List<IVerticalInfo>();
        private List<IDesignInfo> designInfos = new List<IDesignInfo>();
        private List<IStandardPlaneInfo> standPlaneInfos = new List<IStandardPlaneInfo>();
        private List<IStandardPlaneSingleInfo> standPlaneSingleInfos = new List<IStandardPlaneSingleInfo>();
        private int elemTypeInfoID;
        public ARWallParameter WallParameter { get; set; }
        public ARCoverParameter CoverParameter { get; set; }
        public ARAnchorParameter AnchorParameter { get; set; }
        public ARDevelopmentParameter DevelopmentParameter { get; set; }
        public ARLockheadParameter LockheadParameter { get; set; }
        public List<IDesignInfo> DesignInfos { get; set; }
        public List<double> FitStandards { get; set; }
        public List<double> PairFitStandards { get; set; }
        public List<double> TripFitStandards { get; set; }
        public List<double> FitImplants { get; set; }
        public List<double> PairFitImplants { get; set; }
        public ARLevel StartLevel { get; set; }
        public ARLevel EndLevel { get; set; }
        public ARStandardChosen StandardChosen { get; set; }
        public List<double> RebarZ1s { get; set; }
        public List<double> RebarZ2s { get; set; }
        public Document Document { get; set; }
        public Element Element { get; set; }

        public ARRebarVerticalParameter StandardVeticalParameter { get; set; }
        public ARRebarVerticalParameter StirrupVerticalParameter { get; set; }
        #endregion

        #region Add Data
        public void SetElementTypeInfoID(ElementTypeEnum elemTypeEnum)
        {
            elemTypeInfoID = elemTypeInfos.First(x => x.Type == elemTypeEnum).ID;
        }
        public void AddElementTypeInfo(IElementTypeInfo elemTypeInfo) { elemTypeInfos.Equals(elemTypeInfo); }
        public void AddElementInfo(IElementInfo elemInfo) { elemInfos.Add(elemInfo); }
        public void AddRevitInfo(IRevitInfo revitInfo) { revitInfos.Add(revitInfo); }
        public void AddPlaneInfo(IPlaneInfo planeInfo) { planeInfos.Add(planeInfo); }
        public void AddVerticalInfo(IVerticalInfo verticalInfo) { verticalInfos.Add(verticalInfo); }
        public void AddDesignInfo(IDesignInfo designInfo) { designInfos.Add(designInfo); }
        public void AddStandardPlaneInfo(IStandardPlaneInfo standPlaneInfo) { standPlaneInfos.Add(standPlaneInfo); }
        public void AddStandardPlaneSingleInfo(IStandardPlaneSingleInfo standPlaneSingleInfo) { standPlaneSingleInfos.Add(standPlaneSingleInfo); }
        #endregion

        #region Inquire Data
        public IElementTypeInfo GetElementTypeInfo() { return elemTypeInfos.First(x => x.ID == elemTypeInfoID); }
        public int GetElementTypeInfoID() { return elemTypeInfos.First(x => x.ID == elemTypeInfoID).ID; }
        public ElementTypeEnum GetElementTypeEnum() { return elemTypeInfos.First(x => x.ID == elemTypeInfoID).Type; }
        public int GetElementCount() { return revitInfos.Count; }
        public IElementInfo GetElementInfo(int id) { return elemInfos.First(x => x.ID == id); }
        public IRevitInfo GetRevitInfo(int id) { return revitInfos.First(x => x.ID == id); }
        public IPlaneInfo GetPlaneInfo(int id) { return planeInfos.First(x => x.ID == id); }
        public IPlaneInfo GetPlaneInfoAfter(int id)
        {
            int idAfter = id+1 < planeInfos.Count ? id+1 : planeInfos.Count - 1;
            return GetPlaneInfo(idAfter);
        }
        public IVerticalInfo GetVerticalInfo(int id) { return verticalInfos.First(x => x.ID == id); }
        public IDesignInfo GetDesignInfo (int id) { return designInfos.First(x => x.ID == id); }
        public IDesignInfo GetDesignInfoAfter(int id)
        {
            int idAfter = id + 1 < designInfos.Count ? id + 1 : designInfos.Count - 1;
            return GetDesignInfo(idAfter);
        }
        public IStandardPlaneInfo GetStandardPlaneInfo(int id) { return standPlaneInfos.First(x => x.ID == id); }
        public IStandardPlaneSingleInfo GetStandardPlaneSingleInfo(int id) { return standPlaneSingleInfos.First(x => x.ID == id); }
        #endregion
    }
}
