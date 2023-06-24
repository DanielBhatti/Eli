using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Web;

public class HttpStreamFetcher
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClient _client;

    public HttpStreamFetcher(IHttpClientFactory clientFactory = null)
    {
        _clientFactory = clientFactory;
        _client = clientFactory == null ? new HttpClient() : null;
    }

    public Stream GetResponseStream(string url, int timeout = 300)
    {
        var request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
        request.Timeout = timeout;
        var response = (HttpWebResponse)request.GetResponse();
        return response.GetResponseStream();
    }

    public async Task<Stream> GetResponseStreamAsync(string url, int timeout = 300)
    {
        var client = _clientFactory != null ? _clientFactory.CreateClient() : _client;
        client.Timeout = TimeSpan.FromSeconds(timeout);
        var response = await client.GetAsync(url);
        _ = response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync();
    }
}
