using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ip_notifier.Utils
{
    class IPUtil
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(IPUtil));

        public static string GetIp()
        {
            try
            {
                log.Debug("Getting current IP...");
                var request = HttpWebRequest.Create("http://checkip.dyndns.org/");
                request.Method = "GET";

                var response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string answer = ExtractIp(reader.ReadToEnd());
                    log.Debug("Extracted IP: " + answer);
                    return answer;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unhandled exception when trying to get the current IP", ex);
                return null;
            }
        }

        private static string ExtractIp(string answer)
        {
            log.Debug("Extracting IP...");
            var pattern = "Current IP Address: ";
            var idx = answer.IndexOf(pattern);
            if (idx == -1) return null;
            var start = idx + pattern.Length;
            var end = answer.IndexOf("<", start);
            if (end == -1) return null;
            return answer.Substring(start, end - start);
        }
    }
}
