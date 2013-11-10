using log4net;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ip_notifier.Utils
{
    class FirewallUtil
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FirewallUtil));

        private static INetFwMgr WinFirewallManager()
        {
            Type type = Type.GetTypeFromCLSID(new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
            return Activator.CreateInstance(type) as INetFwMgr;
        }

        public static bool AuthorizeProgram(string title, string path, NET_FW_SCOPE_ scope, NET_FW_IP_VERSION_ ipver)
        {
            try
            {
                log.Debug(string.Format("Adding firewall exception for: {0} [{1}]", title, path));
                Type type = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication");
                INetFwAuthorizedApplication authapp = Activator.CreateInstance(type) as INetFwAuthorizedApplication;
                authapp.Name = title;
                authapp.ProcessImageFileName = path;
                authapp.Scope = scope;
                authapp.IpVersion = ipver;
                authapp.Enabled = true;
                INetFwMgr mgr = WinFirewallManager();
                mgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(authapp);
                log.Debug("Firewall exception added properly!");
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unhandled exception when trying to add entry to Window's Firewall", ex);
                return false;
            }
        }
    }
}
