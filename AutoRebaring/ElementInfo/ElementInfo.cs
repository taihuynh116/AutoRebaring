using Autodesk.Revit.DB;
using AutoRebaring.Database;
using AutoRebaring.ElementInfo.RebarInfo.StandardInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class ElementInfo : IElementInfo
    {
        public IPlaneInfo PlaneInfo { get; set; }
        public IVerticalInfo VerticalInfo { get; set; }
        public IDesignInfo DesignInfo { get; set; }
        public IRevitInfo RevitInfo { get; set; }
        
        public ElementInfo() { }
        
    }
}
