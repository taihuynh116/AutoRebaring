using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Constant
{
    public static class ConstantValue
    {
        public static int LoopCount = 20;
        public static double RebarStandardOffsetControl = 1.5;
        public const double feet2Meter = 0.3048;
        public const double meter2Feet = 1/feet2Meter;
        public const double feet2MiliMeter = feet2Meter * 1000;
        public const double milimeter2Feet = 1 / feet2MiliMeter;
    }
}
