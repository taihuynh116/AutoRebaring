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
        public void Update(long idProject, string name)
        {
            var res = db.ARProjects.Where(x => x.ID == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARProject()
                {
                    ID = idProject,
                    CreateDate = DateTime.Now,
                    Name = name
                };
                db.ARProjects.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Name = name;
            }
            db.SaveChanges();
        }
        public long GetId(string name)
        {
            var res = db.ARProjects.Where(x => x.Name == name);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
