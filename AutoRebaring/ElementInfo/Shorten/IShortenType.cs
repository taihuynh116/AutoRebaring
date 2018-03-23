using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace AutoRebaring.ElementInfo.Shorten
{
    public class Shorten
    {
        public enum EnumShorten
        {
            None, Small, Big
        }
        public EnumShorten U { get; set; }
        public EnumShorten V { get; set; }
        public Shorten(EnumShorten u, EnumShorten v)
        { U = u; V = v; }
    }

    public class Delta
    {
        public double U { get; set; }
        public double V { get; set; }
        public Delta(double u, double v)
        { U = u; V = v; }
    }

    public interface IShortenType
    {
        Shorten Shorten1 { get; set; }
        Shorten Shorten2 { get; set; }
        Delta Delta1 { get; set; }
        Delta Delta2 { get; set; }
    }
    public class ShortenType : IShortenType
    {
        public Shorten Shorten1 { get; set; }
        public Shorten Shorten2 { get; set; }
        public Delta Delta1 { get; set; }
        public Delta Delta2 { get; set; }
        public ShortenType() { }
        public ShortenType(Shorten.EnumShorten sU1, Shorten sU2, Shorten sV1, Shorten sV2, double dU1, double dU2, double dV1, double dV2)
        {
            //Shorten1.U = sU1; ShortenU2 = sU2; ShortenV1 = sV1; ShortenV2 = sV2; DeltaU1 = dU1; DeltaU2 = dU2; DeltaV1 = dV1; DeltaV2 = dV2;
        }
    }
}
