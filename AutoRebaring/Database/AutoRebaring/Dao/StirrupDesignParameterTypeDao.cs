using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class StirrupDesignParameterTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StirrupDesignParameterTypeDao() { }
        public long GetId(string param)
        {
            var res = db.ARStirrupDesignParameterTypes.Where(x => x.Parameter == param);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
