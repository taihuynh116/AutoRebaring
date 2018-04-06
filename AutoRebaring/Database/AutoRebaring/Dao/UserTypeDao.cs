using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class UserTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public UserTypeDao() { }
        public long GetID(string userType)
        {
            var res = db.ARUserTypes.Where(x => x.Type == userType);
            if (res.Count() == 0) return -1;
            return res.First().ID;
        }
    }
}
