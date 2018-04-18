using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.ElementInfo;
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
            LocationIndex = turn.LocationIndex;
            Variable = turn.Variable;
            VariableIndex = turn.VariableIndex;
            Swap = turn.Swap;
            VariableIndex = turn.VariableIndex;
            ElementInfoCollection = turn.ElementInfoCollection;
            ElementIndex = turn.ElementIndex;
            Start1 = turn.Start1;
            Start2 = turn.Start2;

            Finish1 = turn.Finish1;
            Finish2 = turn.Finish2;
            Implant1 = turn.Implant1;
            Implant2 = turn.Implant2;
            l1Finish = turn.l1Finish;
            l2Finish = turn.l2Finish;
            EqualZero1 = turn.EqualZero1;
            EqualZero2 = turn.EqualZero2;
            IsImplanted = turn.IsImplanted;
            FirstPass = turn.FirstPass;

            GetL1L2();
        }
        public int LocationIndex { get; set; }
        public Variable Variable { get; set; }
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
        public int VariableIndex { get; set; } = 0;
        public int VariableCount
        {
            get
            {
                if (Implant1 && Implant2) return Variable.CountImplant12;
                if ((Implant1 && EqualZero2) || (Implant2 && EqualZero1)) return Variable.CountImplantEqualZero;
                if (Implant1 || Implant2)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit:
                            return Variable.CountStandardImplant;
                        case TurnChosenType.Residual:
                            return Variable.CountResidualImplant;
                    }
                }
                if (EqualZero1 || EqualZero2)
                {
                    switch (ChosenType)
                    {
                        case TurnChosenType.Fit:
                            return Variable.CountStandardEqualZero;
                        case TurnChosenType.Residual:
                            return Variable.CountResidualEqualZero;
                    }
                }
                switch (ChosenType)
                {
                    case TurnChosenType.Fit:
                        return Variable.CountStandard12;
                    case TurnChosenType.PairFit:
                        return Variable.CountStandard12;
                    case TurnChosenType.TripFit:
                        return Variable.CountStandard12;
                    case TurnChosenType.Residual:
                        return Variable.CountResidual12;
                }
                throw new Exception("This case have not benn checked yet!");
            }
        }
        public int Index { get; set; }
        public ElementInfoCollection ElementInfoCollection { get; set; }
        public IElementInfo ElementInfo
        {
            get { return elemInfo; }
            set
            {
                elemInfo = value;
            }
        }
        private IElementInfo elemInfo;
        public int ElementIndex
        {
            get { return elemIndex; }
            set
            {
                elemIndex = value < ElementInfoCollection.Count - 1 ? value : ElementInfoCollection.Count - 1;
                ElementInfo = ElementInfoCollection[elemIndex];
            }
        }
        private int elemIndex;
        public double Start1 { get; set; }
        public double Start2 { get; set; }
        public double End1
        {
            get { return Start1 + End1; }
        }
        public double End2
        {
            get { return Start2 + End2; }
        }
        public double L1 { get; set; }
        public double L2 { get; set; }
        public bool Finish1 { get; set; } = false;
        public bool Finish2 { get; set; } = false;
        public bool Implant1 { get; set; } = false;
        public bool Implant2 { get; set; } = false;
        public double l1Finish { get; set; }
        public double l2Finish { get; set; }
        public bool EqualZero1 { get; set; } = false;
        public bool EqualZero2 { get; set; } = false;
        public bool IsImplanted { get; set; } = false;
        public Position Position1
        {
            get
            {
                IVerticalInfo verInfo = ElementInfo.VerticalInfo;
                if (Finish1)
                {
                    return Position.Position3;
                }
                return checkPosition(End1, verInfo.EndLimit0s[LocationIndex], verInfo.StartLimit1s[LocationIndex], verInfo.EndLimit1s[LocationIndex], verInfo.StartLimit2s[LocationIndex], verInfo.EndLimit2s[LocationIndex]);
            }
        }
        public Position Position2
        {
            get
            {
                IVerticalInfo verInfo = ElementInfo.VerticalInfo;
                if (Finish2)
                {
                    return Position.Position3;
                }
                return checkPosition(End2, verInfo.EndLimit0s[LocationIndex], verInfo.StartLimit1s[LocationIndex], verInfo.EndLimit1s[LocationIndex], verInfo.StartLimit2s[LocationIndex], verInfo.EndLimit2s[LocationIndex]);
            }
        }
        public bool FirstPass { get; set; } = false;
        public TurnChosenType ChosenType { get; set; }
        public void SetFinish1(double lmax)
        {
            setFinish(lmax, Start1, End1, Finish1, l1Finish);
        }
        public void SetFinish2(double lmax)
        {
            setFinish(lmax, Start2, End2, Finish2, l2Finish);
        }
        public void setFinish(double lmax, double start, double end, bool type, double len)
        {
            if (checkManual(lmax, start, end))
            {
                double topLimit = ElementInfo.VerticalInfo.TopLockHead;
                double l = L1 - (end - topLimit);
                type = true;
                len = ConstantValue.milimeter2Feet * Math.Round(l * ConstantValue.feet2MiliMeter);
            }
        }
        public bool checkManual(double lmax, double start, double end)
        {
            double topLimit = ElementInfo.VerticalInfo.TopLockHead;
            return GeomUtil.IsEqualOrBigger(start + lmax, topLimit);
        }
        public void SetImplant1(double lmax, List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            setImplant(lmax, Start1, End1, Implant1, lImplants, lPlusImplants, ap, dp);
        }
        public void SetImplant2(double lmax, List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            setImplant(lmax, Start2, End2, Implant2, lImplants, lPlusImplants, ap, dp);
        }
        public void setImplant(double lmax, double start, double end, bool type, List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            setImplantChoice(lImplants, lPlusImplants, ap, dp);
            if (checkManual(lmax, start, end))
            {
                type = true;
            }
        }
        public void setImplantChoice(List<double> lImplants, List<double> lPlusImplants, ARAnchorParameter ap, ARDevelopmentParameter dp)
        {
            if (!IsImplanted)
            {
                Variable.SetImplant(lImplants, lPlusImplants, ap, dp, ElementInfo, LocationIndex);
                IsImplanted = true;
            }
        }
        public Position checkPosition(double end, double endLim0, double startLim1, double endLim1, double startLim2, double endLim2)
        {
            if (GeomUtil.IsEqualOrSmaller(end, endLim0))
                return Position.Position1;
            if (GeomUtil.IsBigger(end, endLim0) && GeomUtil.IsSmaller(end, startLim1))
                return Position.Wrong1;
            if (GeomUtil.IsEqualOrBigger(end, startLim1) && GeomUtil.IsEqualOrSmaller(end, endLim1))
                return Position.Position2;
            if (GeomUtil.IsBigger(end, endLim1) && GeomUtil.IsSmaller(end, startLim2))
                return Position.Wrong2;
            if (GeomUtil.IsEqualOrBigger(end, startLim2) && GeomUtil.IsEqualOrSmaller(end, endLim2))
                return Position.Position3;
            return Position.Wrong3;
        }
        public bool CheckValidNextTurn(StandardTurn nextTurn)
        {
            if ((int)Position1 >= 3 || (int)Position2 >= 3) return false;
            if ((int)Position1 == 0 || (int)Position2 == 0) return false;
            if (Position1 == Position2)
            {
                return EqualPositionCheck(nextTurn);
            }
            else if (Position1 < Position2)
            {
                return Position1LowerCheck(nextTurn);
            }
            return Position2LowerCheck(nextTurn);
        }
        public bool EqualPositionCheck(StandardTurn nextTurn)
        {
            int elemIndex = ElementIndex;
            switch (Position1)
            {
                case Position.Position1:
                    break;
                case Position.Position2:
                    elemIndex += 1;
                    break;
                case Position.Position3:
                    elemIndex += 2;
                    break;
            }
            double delta = Math.Abs(End1 - End2) - ElementInfoCollection[elemIndex].VerticalInfo.RebarDevelopmentLengths[LocationIndex];
            if (GeomUtil.IsSmaller(delta, 0)) return false;
            nextTurn.EqualZero1 = false;
            nextTurn.EqualZero2 = false;
            nextTurn.Start1 = End1 - ElementInfoCollection[elemIndex].VerticalInfo.RebarDevelopmentLengths[LocationIndex];
            nextTurn.Start2 = End2 - ElementInfoCollection[elemIndex].VerticalInfo.RebarDevelopmentLengths[LocationIndex];
            nextTurn.ElementIndex = elemIndex;
            return true;
        }
        public bool Position1LowerCheck(StandardTurn nextTurn)
        {
            int elemIndex = ElementIndex;
            switch (Position1)
            {
                case Position.Position1:
                    break;
                case Position.Position2:
                    elemIndex += 1;
                    break;
            }
            nextTurn.EqualZero1 = false;
            nextTurn.EqualZero2 = true;
            nextTurn.Start1 = nextTurn.End1 - ElementInfoCollection[elemIndex].VerticalInfo.RebarDevelopmentLengths[LocationIndex];
            nextTurn.Start2 = nextTurn.End2;
            nextTurn.ElementIndex = elemIndex;
            return true;
        }
        public bool Position2LowerCheck(StandardTurn nextTurn)
        {
            int elemIndex = ElementIndex;
            switch (Position2)
            {
                case Position.Position1:
                    break;
                case Position.Position2:
                    elemIndex += 1;
                    break;
            }
            nextTurn.EqualZero1 = false;
            nextTurn.EqualZero2 = true;
            nextTurn.Start1 = nextTurn.End1;
            nextTurn.Start2 = nextTurn.End2 - ElementInfoCollection[elemIndex].VerticalInfo.RebarDevelopmentLengths[LocationIndex];
            return true;
        }
        public bool Next()
        {
            if (!Swap && CanSwap)
            {
                Swap = true;
                GetL1L2();
                return true;
            }
            if (VariableIndex < VariableCount - 1)
            {
                VariableIndex++;
                GetL1L2();
                Swap = false;
                return true;
            }
            return false;
        }
        public void GetL1L2()
        {
            if (Implant1 && Implant2)
            {
                double li1 = Variable.L1Implants[VariableIndex];
                double li2 = Variable.L2Implants[VariableIndex];
                L1 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li1 - Start1;
                L2 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li2 - Start2;
            }
            else if (Implant1 && EqualZero2)
            {
                double li1 = Variable.LImplants[VariableIndex];
                L1 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li1 - Start1;
                L2 = 0;
            }
            else if (Implant2 && EqualZero1)
            {
                double li2 = Variable.LImplants[VariableIndex];
                L1 = 0;
                L2 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li2 - Start2;
            }
            else if (Implant1)
            {
                double li1 = -1;
                switch (ChosenType)
                {
                    case TurnChosenType.Fit:
                        li1 = Variable.L1ImplantStandards[VariableIndex];
                        L1 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li1 - Start1;
                        L2 = Variable.L2ImplantStandards[VariableIndex];
                        break;
                    case TurnChosenType.Residual:
                        li1 = Variable.L1ImplantResiduals[VariableIndex];
                        L1 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li1 - Start1;
                        L2 = Variable.L2ImplantResiduals[VariableIndex];
                        break;
                }
            }
            else if (Implant2)
            {
                double li2 = -1;
                switch (ChosenType)
                {
                    case TurnChosenType.Fit:
                        li2 = Variable.L2StandardImplants[VariableIndex];
                        L2 = Variable.L1StandardImplants[VariableIndex];
                        L2 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li2 - Start2;
                        break;
                    case TurnChosenType.Residual:
                        li2 = Variable.L2ResidualImplants[VariableIndex];
                        L2 = Variable.L1ResidualImplants[VariableIndex];
                        L2 = ElementInfo.VerticalInfo.TopAnchorAfters[LocationIndex] - li2 - Start2;
                        break;
                }
            }
            else if (EqualZero1)
            {
                switch (ChosenType)
                {
                    case TurnChosenType.Fit:
                        L1 = 0;
                        L2 = Variable.LStandards[VariableIndex];
                        break;
                    case TurnChosenType.Residual:
                        L1 = 0;
                        L2 = Variable.LResiduals[VariableIndex];
                        break;
                }
            }
            else if (EqualZero2)
            {
                switch (ChosenType)
                {
                    case TurnChosenType.Fit:
                        L1 = Variable.LStandards[VariableIndex];
                        L2 = 0;
                        break;
                    case TurnChosenType.Residual:
                        L1 = Variable.LResiduals[VariableIndex];
                        L2 = 0;
                        break;
                }
            }
            else
            {
                switch (ChosenType)
                {
                    case TurnChosenType.Fit:
                        L1 = Variable.L1Standards[VariableIndex];
                        L2 = Variable.L2Standards[VariableIndex];
                        break;
                    case TurnChosenType.Residual:
                        L1 = Variable.L1Residuals[VariableIndex];
                        L2 = Variable.L2Residuals[VariableIndex];
                        break;
                }
            }
        }
    }
    public enum Position
    {
        Position1, Position2, Position3, Wrong1, Wrong2, Wrong3
    }
    public enum TurnChosenType { Fit, PairFit, TripFit, Residual }
}
