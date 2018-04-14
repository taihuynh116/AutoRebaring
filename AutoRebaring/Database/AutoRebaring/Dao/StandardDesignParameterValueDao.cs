using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardDesignParameterValueDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardDesignParameterValueDao() { }
        public void Update(long idDesignLevel, long idStandDesParamType, int value)
        {
            var res = db.ARStandardDesignParameterValues.Where(x => x.IDDesignLevel == idDesignLevel && x.IDStandardDesignParameterType== idStandDesParamType);
            if (res.Count() == 0)
            {
                var obj = new ARStandardDesignParameterValue()
                {
                    IDDesignLevel = idDesignLevel,
                    IDStandardDesignParameterType = idStandDesParamType,
                    Value = value,
                    CreateDate = DateTime.Now
                };
                db.ARStandardDesignParameterValues.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Value = value;
            }
            db.SaveChanges();
        }
        public long GetId(long idDesignLevel, long idStandDesParamType)
        {
            var res = db.ARStandardDesignParameterValues.Where(x => x.IDDesignLevel == idDesignLevel && x.IDStandardDesignParameterType == idStandDesParamType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStandardDesignParameterValue GetStandardDesignParameterValue(long id)
        {
            var res = db.ARStandardDesignParameterValues.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
