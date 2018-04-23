using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class DesignLevelDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public DesignLevelDao() { }
        public void Update(long idmark, long idDesignLevel, int numeric)
        {
            var res = db.ARDesignLevels.Where(x => x.IDMark == idmark && x.Numeric == numeric);
            if (res.Count() == 0)
            {
                var obj = new ARDesignLevel()
                {
                    IDMark = idmark,
                    Numeric = numeric,
                    IDDesignLevel = idDesignLevel,
                    CreateDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };
                db.ARDesignLevels.Add(obj);
            }
            else
            {
                var obj = new ARDesignLevel();
                obj.IDDesignLevel = idDesignLevel;
                obj.LastUpdate = DateTime.Now;
            }
            db.SaveChanges();
        }
        public long GetId(long idmark, int numeric)
        {
            var res = db.ARDesignLevels.Where(x => x.IDMark == idmark && x.Numeric == numeric);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.OrderByDescending(x => x.LastUpdate).First().ID;
        }
        public ARDesignLevel GetDesignLevel(long id)
        {
            var res = db.ARDesignLevels.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
