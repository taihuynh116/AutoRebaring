using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class LevelDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public LevelDao() { }
        public void Update(long idProject, string name, double elevation, string title= null)
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
                if (title != null)
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
        public List<ARLevel> GetLevels(long idProject)
        {
            var res = db.ARLevels.Where(x => x.IDProject == idProject);
            if (res.Count() == 0) return new List<ARLevel>();
            return res.ToList();
        }
        public ARLevel GetLevel(long id)
        {
            var res = db.ARLevels.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
        public string GetTitleLevel(string level)
        {
            var res = db.ARLevels.Where(x => x.Name == level);
            if (res.Count() == 0) return "";
            return res.First().Title;
        }
    }
}
