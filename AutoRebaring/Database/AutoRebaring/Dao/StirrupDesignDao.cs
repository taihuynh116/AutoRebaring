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
        public void Update(long idStirFamType, long idRebarDesType, long idRebarBarType, long idDesLevel)
        {
            var res = db.ARStirrupDesigns.Where(x => x.IDDesignLevel == idDesLevel && x.IDRebarDesignType== idRebarDesType);
            if (res.Count() == 0)
            {
                var obj = new ARStirrupDesign()
                {
                    IDDesignLevel = idDesLevel,
                    IDStirrupFamilyType = idStirFamType,
                    IDRebarDesignType= idRebarDesType,
                    IDRebarBarType = idRebarBarType
                };
                db.ARStirrupDesigns.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDStirrupFamilyType = idStirFamType;
                obj.IDRebarBarType = idRebarBarType;
            }
            db.SaveChanges();
        }
        public long GetId(long idRebarDesType, long idDesLevel)
        {
            var res = db.ARStirrupDesigns.Where(x => x.IDDesignLevel == idDesLevel && x.IDRebarDesignType == idRebarDesType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
