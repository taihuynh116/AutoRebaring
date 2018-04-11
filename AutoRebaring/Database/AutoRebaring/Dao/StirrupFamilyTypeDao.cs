using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StirrupFamilyTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StirrupFamilyTypeDao() { }
        public void Update(long idProject, long idRebarDesType, long idStirFamName)
        {
            var res = db.ARStirrupFamilyTypes.Where(x=> x.IDProject == idProject && x.IDRebarDesignType == idRebarDesType);
            if (res.Count() == 0)
            {
                var obj = new ARStirrupFamilyType()
                {
                    IDProject = idProject,
                    IDRebarDesignType = idRebarDesType,
                    IDStirrupFamilyName = idStirFamName,
                    CreateDate = DateTime.Now
                };
                db.ARStirrupFamilyTypes.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDStirrupFamilyName = idStirFamName;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject, long idRebarDesType)
        {
            var res = db.ARStirrupFamilyTypes.Where(x => x.IDProject == idProject && x.IDRebarDesignType == idRebarDesType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStirrupFamilyType GetStirrupFamilyType(long id)
        {
            var res = db.ARStirrupFamilyTypes.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
