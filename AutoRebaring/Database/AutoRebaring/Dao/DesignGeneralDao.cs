using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class DesignGeneralDao
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
                    IDStandardEndType = idStandardEndType
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
    }
}
