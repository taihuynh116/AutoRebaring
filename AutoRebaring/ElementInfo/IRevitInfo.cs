using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class IRevitInfo
    {
        public Document Document { get; set; }
        public Element Element { get; set; }
        public IRevitInfo(Document doc, Element e)
        {
            Document = doc;
            Element = e;
        }
    }
}
