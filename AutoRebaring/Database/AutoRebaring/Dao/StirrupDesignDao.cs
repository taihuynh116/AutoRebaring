using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class StirrupDesignDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StirrupDesignDao() { }
        public void Update(long idStirFamType, long idRebarBarType, long idDesLevel)
        {
            var res = db.ARStirrupDesigns.Where(x => x.IDDesignLevel == idDesLevel && x.IDStirrupFamilyType== idStirFamType);
            if (res.Count() == 0)
            {
                var obj = new ARStirrupDesign()
                {
                    IDDesignLevel = idDesLevel,
                    IDStirrupFamilyType = idStirFamType,
                    IDRebarBarType = idRebarBarType,
                    CreateDate = DateTime.Now
                };
                db.ARStirrupDesigns.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDRebarBarType = idRebarBarType;
            }
            db.SaveChanges();
        }
        public long GetId(long idStirFamType, long idDesLevel)
        {
            var res = db.ARStirrupDesigns.Where(x => x.IDDesignLevel == idDesLevel && x.IDStirrupFamilyType == idStirFamType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
