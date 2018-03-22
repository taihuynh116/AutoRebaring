using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo
{
    public interface IStandardGeneralInfo
    {
        RebarLayoutRule LayoutRule { get; set; }
        RebarHookType HookType { get; set; }
        List<RebarBarType> BarTypes { get; set; }
        List<RebarBarType> BarTypeAfters { get; set;}
        List<RebarBarType> BarTypeBefores { get; set; }
        RebarStyle Style { get; set; }
    }
    public class StandardGeneralInfo : IStandardGeneralInfo
    {
        public RebarLayoutRule LayoutRule { get; set; }
        public RebarHookType HookType { get; set; }
        public List<RebarBarType> BarTypes { get; set; }
        public List<RebarBarType> BarTypeAfters { get; set; }
        public List<RebarBarType> BarTypeBefores { get; set; }
        public RebarStyle Style { get; set; }
    }
}
