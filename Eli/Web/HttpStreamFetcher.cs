using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Eli.Web;

public class HttpStreamFetcher
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClient _client;

    public HttpStreamFetcher(IHttpClientFactory clientFactory = null)
    {
        _clientFactory = clientFactory;
        _client = clientFactory == null ? new HttpClient() : null;
    }

    public Stream GetResponseStream(string url, int timeoutMilliseconds = 3000)
    {
        try
        {
            var request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            request.Timeout = timeoutMilliseconds;
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }
        catch
        {
            return Stream.Null;
        }
    }

    public async Task<Stream> GetResponseStreamAsync(string url, int timeoutSeconds = 300)
    {
        try
        {
            var client = _clientFactory != null ? _clientFactory.CreateClient() : _client;
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var response = await client.GetAsync(url);
            _ = response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
        catch
        {
            return Stream.Null;
        }
    }
}
