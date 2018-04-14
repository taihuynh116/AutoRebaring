using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardDesignParameterTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardDesignParameterTypeDao() { }
        public long GetId(long idElemType, string param)
        {
            var res = db.ARStandardDesignParameterTypes.Where(x => x.IDElementType == idElemType && x.Parameter == param);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
