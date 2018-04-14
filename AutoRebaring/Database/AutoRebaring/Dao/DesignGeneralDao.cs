using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class DesignGeneralDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public DesignGeneralDao() { }
        public void Update(long idMark, long idStartLevel, long idEndLevel, long idStandardEndType)
        {
            var res = db.ARDesignGenerals.Where(x => x.IDMark == idMark && x.IDStartLevel == idStartLevel && x.IDEndLevel == idEndLevel && x.IDStandardEndType == idStandardEndType);
            if (res.Count() == 0)
            {
                var obj = new ARDesignGeneral()
                {
                    IDMark = idMark,
                    IDStartLevel = idStartLevel,
                    IDEndLevel = idEndLevel,
                    IDStandardEndType = idStandardEndType,
                    CreateDate= DateTime.Now
                };
                db.ARDesignGenerals.Add(obj);
            }
            db.SaveChanges();
        }
        public long GetId(long idMark, long idStartLevel, long idEndLevel, long idStandardEndType)
        {
            var res = db.ARDesignGenerals.Where(x => x.IDMark == idMark && x.IDStartLevel == idStartLevel && x.IDEndLevel == idEndLevel && x.IDStandardEndType == idStandardEndType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public long GetId(long idMark)
        {
            var res = db.ARDesignGenerals.Where(x => x.IDMark == idMark);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.OrderByDescending(x => x.CreateDate).First().ID;
        }
        public ARDesignGeneral GetDesignGeneral(long id)
        {
            var res = db.ARDesignGenerals.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
