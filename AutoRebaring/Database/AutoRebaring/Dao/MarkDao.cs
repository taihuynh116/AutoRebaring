using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class MarkDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public MarkDao() { }
        public void Update( long idProject, string mark)
        {
            var res = db.ARMarks.Where(x => x.IDProject == idProject && x.Mark == mark);
            if (res.Count() == 0)
            {
                var obj = new ARMark()
                {
                    IDProject = idProject,
                    Mark = mark,
                    CreateDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };
                db.ARMarks.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.LastUpdate = DateTime.Now;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject, string mark)
        {
            var res = db.ARMarks.Where(x => x.IDProject == idProject && x.Mark == mark);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public List<string> GetAllMarkNames (long idProject)
        {
            var res = db.ARMarks.Where(x => x.IDProject == idProject);
            if (res.Count() == 0) return new List<string>();
            return res.OrderByDescending(x => x.LastUpdate).Select(x => x.Mark).ToList();
        }
        public string GetMarkName(long id)
        {
            var res = db.ARMarks.Where(x => x.ID == id);
            if (res.Count() == 0) return "";
            return res.First().Mark;
        }
    }
}
