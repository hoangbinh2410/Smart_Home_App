using System;
using System.Linq;
using System.Net;
using System.Reflection;

namespace BA_MobileGPS.Utilities.Helpers
{
    public static class DNSHelper
    {
        /// <summary>
        /// Thuc hien connect toi server
        /// </summary>
        /// <returns>The server.</returns>
        /// <param name="domainName">Domain name.</param>
        /// <param name="port">Port.</param>
        public static string GetIPAddressServer(string domainName, string port)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(domainName))
                {
                    var hostAdress = Dns.GetHostAddresses(domainName).FirstOrDefault();

                    if (hostAdress != null)
                    {
                        result = string.Format("http://{0}:{1}", hostAdress.ToString(), port);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }

            return result;
        }


        /// <summary>
        /// Thuc hien connect toi server
        /// </summary>
        /// <returns>The server.</returns>
        /// <param name="domainName">Domain name.</param>
        /// <param name="port">Port.</param>
        public static string GetDomainAddressServer(string domainName, string port)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(domainName))
                {
                    result = string.Format("{0}:{1}", domainName, port);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }

            return result;
        }
    }
}