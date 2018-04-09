using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class CoverParametersDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public CoverParametersDao() { }
        public void Update(long idProject, double concCover)
        {
            var res = db.ARCoverParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARCoverParameter()
                {
                    IDProject = idProject,
                    ConcreteCover = concCover
                };
                db.ARCoverParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.ConcreteCover = concCover;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject)
        {
            var res = db.ARCoverParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
