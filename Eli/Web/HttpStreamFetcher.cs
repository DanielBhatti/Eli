using Eli.Throttling;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eli.Web;

public class HttpStreamFetcher
{
    private IHttpClientFactory HttpClientFactory { get; }
    private RateLimiter RateLimiter { get; }

    public HttpStreamFetcher(IHttpClientFactory clientFactory, TimeSpan? maxRequestsTimeSpan = null, int? maxRequests = null, TimeSpan? retryWaitTime = null)
    {
        HttpClientFactory = clientFactory;
        RateLimiter = new(maxRequestsTimeSpan ?? TimeSpan.FromSeconds(1), maxRequests ?? 5, retryWaitTime ?? TimeSpan.FromSeconds(1));
    }

    public async Task<Stream> GetResponseStreamAsync(string url, int timeoutMilliseconds = 3000)
    {
        try
        {
            var client = HttpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            
            var response = await RateLimiter.PerformActionAsync(async () => await client.GetAsync(url).ConfigureAwait(false));
            
            var result = response.EnsureSuccessStatusCode();
            if(result.IsSuccessStatusCode) return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            else return Stream.Null;
        }
        catch
        {
            return Stream.Null;
        }
    }
}
