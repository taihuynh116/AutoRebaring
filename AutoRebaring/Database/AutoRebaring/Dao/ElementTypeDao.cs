using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class ElementTypeDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public ElementTypeDao() { }
        public long GetId(string type)
        {
            var res = db.ARElementTypes.Where(x => x.Type == type);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public string GetElementType(long id)
        {
            var res = db.ARElementTypes.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First().Type;
        }
    }
}
