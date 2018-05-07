using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class WindowsNameDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public WindowsNameDao() { }
        public void Update(string windowName)
        {
            var res = db.ARWindowsNames.Where(x => x.Name == windowName);
            if (res.Count() == 0)
            {
                var obj = new ARWindowsName()
                {
                    Name = windowName,
                    CreateDate = DateTime.Now
                };
                db.ARWindowsNames.Add(obj);
            }
            db.SaveChanges();
        }
        public long GetId(string windowName)
        {
            var res = db.ARWindowsNames.Where(x => x.Name == windowName);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
