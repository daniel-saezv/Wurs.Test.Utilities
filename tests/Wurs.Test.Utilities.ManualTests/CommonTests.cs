using System.Net;
using System.Text;

namespace Wurs.Test.Utilities.ManualTests;

[TestClass]
public sealed class CommonTests
{
    private readonly HttpMessageHandler _handler;

    public CommonTests()
    {
        _handler = new HttpClientHandler();
    }

    [TestMethod]
    public void Mixed_Default_And_Conditional_Configuration_Can_Be_Chained()
    {
        var context = _handler.SetupContext()
                              .AddResponse(HttpStatusCode.Accepted)
                              .When(r => r.Method == HttpMethod.Get && r.RequestUri?.AbsolutePath == "/orders")
                              .AddResponse(HttpStatusCode.OK, JsonContent("{\"source\":\"conditional\"}"))
                              .AddResponse(new { source = "default-json" })
                              .When(r => r.Method == HttpMethod.Post && r.RequestUri?.AbsolutePath == "/orders")
                              .AddResponse(HttpStatusCode.Created);

        Assert.IsNotNull(context);
    }

    [TestMethod]
    public async Task Concurrent_Request_Scenario()
    {
        _handler.SetupContext()
                .When(r => r.Method == HttpMethod.Get && r.RequestUri?.AbsolutePath == "/a")
                .AddResponse(HttpStatusCode.OK, JsonContent("{\"value\":\"A\"}"))
                .When(r => r.Method == HttpMethod.Get && r.RequestUri?.AbsolutePath == "/b")
                .AddResponse(HttpStatusCode.OK, JsonContent("{\"value\":\"B\"}"))
                .AddResponse(HttpStatusCode.ServiceUnavailable);

        var simulatedCalls = new[]
        {
            Task.Run(() => new HttpRequestMessage(HttpMethod.Get, "https://stub.url.com/a")),
            Task.Run(() => new HttpRequestMessage(HttpMethod.Get, "https://stub.url.com/b"))
        };

        var requests = await Task.WhenAll(simulatedCalls);

        Assert.HasCount(2, requests);
    }

    private static StringContent JsonContent(string json)
    {
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
