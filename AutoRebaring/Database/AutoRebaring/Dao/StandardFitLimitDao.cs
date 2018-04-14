using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardFitLimitDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardFitLimitDao() { }
        public void Update(long idProject, long idStandardFitType, int limit)
        {
            var res = db.ARStandardFitLimits.Where(x => x.IDProject == idProject && x.IDStandardFitType == idStandardFitType);
            if (res.Count() == 0)
            {
                var obj = new ARStandardFitLimit()
                {
                    IDProject = idProject,
                    IDStandardFitType = idStandardFitType,
                    Limit = limit,
                    CreateDate = DateTime.Now
                };
                db.ARStandardFitLimits.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Limit = limit;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject, long idStandardFitType)
        {
            var res = db.ARStandardFitLimits.Where(x => x.IDProject == idProject && x.IDStandardFitType == idStandardFitType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStandardFitLimit GetStandardFitLimit(long id)
        {
            var res = db.ARStandardFitLimits.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
