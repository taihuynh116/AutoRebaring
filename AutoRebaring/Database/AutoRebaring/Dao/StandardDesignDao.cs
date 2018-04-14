using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class StandardDesignDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public StandardDesignDao() { }
        public void Update(long idRebarDesType, long idRebarBarType, long idDesLevel)
        {
            var res = db.ARStandardDesigns.Where(x => x.IDDesignLevel == idDesLevel && x.IDRebarDesignType == idRebarDesType);
            if (res.Count() == 0)
            {
                var obj = new ARStandardDesign()
                {
                    IDDesignLevel = idDesLevel,
                    IDRebarDesignType = idRebarDesType,
                    IDRebarBarType = idRebarBarType,
                    CreateDate = DateTime.Now
                };
                db.ARStandardDesigns.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDRebarBarType = idRebarBarType;
            }
            db.SaveChanges();
        }
        public long GetId(long idRebarDesType, long idDesLevel)
        {
            var res = db.ARStandardDesigns.Where(x => x.IDDesignLevel == idDesLevel && x.IDRebarDesignType == idRebarDesType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARStandardDesign GetStandardDesign(long id)
        {
            var res = db.ARStandardDesigns.Where(x => x.ID==id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
