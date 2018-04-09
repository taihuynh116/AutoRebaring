using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class DimensionParameterTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public DimensionParameterTypeDao() { }
        public long GetId(long idElemType)
        {
            var res = db.ARDimensionParameterTypes.Where(x => x.IDElementType == idElemType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
