using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class DesignLevelDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public DesignLevelDao() { }
        public void Update(long idmark, long idDesignLevel)
        {
            var res = db.ARDesignLevels.Where(x => x.IDMark == idmark && x.IDDesignLevel==idDesignLevel);
            if (res.Count() == 0)
            {
                var obj = new ARDesignLevel()
                {
                    IDMark = idmark,
                    IDDesignLevel = idDesignLevel,
                    CreateDate = DateTime.Now
                };
                db.ARDesignLevels.Add(obj);
            }
            db.SaveChanges();
        }
        public long GetId(long idmark, long idDesignLevel)
        {
            var res = db.ARDesignLevels.Where(x => x.IDMark == idmark && x.IDDesignLevel == idDesignLevel);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
