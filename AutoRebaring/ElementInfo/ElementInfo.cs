using Autodesk.Revit.DB;
using AutoRebaring.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    
    public class ColumnInfo : IElementInfo
    {
        public IPlaneInfo PlaneInfo { get; set; }
        public IVerticalInfo VerticalInfo { get; set; }
        public IDesignInfo DesignInfo { get; set; }
        public IRevitInfo RevitInfo { get; set; }
        public ColumnInfo(Document doc, Element e, ColumnParameter param)
        {
            RevitInfo = new RevitInfo(doc, e);
            PlaneInfo = new ColumnPlaneInfo(RevitInfo, param);
        }
    }

    public class WallInfo : IElementInfo
    {
        public IPlaneInfo PlaneInfo { get; set; }
        public IVerticalInfo VerticalInfo { get; set; }
        public IDesignInfo DesignInfo { get; set; }
        public IRevitInfo RevitInfo { get; set; }
        public WallInfo(Document doc, Element e, ColumnParameter param)
        {
            RevitInfo = new RevitInfo(doc, e);
            //PlaneInfo = new WallPlaneInfo(RevitInfo, param);
        }
    }
}
