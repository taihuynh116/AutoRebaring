using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class RebarBarTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public RebarBarTypeDao() { }
        public void Update(string type)
        {
            var res = db.ARRebarBarTypes.Where(x => x.Type == type);
            if (res.Count() == 0)
            {
                var obj = new ARRebarBarType()
                {
                    Type = type,
                    CreateDate = DateTime.Now
                };
                db.ARRebarBarTypes.Add(obj);
            }
            db.SaveChanges();
        }
        public long GetId(string type)
        {
            var res = db.ARRebarBarTypes.Where(x => x.Type == type);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARRebarBarType GetRebarBarType(long id)
        {
            var res = db.ARRebarBarTypes.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
