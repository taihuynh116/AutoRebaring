using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class RebarVericalParametersDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public RebarVericalParametersDao() { }
        public void Update(long idProject, long idRebarType, double botOff, double botOffRatio, double topOff, double topOffRatio, bool offInclude, bool offRaitoInclude, bool isInsideBeam)
        {
            var res = db.ARRebarVerticalParameters.Where(x => x.IDProject == idProject && x.IDRebarType== idRebarType);
            if (res.Count() == 0)
            {
                var obj = new ARRebarVerticalParameter()
                {
                    IDProject = idProject,
                    IDRebarType = idRebarType,
                    BottomOffset = botOff,
                    BottomOffsetRatio = botOffRatio,
                    TopOffset = topOff,
                    TopOffsetRatio = topOffRatio,
                    OffsetInclude = offInclude,
                    OffsetRatioInclude = offRaitoInclude,
                    IsInsideBeam = isInsideBeam
                };
                db.ARRebarVerticalParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.BottomOffset = botOff;
                obj.BottomOffsetRatio = botOffRatio;
                obj.TopOffset = topOff;
                obj.TopOffsetRatio = topOffRatio;
                obj.OffsetInclude = offInclude;
                obj.OffsetRatioInclude = offRaitoInclude;
                obj.IsInsideBeam = isInsideBeam;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject, long idRebarType)
        {
            var res = db.ARRebarVerticalParameters.Where(x => x.IDProject == idProject && x.IDRebarType == idRebarType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
