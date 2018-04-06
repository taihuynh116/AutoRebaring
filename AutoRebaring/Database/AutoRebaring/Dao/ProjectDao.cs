using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class ProjectDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public ProjectDao() { }
        public void Update(int idProject, string nameProject)
        {
            var res = db.ARProjects.Where(x => x.ID == idProject);
            if (res.Count() == 0)
            {
                ARProject p = new ARProject()
                {
                    ID = idProject,
                    Name = nameProject
                };
                db.ARProjects.Add(p);
            }
            db.SaveChanges();
        }
        public long GetID(string nameProject)
        {
            var res = db.ARProjects.Where(x => x.Name == nameProject);
            if (res.Count() == 0) return -1;
            return res.First().ID;
        }
    }
}
