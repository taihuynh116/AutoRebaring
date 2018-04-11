using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class LockheadParametersDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public LockheadParametersDao() { }
        public void Update(long idProject, double shortenLimit, int lockheadMulti, double lockheadConcCover, double smallConcCover, double lhRatio)
        {
            var res = db.ARLockheadParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARLockheadParameter()
                {
                    IDProject = idProject,
                    ShortenLimit = shortenLimit,
                    LockheadMutiply = lockheadMulti,
                    LockheadConcreteCover = lockheadConcCover,
                    SmallConcreteCover = smallConcCover,
                    LHRatio = lhRatio,
                    CreateDate = DateTime.Now
                };
                db.ARLockheadParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.ShortenLimit = shortenLimit;
                obj.LockheadMutiply = lockheadMulti;
                obj.LockheadConcreteCover = lockheadConcCover;
                obj.SmallConcreteCover = smallConcCover;
                obj.LHRatio = lhRatio;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject)
        {
            var res = db.ARLockheadParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARLockheadParameter GetLockheadParameter(long id)
        {
            var res = db.ARLockheadParameters.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
