using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace DeviceRegistration.Library
{
    public static class MacAddressAccessor
    {        
        public static string GetMacAddress()
        {
            var networks = NetworkInterface.GetAllNetworkInterfaces();
            var activeNetworks = networks.Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                                                      ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                                      !ni.Description.Contains("vethernet", StringComparison.InvariantCultureIgnoreCase) &&
                                                      !ni.Description.Contains("virtual", StringComparison.InvariantCultureIgnoreCase) &&
                                                      !ni.Description.Contains("pseudo", StringComparison.InvariantCultureIgnoreCase));
            var sortedNetworks = activeNetworks.OrderByDescending(ni => ni.Speed);
            return sortedNetworks?.First().GetPhysicalAddress().ToString();
        }

    }
}
