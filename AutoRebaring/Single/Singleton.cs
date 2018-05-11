using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo;
using AutoRebaring.RebarLogistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Single
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
        private List<IRevitInfo> revitInfos = new List<IRevitInfo>();
        private List<IPlaneInfo> planeInfos = new List<IPlaneInfo>();
        private List<IVerticalInfo> verticalInfos = new List<IVerticalInfo>();
        private List<IDesignInfo> designInfos = new List<IDesignInfo>();
        private List<IStandardPlaneInfo> standPlaneInfos = new List<IStandardPlaneInfo>();
        private List<IStirrupPlaneInfo> stirPlaneInfos = new List<IStirrupPlaneInfo>();
        private List<IStandardPlaneSingleInfo> standPlaneSingleInfos = new List<IStandardPlaneSingleInfo>();
        private List<List<StandardTurn>> standTurnsList = new List<List<StandardTurn>>();
        private List<int> standTurnsCount = new List<int>();
        private List<List<VariableImplant>> variableImplantsList = new List<List<VariableImplant>>();
        private List<List<StirrupDistribution>> stirDissList = new List<List<StirrupDistribution>>();
        private int elemTypeInfoID;
        public VariableStandard VariableStandard { get; set; }
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
        public List<RebarShape> StirrupShapes { get; set; }
        #endregion

        #region Add Data
        public void SetElementTypeInfoID(ElementTypeEnum elemTypeEnum){elemTypeInfoID = elemTypeInfos.First(x => x.Type == elemTypeEnum).ID;}
        public void AddElementTypeInfo(IElementTypeInfo elemTypeInfo) { elemTypeInfos.Add(elemTypeInfo); }
        public void AddRevitInfo(IRevitInfo revitInfo) { revitInfos.Add(revitInfo); }
        public void AddPlaneInfo(IPlaneInfo planeInfo) { planeInfos.Add(planeInfo); }
        public void AddVerticalInfo(IVerticalInfo verticalInfo) { verticalInfos.Add(verticalInfo); }
        public void AddDesignInfo(IDesignInfo designInfo) { designInfos.Add(designInfo); }
        public void AddStandardPlaneInfo(IStandardPlaneInfo standPlaneInfo) { standPlaneInfos.Add(standPlaneInfo); }
        public void AddStirrupPlaneInfo(IStirrupPlaneInfo stirPlaneInfo) { stirPlaneInfos.Add(stirPlaneInfo); }
        public void AddStandardPlaneSingleInfo(IStandardPlaneSingleInfo standPlaneSingleInfo) { standPlaneSingleInfos.Add(standPlaneSingleInfo); }
        public void AddStandardTurn(StandardTurn st)
        {
            if (standTurnsList.Count - 1 < st.LocationIndex)
            {
                standTurnsList.Add(new List<StandardTurn>());
            }
            standTurnsList[st.LocationIndex].Add(st);
        }
        public void AddStandardTurnCount(int locIndex, int count)
        {
            if (standTurnsCount.Count - 1 < locIndex)
            {
                standTurnsCount.Add(count);
            }
            standTurnsCount[locIndex] = count;
        }
        public void AddVariableImplant(VariableImplant vi)
        {
            if (variableImplantsList.Count - 1 < vi.LocationIndex)
            {
                variableImplantsList.Add(new List<VariableImplant>());
            }
            variableImplantsList[vi.LocationIndex].Add(vi);
        }
        public void AddStirrupDistribution(StirrupDistribution sd)
        {
            if (stirDissList.Count - 1 < sd.IDElement)
            {
                stirDissList.Add(new List<StirrupDistribution>());
            }
            stirDissList[sd.IDElement].Add(sd);
        }
        #endregion

        #region Update Data
        public void UpdateRevitInfo(int id, IRevitInfo revitInfo) { revitInfos[id] = revitInfo; }
        public void UpdatePlaneInfo(int id, IPlaneInfo planeInfo) { planeInfos[id] = planeInfo; }
        public void UpdateVerticalInfo(int id, IVerticalInfo verticalInfo) { verticalInfos[id] = verticalInfo; }
        public void UpdateDesignInfo(int id, IDesignInfo designInfo) { designInfos[id] = designInfo; }
        public void UpdateStandardPlaneInfo(int id, IStandardPlaneInfo standPlaneInfo) { standPlaneInfos[id] = standPlaneInfo; }
        public void UpdateStirrupPlaneInfo(int id, IStirrupPlaneInfo stirPlaneInfo) { stirPlaneInfos[id] = stirPlaneInfo; }
        public void UpdateStandardTurn(StandardTurn st) { standTurnsList[st.LocationIndex][st.ID] = st; }
        public void UpdateVaribleImplant(VariableImplant vi) { variableImplantsList[vi.LocationIndex][vi.ID] = vi; }
        public void UpdateStirrupDistributionList(List<StirrupDistribution> sds) { stirDissList[sds[0].IDElement] = sds; }
        public void UpdateStirrupDistribution(StirrupDistribution sd) { stirDissList[sd.IDElement][sd.ID] = sd; }
        #endregion

        #region Inquire Data
        public IElementTypeInfo GetElementTypeInfo() { return elemTypeInfos.First(x => x.ID == elemTypeInfoID); }
        public int GetElementTypeInfoID() { return elemTypeInfos.First(x => x.ID == elemTypeInfoID).ID; }
        public ElementTypeEnum GetElementTypeEnum() { return elemTypeInfos.First(x => x.ID == elemTypeInfoID).Type; }
        public int GetElementCount() { return revitInfos.Count; }
        public IRevitInfo GetRevitInfo(int id) { return revitInfos[id]; }
        public IPlaneInfo GetPlaneInfo(int id) { return planeInfos[id]; }
        public IPlaneInfo GetPlaneInfoAfter(int id)
        {
            int idAfter = id + 1 < planeInfos.Count ? id + 1 : planeInfos.Count - 1;
            return GetPlaneInfo(idAfter);
        }
        public IVerticalInfo GetVerticalInfo(int id) { return verticalInfos[id]; }
        public IVerticalInfo GetVerticalInfoAfter(int id)
        {
            int idAfter = id + 1 < designInfos.Count ? id + 1 : verticalInfos.Count - 1;
            return GetVerticalInfo(idAfter);
        }
        public IVerticalInfo GetVerticalInfo2After(int id)
        {
            int idAfter = id + 2 < designInfos.Count ? id + 2 : verticalInfos.Count - 1;
            return GetVerticalInfo(idAfter);
        }
        public IDesignInfo GetDesignInfo(int id) { return designInfos[id]; }
        public IDesignInfo GetDesignInfoAfter(int id)
        {
            int idAfter = id + 1 < designInfos.Count ? id + 1 : designInfos.Count - 1;
            return GetDesignInfo(idAfter);
        }
        public IStandardPlaneInfo GetStandardPlaneInfo(int id) { return standPlaneInfos[id]; }
        public IStirrupPlaneInfo GetStirrupPlaneInfo(int id) { return stirPlaneInfos[id]; }
        public IStandardPlaneSingleInfo GetStandardPlaneSingleInfo(int id) { return standPlaneSingleInfos[id]; }
        public StandardTurn GetStandardTurn(int id, int locIndex) { return standTurnsList[locIndex][id]; }
        public StandardTurn GetStandardTurnAfter(int id, int locIndex)
        {
            int idAfter = id + 1 < standTurnsList[locIndex].Count ? id + 1 : standTurnsList[locIndex].Count - 1;
            return GetStandardTurn(idAfter, locIndex);
        }
        public VariableImplant GetVariableImplant(int id, int locIndex) { return variableImplantsList[locIndex][id]; }
        public int GetStandardTurnCount(int locIndex)
        {
            return standTurnsCount[locIndex];
        }
        public int GetStirrupDistribuitionsCount (int idElem) { return stirDissList[idElem].Count; }
        public StirrupDistribution GetStirrupDistribution(int idElem, int id) { return stirDissList[idElem][id]; }
        public StirrupDistribution GetStirrupDistributionAfter(int idElem, int id)
        {
            int idAfter = id + 1 < stirDissList[idElem].Count ? id + 1 : stirDissList[idElem].Count - 1;
            return GetStirrupDistribution(idElem, id);
        }
        public RebarShape GetRebarShape(int id)
        {
            return StirrupShapes[id];
        }
        public int GetRebarShapeCount()
        {
            return StirrupShapes.Count;
        }
        #endregion
    }
}
