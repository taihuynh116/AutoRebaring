using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class DesignLevelLimitDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public DesignLevelLimitDao() { }
        public void Update(long idmark, int limit)
        {
            var res = db.ARDesignLevelLimits.Where(x => x.IDMark == idmark);
            if (res.Count() == 0)
            {
                var obj = new ARDesignLevelLimit()
                {
                    IDMark = idmark,
                    Limit = limit,
                    CreateDate = DateTime.Now
                };
                db.ARDesignLevelLimits.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Limit = limit;
            }
            db.SaveChanges();
        }
        public long GetId(long idmark)
        {
            var res = db.ARDesignLevelLimits.Where(x => x.IDMark == idmark);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARDesignLevelLimit GetDesignLevelLimit(long id)
        {
            var res = db.ARDesignLevelLimits.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
