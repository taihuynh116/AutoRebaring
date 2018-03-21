using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.ElementInfo
{
    public class IRebarParameter
    {
        public int PartCount { get; set; }
        public string Mark { get; set; }
        public List<string> Levels { get; set; }
        public List<string> MetaLevels { get; set; }
        public string FirstMetaLevel { get; set; }
        public string LastMetaLevel { get; set; }
        public string Partition { get; set; }
        public IRebarParameter(int partCount, string mark, List<string> levels, List<string> metaLevels, string firstMetaLevel, string lastMetaLevel)
        {
            PartCount = partCount;
            Mark = mark;
            Levels = levels;
            MetaLevels = metaLevels;
            FirstMetaLevel = firstMetaLevel;
            LastMetaLevel = lastMetaLevel;
            Partition = Mark + "_" + FirstMetaLevel + "_" + LastMetaLevel;
        }
        public string FindMetaLevel(string level)
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                if (level == Levels[i])
                    return MetaLevels[i];
            }
            return "";
        }
    }
}
