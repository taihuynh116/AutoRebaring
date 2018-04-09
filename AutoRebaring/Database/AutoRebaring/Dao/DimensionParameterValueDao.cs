using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class DimensionParameterValueDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public DimensionParameterValueDao() { }
        public void Update(long idProject, int idDimParamType)
        {
            var res = db.ARDimensionParameterValues.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                var obj = new ARDimensionParameterValue()
                {
                    IDProject = idProject,
                    IDDimensoinParameterType = idDimParamType
                };
                db.ARDimensionParameterValues.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDDimensoinParameterType = idDimParamType;
            }
            db.SaveChanges();
        }
        public long GetId(long idProject)
        {
            var res = db.ARDimensionParameterValues.Where(x => x.IDProject == idProject);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
