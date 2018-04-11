using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class OtherParameterDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public OtherParameterDao() { }
        public void Update(long idMark, long idView3d, bool view3dInclude, int partCount)
        {
            var res = db.AROtherParameters.Where(x => x.IDMark == idMark);
            if (res.Count() == 0)
            {
                var obj = new AROtherParameter()
                {
                    IDMark = idMark,
                    IDView3d = idView3d,
                    View3dInclude = view3dInclude,
                    PartCount = partCount,
                    CreateDate = DateTime.Now
                };
                db.AROtherParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDView3d = idView3d;
                obj.View3dInclude = view3dInclude;
                obj.PartCount = partCount;
            }
            db.SaveChanges();
        }
        public long GetId(long idMark)
        {
            var res = db.AROtherParameters.Where(x => x.IDMark == idMark);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public AROtherParameter GetOtherParameter(long id)
        {
            var res = db.AROtherParameters.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
