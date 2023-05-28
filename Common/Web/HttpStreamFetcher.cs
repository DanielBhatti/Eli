﻿using System;
using System.IO;
using System.Net;

namespace Common.Web;

public class HttpStreamFetcher
{
    public Stream GetResponseStream(string url, int timeout = 300)
    {
        var request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
        request.Timeout = timeout;
        var response = (HttpWebResponse)request.GetResponse();
        return response.GetResponseStream();
    }
}
