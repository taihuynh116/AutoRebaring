using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class StirrupDesignParameterValueDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StirrupDesignParameterValueDao() { }
        public void Update(long idDesignLevel, long idStirDesParamType, double value)
        {
            var res = db.ARStirrupDesignParameterValues.Where(x => x.IDDesignLevel == idDesignLevel && x.IDStirrupDesignParameterType == idStirDesParamType);
            if (res.Count() == 0)
            {
                var obj = new ARStirrupDesignParameterValue()
                {
                    IDDesignLevel = idDesignLevel,
                    IDStirrupDesignParameterType = idStirDesParamType,
                    Value = value,
                    CreateDate = DateTime.Now
                };
                db.ARStirrupDesignParameterValues.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Value = value;
            }
            db.SaveChanges();
        }
        public long GetId(long idDesignLevel, long idStirDesParamType)
        {
            var res = db.ARStirrupDesignParameterValues.Where(x => x.IDDesignLevel == idDesignLevel && x.IDStirrupDesignParameterType == idStirDesParamType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
