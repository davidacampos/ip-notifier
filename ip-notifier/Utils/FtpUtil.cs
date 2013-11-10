using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ip_notifier.Utils
{
    class FtpUtil
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FtpUtil));

        public static void UploadText(string ftpPath, string ftpUser, string ftpPwd, string fileContent)
        {
            log.Debug(string.Format("Trying to upload file to FTP..."));

            WebRequest webRequest = WebRequest.Create(ftpPath);
            if (webRequest is FtpWebRequest)
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)webRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ftpUser, ftpPwd);

                byte[] fileContents = Encoding.UTF8.GetBytes(fileContent);
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                log.Debug(string.Format("File uploaded properly to: {0}", ftpPath));

            }
            else
            {
                throw new ArgumentException("The path to which the text file is supposed to be uploaded is NOT a proper FTP path.");
            }
        }
    }
}
