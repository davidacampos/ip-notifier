using ip_notifier.Utils;
using log4net;
using System;
using System.Configuration;
using System.Net;
using System.ServiceProcess;
using System.Threading;

namespace ip_notifier
{
    public partial class IPNotifierService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(IPNotifierService));
        private Timer _timer;
        private string _currentIp = "127.0.0.1";

        public IPNotifierService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.Start();
        }

        protected override void OnStop()
        {
            _timer.Dispose();
            _timer = null;
        }

        public void Start()
        {
            _timer = new Timer(CheckIp, null, 0, 86400 * 1000); // Run every 24 hours
        }

        private void CheckIp(object unused)
        {
            try
            {
                log.Debug("Checking IP...");
                string newIp = IPUtil.GetIp();
                if (!_currentIp.Equals(newIp, StringComparison.OrdinalIgnoreCase))
                {
                    NotifyIpChanged(_currentIp, newIp);
                    _currentIp = newIp;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unhandled exception when checking the IP", ex);
            }
        }

        private void NotifyIpChanged(string oldIp, string newIp)
        {
            log.Debug(string.Format("Notifying that the IP has changed. OLD: {0} - NEW: {1}", oldIp, newIp));

            string ftpPath = ConfigurationManager.AppSettings["ftpPath"];
            string ftpUser = ConfigurationManager.AppSettings["ftpUser"];
            string ftpPwd = ConfigurationManager.AppSettings["ftpPwd"];

            try
            {
                FtpUtil.UploadText(ftpPath, ftpUser, ftpPwd, newIp);
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException(string.Format("Verify the ftpPath setting within appSettings section is correct; it commonly should start with \"ftp://\" - ftpPath: {0}", ftpPath), ex);
            }
            catch (WebException ex)
            {
                throw new ArgumentException(string.Format("Verify both ftpUser and ftpPwd settings within appSettings section are correct - ftpUser: {0} - ftpPwd: {0}", ftpUser, ftpPwd), ex);
            }
        }
    }
}
