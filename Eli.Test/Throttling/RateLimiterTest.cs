using Eli.Throttling;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eli.Test.Throttling;

[TestFixture]
public class RateLimiterTest
{
    [Test]
    public async Task RateLimiterRespectsMaxRequests()
    {
        var rateLimiter = new RateLimiter(TimeSpan.FromSeconds(1), 3, TimeSpan.FromMilliseconds(100));
        var counter = 0;

        async Task TestAction()
        {
            counter++;
            await Task.Delay(50);
        }

        var tasks = Enumerable.Range(0, 5).Select(_ => rateLimiter.PerformActionAsync(TestAction)).ToArray();
        await Task.WhenAll(tasks);

        ClassicAssert.AreEqual(5, counter);
    }

    [Test]
    public async Task RateLimiterExecutesAllRequests()
    {
        var rateLimiter = new RateLimiter(TimeSpan.FromSeconds(1), 2, TimeSpan.FromMilliseconds(100));
        var results = new List<int>();
        var lockObject = new object();

        async Task TestAction(int i)
        {
            lock(lockObject)
            {
                results.Add(i);
            }
            await Task.Delay(50);
        }

        var tasks = Enumerable.Range(0, 5).Select(i => rateLimiter.PerformActionAsync(() => TestAction(i))).ToArray();
        await Task.WhenAll(tasks);

        ClassicAssert.AreEqual(5, results.Count);
        CollectionAssert.AreEquivalent(new[] { 0, 1, 2, 3, 4 }, results);
    }

    [Test]
    public async Task RateLimiterHandlesConcurrentRequests()
    {
        var rateLimiter = new RateLimiter(TimeSpan.FromSeconds(1), 2, TimeSpan.FromMilliseconds(100));
        var counter = 0;

        async Task TestAction()
        {
            counter++;
            await Task.Delay(50);
        }

        var tasks = Enumerable.Range(0, 10).Select(_ => rateLimiter.PerformActionAsync(TestAction)).ToArray();
        await Task.WhenAll(tasks);

        ClassicAssert.AreEqual(10, counter);
    }
}