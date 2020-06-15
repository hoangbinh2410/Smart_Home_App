using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace BA_MobileGPS.Core
{
    public static class ServerStatusHelper
    {
        public static bool ServerStatusBy(string url)
        {
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                string result = client.DownloadString(url);
                if(result.Length>0)
                    return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }

        //public static bool ServerStatusBy(string url)
        //{

        //    try
        //    {
        //        url = url.Replace("http://", string.Empty);
        //        string[] item = url.Split(':');
        //        var hostUri = item[0].Trim();
        //        var portNumber = item[1].Trim();
        //        using (var client = new TcpClient(hostUri, int.Parse(portNumber)))
        //            return true;
        //    }
        //    catch (SocketException ex)
        //    {
        //        //MessageBox.Show("Error pinging host:'" + hostUri + ":" + portNumber.ToString() + "'");
        //        return false;
        //    }

        //}
    }
}