using AutoRebaring.Database.AutoRebaring.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Database.AutoRebaring.Dao
{
    public class MacAddressDao
    {
        AutoRebaringDbContext db = new AutoRebaringDbContext();
        public MacAddressDao() { }
        public void Update(string macAddress)
        {
            var res = db.ARMacAddresses.Where(x => x.MacAddress == macAddress);
            if (res.Count() == 0)
            {
                var obj = new ARMacAddress()
                {
                    MacAddress = macAddress,
                    CreateDate = DateTime.Now
                };
                db.ARMacAddresses.Add(obj);
            }
            db.SaveChanges();
        }
        public long GetId(string macAddress)
        {
            var res = db.ARMacAddresses.Where(x => x.MacAddress == macAddress);
            if (res.Count() == 0)
            {
                return -1;
            }
            return res.First().ID;
        }
    }
}
