using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class ElementTypeProjectDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public ElementTypeProjectDao() { }
        public void Update(long idMark, int idElemType)
        {
            var res = db.ARElementTypeProjects.Where(x => x.IDMark == idMark);
            if (res.Count() == 0)
            {
                var obj = new ARElementTypeProject()
                {
                    IDMark = idMark,
                    IDElementType = idElemType
                };
                db.ARElementTypeProjects.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDElementType = idElemType;
            }
            db.SaveChanges();
        }
        public long GetId(long idMark)
        {
            var res = db.ARElementTypeProjects.Where(x => x.IDMark == idMark);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
