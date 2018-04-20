using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class ElementTypeInfo:IElementTypeInfo
    {
        public static int NextID = 0;
        public int ID { get; set; }
        public ElementTypeEnum Type { get; set; }
        public int LocationCount { get; set; }
        public ElementTypeInfo(ElementTypeEnum type, int locCount)
        {
            ID = NextID;
            Type = type;
            LocationCount = locCount;

            NextID++;
        }
    }
    public enum ElementTypeEnum
    {
        Column, Wall
    }
}
