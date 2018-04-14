using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardStartZDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardStartZDao() { }
        public void Update(long idDesignGeneral, long idStandStartZType, double z1, double z2)
        {
            var res = db.ARStandardStartZs.Where(x => x.IDStandardDesignGeneral == idDesignGeneral && x.IDStandardStartZType == idStandStartZType);
            if (res.Count() == 0)
            {
                var obj = new ARStandardStartZ()
                {
                    IDStandardDesignGeneral = idDesignGeneral,
                    IDStandardStartZType = idStandStartZType,
                    Z1 = z1,
                    Z2 = z2,
                    CreateDate = DateTime.Now

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
        public long GetId(long idDesignGeneral, long idStandStartZType)
        {
            var res = db.ARStandardStartZs.Where(x => x.IDStandardDesignGeneral == idDesignGeneral && x.IDStandardStartZType == idStandStartZType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStandardStartZ GetStandardStartZ(long id)
        {
            var res = db.ARStandardStartZs.Where(x=> x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
