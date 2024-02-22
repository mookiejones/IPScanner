using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPScanner.Util
{
    internal static class NetworkInterfaceEx
    {
        public static bool IsValid(this NetworkInterface adapter)
        {
            var isUp = adapter.OperationalStatus == OperationalStatus.Up;
            if (!isUp) return false;

            var props = adapter.GetIPProperties();

            var result = props.UnicastAddresses.FirstOrDefault(o => o.Address.IsInternetWork());



            return result != null;


        }

        internal static bool IsInternetWork(this IPAddress address)
        {
            return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
        }


        internal static IEnumerable<NetworkInterface> WhereUp(this IEnumerable<NetworkInterface> interfaces)
            =>interfaces.Where(o=>o.OperationalStatus != OperationalStatus.Up);
        public static IEnumerable<NetworkInterface> WhereInternetwork(this IEnumerable<NetworkInterface> interfaces) 
            =>interfaces.Where(o=>o.GetIPProperties().UnicastAddresses.Any(a=>a.Address.AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork));
    }
}
