using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class AnchorParametersDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public AnchorParametersDao() { }
        public void Update(long idProject, int anchorMulti)
        {
            var res = db.ARAnchorParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARAnchorParameter()
                {
                    IDProject = idProject,
                    AnchorMultiply = anchorMulti,
                    CreateDate = DateTime.Now
                };
                db.ARAnchorParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.AnchorMultiply = anchorMulti;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject)
        {
            var res = db.ARAnchorParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARAnchorParameter GetAnchorParameter(long id)
        {
            var res = db.ARAnchorParameters.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
