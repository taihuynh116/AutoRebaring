using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo.RebarInfo.StandardInfo
{
    public interface IStandardParameter
    {
        List<string> ParameterNames { get; set; }
        List<object> ParameterValues { get; set; }
    }
    public class StandardParameter : IStandardParameter
    {
        public List<string> ParameterNames { get; set; }
        public List<object> ParameterValues { get; set; }
    }
}
