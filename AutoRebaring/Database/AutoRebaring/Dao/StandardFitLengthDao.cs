using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardFitLengthDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardFitLengthDao() { }
        public void Update(int numeric, long idStandardFitType, long idProject, double length)
        {
            var res = db.ARStandardFitLengths.Where(x => x.IDProject == idProject && x.Numeric == numeric && x.IDStandardFitType== idStandardFitType);
            if (res.Count() == 0)
            {
                var obj = new ARStandardFitLength()
                {
                    IDProject = idProject,
                    IDStandardFitType = idStandardFitType,
                    Numeric = numeric,
                    Length = length,
                    CreateDate = DateTime.Now
                };
                db.ARStandardFitLengths.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Length = length;
            }
            db.SaveChanges();
        }
        public long GetId(int numeric, long idStandardFitType, long idProject)
        {
            var res = db.ARStandardFitLengths.Where(x => x.IDProject == idProject && x.Numeric == numeric && x.IDStandardFitType == idStandardFitType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStandardFitLength GetStandardFitLength(long id)
        {
            var res = db.ARStandardFitLengths.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
