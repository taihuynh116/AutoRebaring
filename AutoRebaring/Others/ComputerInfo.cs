using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Others
{
    class ComputerInfo
    {
        public static string QueryComputerData()
        {
            string pTableName = "NetworkAdapterConfiguration";
            string pMethodName = "MacAddress";
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_" + pTableName);
            foreach (ManagementObject MO in MOS.Get())
            {
                try
                {
                    return MO[pMethodName].ToString();
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }
    }
}
