using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardStartZTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardStartZTypeDao() { }
        public long GetId(long idElemType, string type)
        {
            var res = db.ARStandardStartZTypes.Where(x => x.IDElementType == idElemType && x.Type == type);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
