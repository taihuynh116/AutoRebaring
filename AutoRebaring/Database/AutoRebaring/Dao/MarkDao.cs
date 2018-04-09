using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class MarkDao
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
                    Mark = mark
                };
                db.ARMarks.Add(obj);
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
    }
}
