using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class View3dDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public View3dDao() { }
        public void Update(long idProject, string name)
        {
            var res = db.ARView3d.Where(x => x.IDProject == idProject && x.Name == name);
            if (res.Count() == 0)
            {
                var obj = new ARView3d()
                {
                    IDProject = idProject,
                    Name = name,
                    CreateDate = DateTime.Now
                };
                db.ARView3d.Add(obj);
            }
            db.SaveChanges();
        }
        public long GetId(long idProject, string name)
        {
            var res = db.ARView3d.Where(x => x.IDProject == idProject && x.Name == name);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARView3d GetView3d(long id)
        {
            var res = db.ARView3d.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
