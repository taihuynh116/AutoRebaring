using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class UserDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public UserDao() { }
        public void Update(string userName, string password)
        {
            var res = db.ARUsers.Where(x => x.Username == userName);
            if (res.Count() == 0)
            {
                ARUser u = new ARUser()
                {
                    Username = userName,
                    Password = password
                };
                db.ARUsers.Add(u);
            }
            else
            {
                ARUser user = res.First();
                user.Password = password;
            }
            db.SaveChanges();
        }
        public long GetID(string userName)
        {
            var res = db.ARUsers.Where(x => x.Username == userName);
            if (res.Count() == 0) return -1;
            return res.First().ID;
        }
    }
}
