using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    class UserProjectDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public UserProjectDao() { }
        public void Update(long idProject, long idUserType, long idUser, long idMacAddress)
        {
            var res = db.ARUserProjects.Where(x => x.IDProject == idProject && x.IDUser == idUser);
            if (res.Count() == 0)
            {
                var obj = new ARUserProject()
                {
                    IDProject = idProject,
                    IDUser = idUser,
                    IDUserType = idUserType,
                    IDMacAddress = idMacAddress,
                    IsActive = false
                };
                db.ARUserProjects.Add(obj);
            }
            else
            {
                var obj = res.First();
                if (obj.IDMacAddress != idMacAddress)
                {
                    obj.IDUserType = idUserType;
                    obj.IDMacAddress = idMacAddress;
                    obj.IsActive = false;
                }
            }
            db.SaveChanges();
        }
        public long GetId(long idProject, long idUser)
        {
            var res = db.ARUserProjects.Where(x => x.IDProject == idProject && x.IDUser == idUser);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
