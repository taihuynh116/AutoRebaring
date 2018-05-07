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
        public long GetId(string type)
        {
            var res = db.ARUserTypes.Where(x => x.Type == type);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARUserType GetUserType(long id)
        {
            var res = db.ARUserTypes.Where(x => x.ID == id);
            if (res.Count() == 0) return null;
            return res.First();
        }
    }
}
