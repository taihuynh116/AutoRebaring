using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class LockheadParametersDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public LockheadParametersDao() { }
        public void Update(long idProject, double shortenLimit, int lockheadMulti, double lockheadConcCover)
        {
            var res = db.ARLockheadParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARLockheadParameter()
                {
                    IDProject = idProject,
                    ShortenLimit = shortenLimit,
                    LockheadMutiply = lockheadMulti,
                    LockheadConcreteCover = lockheadConcCover
                };
                db.ARLockheadParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.ShortenLimit = shortenLimit;
                obj.LockheadMutiply = lockheadMulti;
                obj.LockheadConcreteCover = lockheadConcCover;
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
    }
}
