using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class RebarDesignTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public RebarDesignTypeDao() { }
        public long GetId(long idElemType, long idRebarType, string type)
        {
            var res = db.ARRebarDesignTypes.Where(x => x.IDElementType == idElemType && x.IDRebarType == idRebarType && x.Type == type);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
