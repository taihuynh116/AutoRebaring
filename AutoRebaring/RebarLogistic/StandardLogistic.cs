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
        public StandardLogistic(ElementInfoCollection elemInfoColl, IInputForm inputForm)
        {
            for (int i = 0; i < elemInfoColl.Count; i++)
            {
                ProgressTurns.Add(new StandardTurn() { Index = i });
            }
            ElementInfoCollection = elemInfoColl;
            ElementCount = ElementInfoCollection.Count;
            InputForm = inputForm;
        }
        public bool RunLogistic()
        {
            for (int i = 0; i < ProgressTurns.Count; i++)
            {
                while (true)
                {
                    if (!ProgressTurns[i].Swap)
                    {
                        if (Check(i))
                        {

                        }
                    }
                }
            }
        }
        public bool Check(int i)
        {
            double lmax = InputForm.StandardChosen.Lmax * ConstantValue.milimeter2Feet;
            if (ProgressTurns[i].ElementIndex == ElementCount - 1)
            {
                ProgressTurns[i].SetFinish1(lmax);
                ProgressTurns[i].SetFinish2(lmax);
            }
            else
            {
                if (ElementInfoCollection[ProgressTurns[i].ElementIndex].VerticalInfo.SmallStandardChosens[0])
                {
                    ProgressTurns[i].SetImplant1(lmax, InputForm.LImplants, InputForm.LPlusImplants, InputForm.AnchorParameter, InputForm.DevelopmentParameter);
                    ProgressTurns[i].SetImplant2(lmax, InputForm.LImplants, InputForm.LPlusImplants, InputForm.AnchorParameter, InputForm.DevelopmentParameter);
                }
            }

            
        }
    }
}
