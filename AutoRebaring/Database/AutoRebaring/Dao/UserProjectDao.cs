using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class UserProjectDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public UserProjectDao() { }
        public void Update(long idProject, long idUserType, long idUser, long idMacAddress)
        {
            var res = db.ARUserProjects.Where(x => x.IDProject == idProject && x.IDUserType== idUserType && x.IDUser == idUser && x.IDMacAddress == idMacAddress);
            if (res.Count() == 0)
            {
                ARUserProject up = new ARUserProject()
                {
                    IDProject = idProject,
                    IDUserType = idUserType,
                    IDUser = idUser,
                    IDMacAddress = idMacAddress
                };
                db.ARUserProjects.Add(up);
            }
            db.SaveChanges();
        }
        public long GetID(long idProject, long idUserType, long idUser, long idMacAddress)
        {
            var res = db.ARUserProjects.Where(x => x.IDProject == idProject && x.IDUserType == idUserType && x.IDUser == idUser && x.IDMacAddress == idMacAddress);
            if (res.Count() == 0) return -1;
            return res.First().ID;
        }
    }
}
