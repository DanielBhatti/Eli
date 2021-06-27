using System;
using System.IO;
using System.Net;

namespace Common.Web
{
    public class HttpStreamFetcher
    {
        public Stream GetResponseStream(string url, int timeout = 300000)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            }
            catch
            {
                throw;
            }
        }
    }
}
