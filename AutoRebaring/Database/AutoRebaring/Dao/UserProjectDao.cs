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
        public void Update(long idProject, long idUserType, long idUser, long idMacAddress, long idWindowsName)
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
                    IDWindowsName = idWindowsName,
                    IsActive = false,
                    CreateDate = DateTime.Now,
                    LastLogin = DateTime.Now
                };
                db.ARUserProjects.Add(obj);
            }
            else
            {
                var obj = res.First();
                obj.IDUserType = idUserType;
                obj.LastLogin = DateTime.Now;
                if (obj.IDMacAddress != idMacAddress || obj.IDWindowsName != idWindowsName)
                {
                    obj.IDMacAddress = idMacAddress;
                    obj.IDWindowsName = idWindowsName;
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
        public long GetId(long idProject, long idMacAddress, long idWindowsName)
        {
            var res = db.ARUserProjects.Where(x => x.IDProject == idProject && x.IDMacAddress == idMacAddress && x.IDWindowsName == idWindowsName);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.OrderByDescending(x=> x.LastLogin).First().ID;
        }
        public ARUserProject GetUserProject(long id)
        {
            var res = db.ARUserProjects.Where(x => x.ID == id);
            if (res.Count() == 0)
            {
                return null;
            }
            return res.First();
        }
        public int GetStatus(long id)
        {
            var res = db.ARUserProjects.Where(x => x.ID == id);
            if (res.Count() == 0) return -1;
            return res.First().IsActive ? 1 : 0;
        }
    }
}
