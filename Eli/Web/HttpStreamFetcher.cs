using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace Eli.Web;

public class HttpStreamFetcher
{
    private IHttpClientFactory? HttpClientFactory { get; }
    private Lazy<HttpClient> HttpClient { get; } = new Lazy<HttpClient>(() => new HttpClient());
    private AsyncRetryPolicy<HttpResponseMessage> RetryPolicy { get; }

    private int DefaultMaxRequests { get; } = 3;
    private TimeSpan DefaultMaxRequestsTimeSpan { get; } = TimeSpan.FromSeconds(1);

    public HttpStreamFetcher(IHttpClientFactory? clientFactory = null, int? maxRequests = null, TimeSpan? retryWaitTime = null)
    {
        HttpClientFactory = clientFactory;

        if(maxRequests.HasValue && maxRequests <= 0) throw new ArgumentException("Max Requests must be a positive integer", nameof(maxRequests));
        if(retryWaitTime.HasValue && retryWaitTime.Value.TotalSeconds <= 0) throw new ArgumentException("Retry Wait Time must be a positive TimeSpan", nameof(retryWaitTime));

        var retryMaxRequests = maxRequests ?? DefaultMaxRequests - 1;
        var retryWaitDuration = retryWaitTime ?? DefaultMaxRequestsTimeSpan;

        RetryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(retryCount: retryMaxRequests,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * retryWaitDuration.TotalSeconds),
                onRetry: (response, timespan, retryCount, context) => { });
    }

    public Stream GetResponseStream(string url, int timeoutMilliseconds = 3000, string? userAgentValue = null)
    {
        try
        {
            var client = HttpClientFactory?.CreateClient() ?? HttpClient.Value;
            client.Timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if(userAgentValue is not null) request.Headers.Add("User-Agent", userAgentValue);

            var response = client.SendAsync(request).GetAwaiter().GetResult();

            var result = response.EnsureSuccessStatusCode();
            if(result.IsSuccessStatusCode) return response.Content.ReadAsStream();
            else return Stream.Null;
        }
        catch
        {
            return Stream.Null;
        }
    }

    public async Task<Stream> GetResponseStreamAsync(string url, int timeoutMilliseconds = 3000, string? userAgentStringValue = null)
    {
        try
        {
            var client = HttpClientFactory?.CreateClient() ?? HttpClient.Value;
            client.Timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if(userAgentStringValue is not null) request.Headers.Add("User-Agent", userAgentStringValue);

            var response = await RetryPolicy.ExecuteAsync(async () =>
            {
                var result = await client.SendAsync(request).ConfigureAwait(false);
                return result;
            }).ConfigureAwait(false);

            _ = response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
        catch
        {
            return Stream.Null;
        }
    }
}
