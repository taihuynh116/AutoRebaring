using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.Shorten
{
    public enum Shorten
    {
        None, Small, Big
    }
    public interface IShortenType
    {
        Shorten ShortenU1 { get; set; }
        Shorten ShortenU2 { get; set; }
        Shorten ShortenV1 { get; set; }
        Shorten ShortenV2 { get; set; }
        double DeltaU1 { get; set; }
        double DeltaU2 { get; set; }
        double DeltaV1 { get; set; }
        double DeltaV2 { get; set; }
    }
    public class ShortenType : IShortenType
    {
        public Shorten ShortenU1 { get; set; }
        public Shorten ShortenU2 { get; set; }
        public Shorten ShortenV1 { get; set; }
        public Shorten ShortenV2 { get; set; }
        public double DeltaU1 { get; set; }
        public double DeltaU2 { get; set; }
        public double DeltaV1 { get; set; }
        public double DeltaV2 { get; set; }
        public ShortenType(Shorten sU1, Shorten sU2, Shorten sV1, Shorten sV2, double dU1, double dU2, double dV1, double dV2)
        {
            ShortenU1 = sU1; ShortenU2 = sU2; ShortenV1 = sV1; ShortenV2 = sV2;
            DeltaU1 = dU1; DeltaU2 = dU2; DeltaV1 = dV1; DeltaV2 = dV2;
        }
    }
}
