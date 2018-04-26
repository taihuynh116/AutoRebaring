using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo;
using AutoRebaring.Single;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.RebarLogistic
{
    public class StandardTurn
    {
        public StandardTurn()
        {
        }
        public StandardTurn(StandardTurn turn)
        {
            ID = turn.ID;
            LocationIndex = turn.LocationIndex;
            IDElement = turn.IDElement;
            IDVariable = turn.IDVariable;
            Swap = turn.Swap;
            Start1 = turn.Start1;
            Start2 = turn.Start2;

            Implant1 = turn.Implant1;
            Implant2 = turn.Implant2;
            EqualZero1 = turn.EqualZero1;
            EqualZero2 = turn.EqualZero2;
            IsImplanted = turn.IsImplanted;
            FirstPass = turn.FirstPass;
        }
        public int ID { get; set; }
        public int LocationIndex { get; set; }
        public int IDElement { get; set; }
        public bool Swap { get; set; } = false;
        public bool CanSwap
        {
            get
            {
                if (EqualZero1 || EqualZero2 || Implant1 || Implant2) return false;
                if (GeomUtil.IsEqual(L1, L2)) return false;
                return true;
            }
        }
        public int IDVariable { get; set; } = 0;
        public int VariableCount
        {
            get
            {
                if (Implant1 && Implant2) return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).CountImplant12;
                if ((Implant1 && EqualZero2) || (Implant2 && EqualZero1)) return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).CountImplantEqualZero;
                if (Implant1 || Implant2)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit:
                            return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).CountStandardImplant;
                        case TurnChosenType.Residual:
                            return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).CountResidualImplant;
                    }
                }
                if (EqualZero1 || EqualZero2)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit:
                            return Singleton.Instance.VariableStandard.CountStandardEqualZero;
                        case TurnChosenType.Residual:
                            return Singleton.Instance.VariableStandard.CountResidualEqualZero;
                    }
                }
                switch (ChosenType)
                {
                    case TurnChosenType.Fit:
                        return Singleton.Instance.VariableStandard.CountStandard12;
                    case TurnChosenType.PairFit:
                        return Singleton.Instance.VariableStandard.CountStandard12;
                    case TurnChosenType.TripFit:
                        return Singleton.Instance.VariableStandard.CountStandard12;
                    case TurnChosenType.Residual:
                        return Singleton.Instance.VariableStandard.CountResidual12;
                }
                throw new Exception("This case have not benn checked yet!");
            }
        }
        public double Start1 { get; set; }
        public double Start2 { get; set; }
        public double End1
        {
            get { return Start1 + L1; }
        }
        public double End2
        {
            get { return Start2 + L2; }
        }
        public double L1
        {
            get
            {
                if (Finish1)
                    return l1Finish;
                if (EqualZero1)
                    return 0;
                if (Implant1)
                {
                    double l = 0;
                    if (Implant2)
                        l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L1Implants[IDVariable];
                    else if (EqualZero2)
                        l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).LImplants[IDVariable];
                    else switch (ChosenType)
                        {
                            case TurnChosenType.Fit: l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L1ImplantStandards[IDVariable]; break;
                            case TurnChosenType.Residual: l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L1ImplantResiduals[IDVariable]; break;
                        }
                    return (Singleton.Instance.GetVerticalInfo(IDElement).TopAnchorAfters[LocationIndex] + l) - Start1;
                }
                if (Implant2)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit: return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L1StandardImplants[IDVariable];
                        case TurnChosenType.Residual: return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L1ResidualImplants[IDVariable];
                    }
                }
                if (EqualZero2)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit:
                            return Singleton.Instance.VariableStandard.LStandards[IDVariable];
                        case TurnChosenType.Residual:
                            return Singleton.Instance.VariableStandard.LResiduals[IDVariable];
                    }
                }
                if (ChosenType == TurnChosenType.Fit)
                {
                    if (!Swap)
                    {
                        return Singleton.Instance.VariableStandard.L1Standards[IDVariable];
                    }
                    return Singleton.Instance.VariableStandard.L2Standards[IDVariable];
                }
                else
                {
                    if (!Swap)
                    {
                        return Singleton.Instance.VariableStandard.L1Residuals[IDVariable];
                    }
                    return Singleton.Instance.VariableStandard.L2Residuals[IDVariable];
                }
            }
        }
        public double L2
        {
            get
            {
                if (Finish2)
                    return l2Finish;
                if (EqualZero2)
                    return 0;
                if (Implant2)
                {
                    double l = 0;
                    if (Implant1)
                        l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L2Implants[IDVariable];
                    else if (EqualZero1)
                        l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).LImplants[IDVariable];
                    else switch (ChosenType)
                        {
                            case TurnChosenType.Fit: l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L2StandardImplants[IDVariable]; break;
                            case TurnChosenType.Residual: l = Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L2ResidualImplants[IDVariable]; break;
                        }
                    return (Singleton.Instance.GetVerticalInfo(IDElement).TopAnchorAfters[LocationIndex] + l) - Start2;
                }
                if (Implant1)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit: return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L2ImplantStandards[IDVariable];
                        case TurnChosenType.Residual: return Singleton.Instance.GetVariableImplant(IDElement, LocationIndex).L2ImplantResiduals[IDVariable];
                    }
                }
                if (EqualZero1)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit:
                            return Singleton.Instance.VariableStandard.LStandards[IDVariable];
                        case TurnChosenType.Residual:
                            return Singleton.Instance.VariableStandard.LResiduals[IDVariable];
                    }
                }
                if (ChosenType == TurnChosenType.Fit)
                {
                    if (!Swap)
                    {
                        return Singleton.Instance.VariableStandard.L2Standards[IDVariable];
                    }
                    return Singleton.Instance.VariableStandard.L1Standards[IDVariable];
                }
                else
                {
                    if (!Swap)
                    {
                        return Singleton.Instance.VariableStandard.L2Residuals[IDVariable];
                    }
                    return Singleton.Instance.VariableStandard.L1Residuals[IDVariable];
                }
            }
        }
        public double L1mm { get { return L1 * ConstantValue.feet2MiliMeter; } }
        public double L2mm { get { return L2 * ConstantValue.feet2MiliMeter; } }
        public bool Finish1
        {
            get
            {
                if (IDElement < Singleton.Instance.GetElementCount() - 1) return false;
                return checkFinish(Start1);
            }
        }
        public bool Finish2
        {
            get
            {
                if (IDElement < Singleton.Instance.GetElementCount() - 1) return false;
                return checkFinish(Start2);
            }
        }
        public bool Implant1 { get; set; } = false;
        public bool Implant2 { get; set; } = false;
        public double l1Finish
        {
            get
            {
                if (!Finish1) return 0;
                return setLFinish(Start1);
            }
        }
        public double l2Finish
        {
            get
            {
                if (!Finish2) return 0;
                return setLFinish(Start2);
            }
        }
        public bool EqualZero1 { get; set; } = false;
        public bool EqualZero2 { get; set; } = false;
        public bool IsImplanted { get; set; } = false;
        public Position Position1
        {
            get
            {
                IVerticalInfo verInfo = Singleton.Instance.GetVerticalInfo(IDElement);
                if (Finish1)
                {
                    return Position.Position2;
                }
                return checkPosition(End1);
            }
        }
        public Position Position2
        {
            get
            {
                IVerticalInfo verInfo = Singleton.Instance.GetVerticalInfo(IDElement);
                if (Finish2)
                {
                    return Position.Position2;
                }
                return checkPosition(End2);
            }
        }
        public int IDNextElement
        {
            get
            {
                if (Position1 == Position.Wrong || Position2 == Position.Wrong) return IDElement;
                int idnext = IDElement+ Math.Min((int)Position1, (int)Position2);
                return idnext < Singleton.Instance.GetElementCount() ? idnext : Singleton.Instance.GetElementCount() - 1;
            }
        }
        public double Delta
        {
            get
            {
                return Math.Abs(End1 - End2) - Singleton.Instance.GetVerticalInfo(IDNextElement).RebarDevelopmentLengths[LocationIndex];
            }
        }
        public bool IsValidate
        {
            get
            {
                if (Position1 == Position.Wrong || Position2 == Position.Wrong) return false;
                if (Finish1 || Finish2) return true;
                if (GeomUtil.IsSmaller(Delta, 0)) return false;
                return true;
            }
        }
        public bool FirstPass { get; set; } = false;
        public TurnChosenType ChosenType { get; set; }

        public double setLFinish(double start)
        {
            IVerticalInfo verInfo = Singleton.Instance.GetVerticalInfo(IDElement);
            double l = verInfo.TopLockHead - start;
            return ConstantValue.milimeter2Feet * Math.Round(l * ConstantValue.feet2MiliMeter);
        }
        public bool checkFinish(double start)
        {
            IVerticalInfo verInfo = Singleton.Instance.GetVerticalInfo(IDElement);
            double topLimit = verInfo.TopLockHead;
            double lmax = Singleton.Instance.StandardChosen.Lmax * ConstantValue.milimeter2Feet;
            return GeomUtil.IsEqualOrBigger(start + lmax, topLimit);
        }
        public void SetImplant1()
        {
            setImplant(Start1, End1, Implant1);
        }
        public void SetImplant2()
        {
            setImplant(Start2, End2, Implant2);
        }
        public void setImplant(double start, double end, bool type)
        {
            //setImplantChoice();
            //if (checkManual(start, end))
            //{
            //    type = true;
            //}
        }
        public void setImplantChoice()
        {
            if (!IsImplanted)
            {
                IsImplanted = true;
            }
        }
        public Position checkPosition(double end)
        {
            IVerticalInfo verInfo = Singleton.Instance.GetVerticalInfo(IDElement);
            IVerticalInfo verInfoAfter = Singleton.Instance.GetVerticalInfoAfter(IDElement);
            IVerticalInfo verInfo2Afer = Singleton.Instance.GetVerticalInfo2After(IDElement);

            double endLim0 = verInfo.TopOffset;
            double startLim1 = -1, startLim2 = -1, endLim1 = -1, endLim2 = -1;
            if (IDElement + 1 < Singleton.Instance.GetElementCount())
            {
                startLim1 = verInfoAfter.BottomOffset + verInfoAfter.RebarDevelopmentLengths[LocationIndex];
                endLim1 = verInfoAfter.TopOffset;
            }
            else
            {
                startLim1 = endLim0 + 20000 * ConstantValue.milimeter2Feet;
                endLim1 = startLim1 + 5000 * ConstantValue.milimeter2Feet;
            }
            if (IDElement + 2 < Singleton.Instance.GetElementCount())
            {
                startLim2 = verInfo2Afer.BottomOffset + verInfo2Afer.RebarDevelopmentLengths[LocationIndex];
                endLim2 = verInfo2Afer.TopOffset;
            }
            else
            {
                startLim2 = endLim1 + 20000 * ConstantValue.milimeter2Feet;
                endLim2 = startLim2 + 5000 * ConstantValue.milimeter2Feet;
            }

            if (GeomUtil.IsEqualOrSmaller(end, endLim0))
                return Position.Wrong;
            if (GeomUtil.IsBigger(end, endLim0) && GeomUtil.IsSmaller(end, startLim1))
                return Position.Wrong;
            if (GeomUtil.IsEqualOrBigger(end, startLim1) && GeomUtil.IsEqualOrSmaller(end, endLim1))
                return Position.Position1;
            if (GeomUtil.IsBigger(end, endLim1) && GeomUtil.IsSmaller(end, startLim2))
                return Position.Wrong;
            if (GeomUtil.IsEqualOrBigger(end, startLim2) && GeomUtil.IsEqualOrSmaller(end, endLim2))
                return Position.Position2;
            return Position.Wrong;
        }
        public void HandleNextTurn()
        {
            if (!IsValidate) return;
            StandardTurn nextTurn = Singleton.Instance.GetStandardTurn(ID + 1, LocationIndex);
            bool pos1Bigger = (int)Position1 > (int)Position2;
            bool pos2Bigger = (int)Position2 > (int)Position1;
            nextTurn.IDElement = IDNextElement;
            nextTurn.EqualZero1 = pos1Bigger ? true : false;
            nextTurn.EqualZero2 = pos2Bigger ? true : false;
            nextTurn.Start1 = pos1Bigger ? End1 : End1 - Singleton.Instance.GetVerticalInfo(IDNextElement).RebarDevelopmentLengths[LocationIndex];
            nextTurn.Start2 = pos2Bigger ? End2 : End2 - Singleton.Instance.GetVerticalInfo(IDNextElement).RebarDevelopmentLengths[LocationIndex];
            Singleton.Instance.UpdateStandardTurn(nextTurn);
        }
        public bool Next()
        {
            if (!Swap && CanSwap)
            {
                Swap = true;
                return true;
            }
            if (IDVariable < VariableCount - 1)
            {
                IDVariable++;
                Swap = false;
                return true;
            }
            return false;
        }
    }
    public enum Position
    {
        Wrong=0,
        Position1 =1,
        Position2 =2
    }
    public enum TurnChosenType { Fit, PairFit, TripFit, Residual }
}
