using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class EDParameterTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public EDParameterTypeDao() { }
        public long GetId(long idElemType, string type)
        {
            var res = db.AREDParameterTypes.Where(x => x.IDElementType == idElemType && x.Type == type);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
