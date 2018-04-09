using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class RebarDesignTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public RebarDesignTypeDao() { }
        public long GetId(string type)
        {
            var res = db.ARRebarDesignTypes.Where(x => x.Type == type);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
