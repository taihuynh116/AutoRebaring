using AutoRebaring.Constant;
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
    public class StandardLogistic
    {
        public int Count;
        public int LocationIndex { get; set; }
        public int LoopCount { get { return TrackingTopIndex - TrackingBottomIndex; } }
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
        public int ElementCount;
        public List<StandardTurn> TrackingTurns;
        public List<StandardTurn> ProgressTurns = new List<StandardTurn>();
        public StandardLogistic(int locIndex)
        {
            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                StandardTurn st = new StandardTurn()
                {
                    Index = i,
                    LocationIndex = locIndex,
                    Variable = new Variable()
                };
                ProgressTurns.Add(st);
            }

            LocationIndex = locIndex;

            ProgressTurns[0].Start1 = Singleton.Instance.RebarZ1s[LocationIndex] - Singleton.Instance.GetVerticalInfo(0).RebarDevelopmentLengths[LocationIndex];
            ProgressTurns[0].Start2 = Singleton.Instance.RebarZ2s[LocationIndex] - Singleton.Instance.GetVerticalInfo(0).RebarDevelopmentLengths[LocationIndex];
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
                                ProgressTurns[k].FirstPass = true;
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
                                ProgressTurns[k] = ProgressTurns[i];
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
            double lmax = Singleton.Instance.StandardChosen.Lmax * ConstantValue.milimeter2Feet;
            if (ProgressTurns[i].IDElement == ElementCount - 1)
            {
                ProgressTurns[i].SetFinish1(lmax);
                ProgressTurns[i].SetFinish2(lmax);
            }
            else
            {
                if (Singleton.Instance.GetVerticalInfo(ProgressTurns[i].IDElement).SmallStandardChosens[LocationIndex])
                {
                    ProgressTurns[i].SetImplant1();
                    ProgressTurns[i].SetImplant2();
                }
            }

            return ProgressTurns[i].CheckValidNextTurn(ProgressTurns[i+1]);
        }
    }
}
