using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace WifiKey.Network
{
    public class WifiNetworkList : ObservableCollection<WifiNetwork>
    {
        public event EventHandler<string> OnNetworkNameFound;
        public event EventHandler<EventArgs> OnEndScanning;

        /// <summary>
        /// Tests whether the current user is a member of the Administrator's group.
        /// </summary>
        /// <returns>Returns TRUE if the user is a member of the Administrator's group; otherwise, FALSE.</returns>
        [DllImport("shell32.dll")]
        private static extern bool IsUserAnAdmin();

        private List<string> NetSh(string arguments)
        {
            var cmdUtility = new Process
            {
                StartInfo =
                {
                    FileName = "netsh",
                    Arguments = arguments,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            cmdUtility.Start();
            var output = new List<string>(Regex.Split(cmdUtility.StandardOutput.ReadToEnd(), "\r\n"));
            cmdUtility.WaitForExit();
            return output;
        }
        
        public void FillList()
        {
            foreach (var ssid in NetSh("wlan show all").Where(line => line.Contains("SSID name")))
            {
                var elements = ssid.Split(':');
                if (elements.Length != 2)
                    continue;

                var networkName = Regex.Replace(elements[1].Trim(), "^\"|\"$", "");
                OnNetworkNameFound?.Invoke(this, networkName);
                var buffer = NetSh($"wlan show profile name=\"{networkName}\" key=clear").FirstOrDefault(line => line.Contains("Key Content"))?.Split(':');
                var password = buffer?.Length != 2 ? string.Empty : buffer[1].Trim();

                var element = new WifiNetwork { Name = networkName, Password = password };
                Add(element);
            }

            OnEndScanning?.Invoke(this, EventArgs.Empty);
        }

        public WifiNetworkList()
        {
            if (!Debugger.IsAttached && !IsUserAnAdmin())
                throw new UnauthorizedAccessException("Program must be run with administrator privileges!");
        }

        public WifiNetworkList RemoveProfile(WifiNetwork profile)
        {
            if (profile == null)
                return this;

            var element = this.FirstOrDefault(e => e.Name.Equals(profile.Name, StringComparison.CurrentCultureIgnoreCase));

            if (element != null && !Debugger.IsAttached)
                NetSh($"wlan delete profile name=\"{element.Name}\"");
            Remove(element);

            return this;
        }
    }
}
