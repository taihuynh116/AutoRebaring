using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public interface IRevitInfo
    {
        Document Document { get; set; }
        Element Element { get; set; }
    }
    public class RevitInfo:IRevitInfo
    {
        public Document Document { get; set; }
        public Element Element { get; set; }
        public RevitInfo(Document doc, Element e)
        {
            Document = doc;
            Element = e;
        }
    }
}
