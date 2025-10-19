using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Eli.Web.Tests;

[TestFixture]
public class HttpStreamFetcherTests
{
    private Mock<IHttpClientFactory> HttpClientFactoryMock { get; set; }
    private Mock<HttpMessageHandler> HttpMessageHandlerMock { get; set; }
    private HttpClient HttpClient { get; set; }
    private HttpStreamFetcher HttpStreamFetcher { get; set; }

    [SetUp]
    public void SetUp()
    {
        HttpClientFactoryMock = new Mock<IHttpClientFactory>();
        HttpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        HttpClient = new HttpClient(HttpMessageHandlerMock.Object);

        _ = HttpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(HttpClient);
        HttpStreamFetcher = new HttpStreamFetcher(HttpClientFactoryMock.Object);
    }

    [Test]
    public async Task GetResponseStreamAsyncShouldReturnStreamWhenResponseIsSuccessful()
    {
        var content = new StringContent("test");
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = content
        };

        _ = HttpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var stream = await HttpStreamFetcher.GetResponseStreamAsync("https://example.com");
        Assert.IsNotNull(stream);
        Assert.IsInstanceOf<Stream>(stream);
    }

    [Test]
    public async Task GetResponseStreamAsyncShouldRetryOnFailure()
    {
        var failedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var successResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("test")
        };

        _ = HttpMessageHandlerMock.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(failedResponse)
            .ReturnsAsync(successResponse);

        var stream = await HttpStreamFetcher.GetResponseStreamAsync("https://example.com");
        Assert.IsNotNull(stream);
        Assert.IsInstanceOf<Stream>(stream);
    }

    [Test]
    public async Task GetResponseStreamAsyncShouldReturnNullOnException()
    {
        _ = HttpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException());

        var stream = await HttpStreamFetcher.GetResponseStreamAsync("https://example.com");
        Assert.AreEqual(Stream.Null, stream);
    }

    [Test]
    public void GetResponseStreamShouldReturnStreamWhenResponseIsSuccessful()
    {
        var content = new StringContent("test");
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = content
        };

        _ = HttpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var stream = HttpStreamFetcher.GetResponseStream("https://example.com");
        Assert.IsNotNull(stream);
        Assert.IsInstanceOf<Stream>(stream);
    }

    [Test]
    public void GetResponseStreamShouldReturnNullOnException()
    {
        _ = HttpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException());

        var stream = HttpStreamFetcher.GetResponseStream("https://example.com");
        Assert.AreEqual(Stream.Null, stream);
    }
}
