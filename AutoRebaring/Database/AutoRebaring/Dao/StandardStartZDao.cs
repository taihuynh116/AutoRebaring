using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class StandardStartZDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardStartZDao() { }
        public void Update(long idDesignGeneral, long idStandardDesignGeneral, double z1, double z2)
        {
            var res = db.ARStandardStartZs.Where(x => x.IDStandardDesignGeneral == idDesignGeneral && x.IDStandardDesignGeneral== idStandardDesignGeneral);
            if (res.Count() == 0)
            {
                var obj = new ARStandardStartZ()
                {
                    IDStandardDesignGeneral = idDesignGeneral,
                    IDStandardStartZType = idStandardDesignGeneral,
                    Z1 = z1,
                    Z2 = z2
                };
                db.ARStandardStartZs.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Z1 = z1;
                obj.Z2 = z2;
            }
            db.SaveChanges();
        }
        public long GetId(long idDesignGeneral, long idStandardDesignGeneral)
        {
            var res = db.ARStandardStartZs.Where(x => x.IDStandardDesignGeneral == idDesignGeneral && x.IDStandardDesignGeneral == idStandardDesignGeneral);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
