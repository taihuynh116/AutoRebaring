using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    
    public class RevitInfo:IRevitInfo
    {
        public int ID { get; set; }
        public Document Document { get; set; }
        public Element Element { get; set; }
        public double Elevation { get; set; }
        public Level Level { get; set; }

        public RevitInfo(Document doc, Element e)
        {
            Document = doc;
            Element = e;
            if (!(e is Wall))
            {
                Level = doc.GetElement(e.LookupParameter("Base Level").AsElementId()) as Level;
            }
            else
            {
                Level = doc.GetElement(e.LookupParameter("Base Constraint").AsElementId()) as Level;
            }
            Elevation = Level.Elevation;
        }
    }
}
