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
        public StandardLogistic(int locIndex)
        {
            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                StandardTurn st = new StandardTurn()
                {
                    ID = i,
                    LocationIndex = locIndex,
                };
                Singleton.Instance.AddStandardTurn(st);
            }

            LocationIndex = locIndex;

            Singleton.Instance.GetStandardTurn(0, locIndex).Start1 = Singleton.Instance.RebarZ1s[LocationIndex] - Singleton.Instance.GetVerticalInfo(0).RebarDevelopmentLengths[LocationIndex];
            Singleton.Instance.GetStandardTurn(0, locIndex).Start2 = Singleton.Instance.RebarZ2s[LocationIndex] - Singleton.Instance.GetVerticalInfo(0).RebarDevelopmentLengths[LocationIndex];
        }
        public bool RunLogistic()
        {
            for (int i = 0; i < Singleton.Instance.GetElementCount(); i++)
            {
                FirstLine:
                while (true)
                {
                    StandardTurn st = Singleton.Instance.GetStandardTurn(i, LocationIndex);
                    StandardTurn stN = Singleton.Instance.GetStandardTurnAfter(i, LocationIndex);
                    if (!st.Swap)
                    {
                        if (st.IsValidate)
                        {
                            if (st.Finish1 && st.Finish2)
                            {
                                Singleton.Instance.AddStandardTurnCount(LocationIndex, i + 1);
                                goto L1;
                            }
                            st.HandleNextTurn();
                            break;
                        }
                    }
                    if (st.CanSwap)
                    {
                        st.Swap = true;
                        if (st.IsValidate)
                        {
                            if (st.Finish1 && st.Finish2)
                            {
                                Singleton.Instance.AddStandardTurnCount(LocationIndex, i + 1);
                                goto L1;
                            }
                            st.HandleNextTurn();
                            break;
                        }
                    }
                    while (!st.Next())
                    {
                        if (!st.FirstPass)
                        {
                            for (int k = 0; k <= i; k++)
                            {
                                Singleton.Instance.GetStandardTurn(k, LocationIndex).FirstPass = true;
                            }
                            TrackingTopIndex = i;
                            TrackingTurns = new List<StandardTurn>();
                            for (int k = i - LoopCount; k <= i; k++)
                            {
                                TrackingTurns.Add(new StandardTurn(Singleton.Instance.GetStandardTurn(k, LocationIndex)));
                            }
                        }
                        st.IDVariable = 0;
                        Singleton.Instance.UpdateStandardTurn(st);
                        i--;
                        if (i>=0)
                            st = Singleton.Instance.GetStandardTurn(i, LocationIndex);
                        
                        if (i < TrackingBottomIndex)
                        {
                            i = TrackingTopIndex;
                            st = Singleton.Instance.GetStandardTurn(i, LocationIndex);
                            if (st.ChosenType == TurnChosenType.Residual)
                            {
                                throw new Exception("There are not any case matching this loop. You should increase loop count!");
                            }
                            int j = 0;
                            for (int k = i - LoopCount; k <= i; k++)
                            {
                                Singleton.Instance.UpdateStandardTurn(TrackingTurns[j]); j++;
                            }
                            st.ChosenType = TurnChosenType.Residual;
                            st.IDVariable = 0;
                            Singleton.Instance.UpdateStandardTurn(st);
                            goto FirstLine;
                        }
                    }
                }
            }
            L1:
            return true;
        }
    }
}
