using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.Constant;
using AutoRebaring.Single;

namespace AutoRebaring.ElementInfo.RebarInfo.StirrupInfo.SingleInfo
{
    public class StirrupID
    {
        public static int ID = 0;
    }
    public class StirrupPlaneSingleInfo : IStirrupPlaneSingleInfo
    {
        public int ID { get; set; }
        public int IDStirrupShape { get; set; }
        public int IDStirrupType
        {
            get
            {
                switch (Singleton.Instance.GetElementTypeEnum())
                {
                    case ElementTypeEnum.Column:
                        return IDStirrupShape;
                    case ElementTypeEnum.Wall:
                        return IDStirrupType == 2 ? 1 : 0;
                }
                throw new Exception();
            }
        }
        public List<string> ParameterKeys
        {
            get
            {
                switch (IDStirrupType)
                {
                    case 0:
                        return ConstantValue.CoverStirrupParameters;
                    case 1:
                        return ConstantValue.CStirrupParameters;
                }
                throw new Exception();
            }
        }
        public UV StartPoint { get; set; }
        public XYZ VectorX { get; set; }
        public XYZ VectorY { get; set; }
        public List<double> ParameterValues { get; set; }
        public StirrupTypeEnum StirrupType { get; set; }

        public StirrupPlaneSingleInfo()
        {
            ID = StirrupID.ID;
            StirrupID.ID++;
        }
        public Rebar CreateRebars(int idElem, int idStirDis)
        {
            IDesignInfo designInfo = Singleton.Instance.GetDesignInfo(idElem);
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(idElem);
            StirrupDistribution sd = Singleton.Instance.GetStirrupDistribution(idElem, idStirDis);
            RebarShape rs = Singleton.Instance.GetRebarShape(IDStirrupShape);

            Document doc = Singleton.Instance.Document;
            Element elem = revitInfo.Element;
            RebarBarType rbt = designInfo.StirrupTypes[IDStirrupShape];
            double tbSpa = designInfo.BotTopSpacings[IDStirrupType];
            double mSpa = designInfo.MiddleSpacings[IDStirrupType];
            double stirDia = designInfo.StirrupDiameters[IDStirrupShape];

            Rebar rb = null;
            XYZ startPoint = null;
            switch (sd.StirrupLocation)
            {
                case StirrupLocation.Middle:
                case StirrupLocation.Top:
                    try
                    {
                        rb = CreateSingleRebar(idElem, doc, elem, rs, rbt, sd.StartZ1s[IDStirrupType], sd.EndZ1s[IDStirrupType], sd.Number1s[IDStirrupType], mSpa, stirDia, sd.StirrupLocation);
                    }
                    catch
                    {
                        throw;
                    }
                    break;
            }
            rb = CreateSingleRebar(idElem, doc, elem, rs, rbt, sd.StartZ2s[IDStirrupType], sd.EndZ2s[IDStirrupType], sd.Number2s[IDStirrupType], tbSpa, stirDia, sd.StirrupLocation);
            return rb;
        }
        public Rebar CreateSingleRebar(int idElem, Document doc, Element elem, RebarShape rs, RebarBarType rbt, double start, double end, int num, double spa, double stirDia, StirrupLocation stirLoc)
        {
            XYZ startPoint = new XYZ(StartPoint.U, StartPoint.V, start);
            Rebar rb = Rebar.CreateFromRebarShape(doc, rs, rbt, elem, startPoint, VectorX, VectorY);
            RebarShapeDrivenAccessor rsda = rb.GetShapeDrivenAccessor();
            IRevitInfo revitInfo = Singleton.Instance.GetRevitInfo(idElem);

            for (int i = 0; i < ParameterKeys.Count; i++)
            {
                    rb.LookupParameter(ParameterKeys[i]).Set(ParameterValues[i]);
            }
            if (num == 1)
            {
                rsda.SetLayoutAsSingle();
            }
            else
            {
                rsda.SetLayoutAsNumberWithSpacing(num, spa, true, true, true);
            }
            doc.Regenerate();
            BoundingBoxXYZ bb = rb.get_BoundingBox(null);
            XYZ midPnt = new XYZ((bb.Min.X + bb.Max.X) / 2, (bb.Min.Y + bb.Max.Y) / 2, (bb.Min.Z + bb.Max.Z) / 2);
            switch (IDStirrupType)
            {
                case 0:
                    //ElementTransformUtils.MoveElement(doc, rb.Id, new XYZ(ri.TopUVStirrup1.U - midPnt.X - ri.StirrupDiameter1 / 4, ri.TopUVStirrup1.V - midPnt.Y - ri.StirrupDiameter1 / 4, (endZ + startZ) / 2 - midPnt.Z + ri.StirrupDiameter1 / 2));
                    ElementTransformUtils.MoveElement(doc, rb.Id, new XYZ(startPoint.X - midPnt.X - stirDia / 4, startPoint.Y - midPnt.Y - stirDia / 4, (end + start) / 2 - midPnt.Z + stirDia / 2));
                    break;
                case 1:
                    //ElementTransformUtils.MoveElement(doc, rb.Id, new XYZ(topUVstir.U - midPnt.X, topUVstir.V - midPnt.Y, (endZ + startZ) / 2 - midPnt.Z));
                    ElementTransformUtils.MoveElement(doc, rb.Id, new XYZ(startPoint.X - midPnt.X, startPoint.Y - midPnt.Y, (end + start) / 2 - midPnt.Z));
                    break;
            }

            switch (stirLoc)
            {
                case StirrupLocation.Bottom:
                case StirrupLocation.Top:
                    rb.LookupParameter("Location").Set("BotTop");
                    break;
                case StirrupLocation.Middle:
                    rb.LookupParameter("Location").Set("Middle");
                    break;
            }
            rb.LookupParameter("Comments").Set("add-in");
            rb.LookupParameter("Level").Set(revitInfo.TitleLevel);
            rb.LookupParameter("SLCauKien").Set(Singleton.Instance.OtherParameter.PartCount);
            rb.LookupParameter("SoLuong").Set(Singleton.Instance.GetStirrupLevelCount(idElem, this).Count);

            List<View3D> view3ds = ConstantValue.View3dIDIntergers.Select(x => Singleton.Instance.Document.GetElement(new ElementId(x))).Cast<View3D>().ToList();
            List<View> views = ConstantValue.ViewIDIntergers.Select(x => Singleton.Instance.Document.GetElement(new ElementId(x))).Cast<View>().ToList();
            view3ds.ForEach(x => ElementInfoUtils.SetupView3d(x, rb));
            views.ForEach(x => rb.SetUnobscuredInView(x, true));

            return rb;
        }
    }
}
