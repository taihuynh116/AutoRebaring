using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardChosenDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardChosenDao() { }
        public void Update(long idProject, double lmax, double lmin, double step, double limplantmax)
        {
            var res = db.ARStandardChosens.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARStandardChosen()
                {
                    IDProject = idProject,
                    Lmax = lmax,
                    Lmin =lmin,
                    Step = step,
                    LImplantMax = limplantmax,
                    CreateDate = DateTime.Now
                };
                db.ARStandardChosens.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Lmax = lmax;
                obj.Lmin = lmin;
                obj.Step = step;
                obj.LImplantMax = limplantmax;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject)
        {
            var res = db.ARStandardChosens.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStandardChosen GetStandardChosen(long id)
        {
            var res = db.ARStandardChosens.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
