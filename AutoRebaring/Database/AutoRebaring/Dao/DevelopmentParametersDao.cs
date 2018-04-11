using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class DevelopmentParametersDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public DevelopmentParametersDao() { }
        public void Update(long idProject, int devMulti, double devLensDistance, double delDevErr, int numDevErr, bool devErrInclude, double devLevelOffAllowed, bool devLevelOffInclude, bool reinforceStirrInclude)
        {
            var res = db.ARDevelopmentParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARDevelopmentParameter()
                {
                    IDProject = idProject,
                    DevelopmentMultiply = devMulti,
                    DevelopmentLengthsDistance = devLensDistance,
                    DeltaDevelopmentError = delDevErr,
                    NumberDevelopmentError = numDevErr,
                    DevelopmentErrorInclude = devErrInclude,
                    DevelopmentLevelOffsetAllowed = devLevelOffAllowed,
                    DevelopmentLevelOffsetInclude = devLevelOffInclude,
                    ReinforcementStirrupInclude = reinforceStirrInclude,
                    CreateDate = DateTime.Now
                };
                db.ARDevelopmentParameters.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.DevelopmentMultiply = devMulti;
                obj.DevelopmentLengthsDistance = devLensDistance;
                obj.DeltaDevelopmentError = delDevErr;
                obj.NumberDevelopmentError = numDevErr;
                obj.DevelopmentErrorInclude = devErrInclude;
                obj.DevelopmentLevelOffsetAllowed = devLevelOffAllowed;
                obj.DevelopmentLevelOffsetInclude = devLevelOffInclude;
                obj.ReinforcementStirrupInclude = reinforceStirrInclude;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject)
        {
            var res = db.ARDevelopmentParameters.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
        public ARDevelopmentParameter GetDevelopmentParameter(long id)
        {
            var res = db.ARDevelopmentParameters.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
    }
}
