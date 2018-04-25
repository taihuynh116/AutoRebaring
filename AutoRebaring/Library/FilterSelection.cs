using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class WallAndColumnSelection : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem is Wall)
                return true;
            if (elem is FamilyInstance && elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns)
                return true;
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
