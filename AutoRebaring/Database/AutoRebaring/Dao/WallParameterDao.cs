using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class WallParameterDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public WallParameterDao() { }
        public void Update(long idMark, double edgeWidth, bool edgeWidthInclude, double edgeRatio, bool edgeRatioInclude)
        {
            var res = db.ARWallParameters.Where(x => x.IDMark == idMark);
            if (res.Count() == 0)
            {
                var obj = new ARWallParameter()
                {
                    IDMark = idMark,
                    EdgeWidth = edgeWidth,
                    EdgeWidthInclude = edgeWidthInclude,
                    EdgeRatio = edgeRatio,
                    EdgeRatioInclude = edgeRatioInclude,
                    CreateDate = DateTime.Now
                };
                db.ARWallParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.EdgeWidth = edgeWidth;
                obj.EdgeWidthInclude = edgeWidthInclude;
                obj.EdgeRatio = edgeRatio;
                obj.EdgeRatioInclude = edgeRatioInclude;
            }
            db.SaveChanges();
        }
        public long GetId(long idMark)
        {
            var res = db.ARWallParameters.Where(x => x.IDMark == idMark);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARWallParameter GetWallParameter(long id)
        {
            var res = db.ARWallParameters.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
