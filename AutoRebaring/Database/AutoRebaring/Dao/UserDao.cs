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
                var obj = new ARUser()
                {
                    Username = userName,
                    Password = password,
                    CreateDate = DateTime.Now
                };
                db.ARUsers.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Password = password;
            }
            db.SaveChanges();
        }
        public long GetId(string userName)
        {
            var res = db.ARUsers.Where(x => x.Username == userName);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARUser GetUser(long idUser)
        {
            var res = db.ARUsers.Where(x => x.ID == idUser);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
