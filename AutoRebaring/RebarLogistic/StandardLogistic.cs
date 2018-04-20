using AutoRebaring.Constant;
using AutoRebaring.ElementInfo;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.RebarLogistic
{
    public class StandardLogistic
    {
        public int Count;
        public int LocationIndex { get; set; }
        public int LoopCount { get { return TrackingTopIndex - TrackingBottomIndex; } }
        public List<StandardTurn> ProgressTurns = new List<StandardTurn>();
        public List<StandardTurn> TrackingTurns = new List<StandardTurn>();
        public int TrackingTopIndex;
        public int TrackingBottomIndex
        {
            get
            {
                int index = TrackingTopIndex - ConstantValue.LoopCount;
                if (index > TrackingBottomLimit)
                    return index;
                return TrackingBottomLimit;
            }
        }
        public int TrackingBottomLimit = 0;
        public ElementInfoCollection ElementInfoCollection;
        public int ElementCount;
        public IInputForm InputForm;
        public StandardLogistic(ElementInfoCollection elemInfoColl, IInputForm inputForm, int locIndex)
        {
            for (int i = 0; i < elemInfoColl.Count; i++)
            {
                StandardTurn st = new StandardTurn() {
                    Index = i,
                    LocationIndex = locIndex,
                    ElementInfoCollection = elemInfoColl,
                    Variable = new Variable(inputForm.StandardChosen, inputForm.FitStandards,
                        inputForm.PairFitStandards, inputForm.TripFitStandards)
                };
                //st.GetL1L2();
                ProgressTurns.Add(st);
            }
            
            ElementInfoCollection = elemInfoColl;
            ElementCount = ElementInfoCollection.Count;
            InputForm = inputForm;
            LocationIndex = locIndex;

            ProgressTurns[0].Start1 = InputForm.RebarZ1s[LocationIndex] - ElementInfoCollection[0].VerticalInfo.RebarDevelopmentLengths[LocationIndex];
            ProgressTurns[0].Start2 = InputForm.RebarZ2s[LocationIndex] - ElementInfoCollection[0].VerticalInfo.RebarDevelopmentLengths[LocationIndex];
        }
        public bool RunLogistic()
        {
            for (int i = 0; i < ProgressTurns.Count; i++)
            {
                FirstLine:
                while (true)
                {
                    if (!ProgressTurns[i].Swap)
                    {
                        if (CheckPosition(i))
                        {
                            if (ProgressTurns[i].Finish1 && ProgressTurns[i].Finish2)
                            {
                                Count = i + 1;
                                goto L1;
                            }
                            break;
                        }
                    }
                    if (ProgressTurns[i].CanSwap)
                    {
                        ProgressTurns[i].Swap = true;
                        if (CheckPosition(i))
                        {
                            if (ProgressTurns[i].Finish1 && ProgressTurns[i].Finish2)
                            {
                                Count = i + 1;
                                goto L1;
                            }
                            break;
                        }
                    }
                    while (!ProgressTurns[i].Next())
                    {
                        if (!ProgressTurns[i].FirstPass)
                        {
                            for (int k = 0; k <= i; k++)
                            {
                                ProgressTurns[i].FirstPass = true;
                            }
                            TrackingTopIndex = i;
                            TrackingTurns = new List<StandardTurn>();
                            for (int k = i - LoopCount; k <= i; k++)
                            {
                                TrackingTurns.Add(new StandardTurn(ProgressTurns[k]));
                            }
                        }
                        ProgressTurns[i].VariableIndex = 0;
                        i--;
                        if (i < TrackingBottomIndex)
                        {
                            i = TrackingTopIndex;
                            if (ProgressTurns[i].ChosenType == TurnChosenType.Residual)
                            {
                                throw new Exception("There are not any case matching this loop. You should increase loop count!");
                            }
                            int j = 0;
                            for (int k = i - LoopCount; k <= i; k++)
                            {
                                ProgressTurns[k] = TrackingTurns[j]; j++;
                            }
                            ProgressTurns[i].ChosenType = TurnChosenType.Residual;
                            ProgressTurns[i].VariableIndex = 0;
                            goto FirstLine;
                        }
                    }
                }
            }
            L1:
            return true;
        }
        public bool CheckPosition(int i)
        {
            double lmax = InputForm.StandardChosen.Lmax * ConstantValue.milimeter2Feet;
            if (ProgressTurns[i].ElementIndex == ElementCount - 1)
            {
                ProgressTurns[i].SetFinish1(lmax);
                ProgressTurns[i].SetFinish2(lmax);
            }
            else
            {
                if (ElementInfoCollection[ProgressTurns[i].ElementIndex].VerticalInfo.SmallStandardChosens[LocationIndex])
                {
                    ProgressTurns[i].SetImplant1(lmax, InputForm.FitImplants, InputForm.PairFitImplants, InputForm.AnchorParameter, InputForm.DevelopmentParameter);
                    ProgressTurns[i].SetImplant2(lmax, InputForm.FitImplants, InputForm.PairFitImplants, InputForm.AnchorParameter, InputForm.DevelopmentParameter);
                }
            }

            return ProgressTurns[i].CheckValidNextTurn(ProgressTurns[i + 1]);
        }
    }
}
