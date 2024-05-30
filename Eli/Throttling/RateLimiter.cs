using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Eli.Throttling;

public class RateLimiter
{
    private ConcurrentQueue<DateTime> RequestTimes { get; } = new ConcurrentQueue<DateTime>();
    private SemaphoreSlim Semaphore { get; }

    private TimeSpan MaxRequestsTimeSpan { get; }
    private int MaxRequests { get; }
    private TimeSpan RetryWaitTime { get; }

    public RateLimiter(TimeSpan maxRequestsTimeSpan, int maxRequests, TimeSpan retryWaitTime)
    {
        if(maxRequests <= 0) throw new ArgumentException("Max requests must be greater than zero.", nameof(maxRequests));
        if(maxRequestsTimeSpan <= TimeSpan.Zero) throw new ArgumentException("Time span must be greater than zero.", nameof(maxRequestsTimeSpan));

        MaxRequests = maxRequests;
        MaxRequestsTimeSpan = maxRequestsTimeSpan;
        Semaphore = new(maxRequests, maxRequests);
        RetryWaitTime = retryWaitTime;
    }

    public async Task PerformActionAsync(Func<Task> action)
    {
        await WaitUntilAction();
        try
        {
            RequestTimes.Enqueue(DateTime.UtcNow);
            await action();
        }
        finally
        {
            _ = Semaphore.Release();
        }
    }

    public async Task<T> PerformActionAsync<T>(Func<Task<T>> action)
    {
        await WaitUntilAction();
        try
        {
            RequestTimes.Enqueue(DateTime.UtcNow);
            return await action();
        }
        finally
        {
            _ = Semaphore.Release();
        }
    }

    private async Task WaitUntilAction()
    {
        while(true)
        {
            await Semaphore.WaitAsync();
            CleanupOldRequests();
            if(RequestTimes.Count < MaxRequests) break;
            _ = Semaphore.Release();
            await Task.Delay(RetryWaitTime);
        }
    }

    private void CleanupOldRequests()
    {
        var cutoff = DateTime.UtcNow - MaxRequestsTimeSpan;
        while(RequestTimes.TryPeek(out var timestamp) && timestamp <= cutoff) _ = RequestTimes.TryDequeue(out _);
    }
}
