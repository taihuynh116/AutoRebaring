using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StirrupFamilyNameDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StirrupFamilyNameDao() { }
        public void Update(string name)
        {
            var res = db.ARStirrupFamilyNames.Where(x => x.Name == name);
            if (res.Count() == 0)
            {
                var obj = new ARStirrupFamilyName()
                {
                    Name = name,
                    CreateDate = DateTime.Now
                };
                db.ARStirrupFamilyNames.Add(obj);
            }
            db.SaveChanges();
        }
        public long GetId(string name)
        {
            var res = db.ARStirrupFamilyNames.Where(x => x.Name == name);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStirrupFamilyName GetStirrupFamilyname(long id)
        {
            var res = db.ARStirrupFamilyNames.Where(x=> x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
