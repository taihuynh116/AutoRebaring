using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class EDParameterValueDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public EDParameterValueDao() { }
        public void Update(long idMark, long idParamType, double value)
        {
            var res = db.AREDParameterValues.Where(x => x.IDMark==idMark && x.IDParameterType== idParamType);
            if (res.Count() == 0)
            {
                var obj = new AREDParameterValue()
                {
                    IDParameterType= idParamType,
                    IDMark = idMark,
                    Value = value,
                    CreateDate = DateTime.Now

                };
                db.AREDParameterValues.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.Value = value;
            }
            db.SaveChanges();
        }
        public long GetId(long idMark, long idParamType)
        {
            var res = db.AREDParameterValues.Where(x => x.IDMark == idMark && x.IDParameterType == idParamType);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
