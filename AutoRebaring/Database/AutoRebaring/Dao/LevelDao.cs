using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class LevelDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public LevelDao() { }
        public void Update(long idProject, string name, string title, double elevation)
        {
            var res = db.ARLevels.Where(x => x.IDProject == idProject && x.Name == name);
            if (res.Count() == 0)
            {
                var obj = new ARLevel()
                {
                    IDProject = idProject,
                    Name = name,
                    Title = title,
                    Elevation = elevation,
                    CreateDate = DateTime.Now
                };
                db.ARLevels.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Title = title;
                obj.Elevation = elevation;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject, string name)
        {
            var res = db.ARLevels.Where(x => x.IDProject == idProject && x.Name == name);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
