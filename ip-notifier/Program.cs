using ip_notifier.Utils;
using log4net;
using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;

namespace ip_notifier
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // log4net initialization
            log4net.Config.XmlConfigurator.Configure();

            if (Environment.UserInteractive)
            {
                log.Debug("Starting as an interactive app...");

                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        log.Debug("Installing as a service...");
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        FirewallUtil.AuthorizeProgram(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.Assembly.GetExecutingAssembly().Location, NetFwTypeLib.NET_FW_SCOPE_.NET_FW_SCOPE_ALL, NetFwTypeLib.NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY);
                        log.Debug("Service installed properly!");
                        break;
                    case "--uninstall":
                        log.Debug("Uninstalling service...");
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        log.Debug("Service uninstalled properly!");
                        break;
                    default:
                        IPNotifierService service = new IPNotifierService();
                        service.Start();
                        System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                        break;
                }
            }
            else
            {
                log.Debug("Starting as a service...");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new IPNotifierService() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

       
    }
}
