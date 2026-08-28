using System.Net;
using System.Text;

namespace Wurs.Test.Utilities.ManualTests;

public abstract class HttpMessageHandlerFrameworkTestsBase<THandler>
{
    protected readonly THandler Handler;
    public required TestContext TestContext { get; init; }

    protected HttpMessageHandlerFrameworkTestsBase()
    {
        Handler = CreateHandler();
    }

    protected abstract THandler CreateHandler();
    protected abstract HttpMessageHandler ExtractMessageHandler(THandler handler);
    protected abstract HttpMessageHandlerContext SetupContext(THandler handler);
    protected abstract HttpClient CreateConfiguredClient(THandler handler, string clientName);
    protected abstract string ClientName { get; }
    protected abstract string RequestPath { get; }
    protected abstract string ExpectedBaseAddress { get; }
    protected abstract string ExpectedSource { get; }

    [TestMethod]
    public async Task Creates_Configured_Client_And_Uses_SetupContext_Response()
    {
        SetupContext(Handler).AddResponse(HttpStatusCode.OK, JsonContent($"{{\"source\":\"{ExpectedSource}\"}}"));

        var client = CreateConfiguredClient(Handler, ClientName);
        var response = await client.GetAsync(RequestPath, TestContext.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.CancellationToken);

        Assert.AreEqual(ExpectedBaseAddress, client.BaseAddress?.ToString());
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual($"{{\"source\":\"{ExpectedSource}\"}}", content);
    }

    [TestMethod]
    public async Task SetupContext_Conditional_Responses_Can_Be_Chained()
    {
        SetupContext(Handler)
            .When(r => r.Method == HttpMethod.Get && r.RequestUri?.AbsolutePath == "/a")
            .AddResponse(HttpStatusCode.OK, JsonContent("{\"value\":\"A\"}"))
            .When(r => r.Method == HttpMethod.Get && r.RequestUri?.AbsolutePath == "/b")
            .AddResponse(HttpStatusCode.OK, JsonContent("{\"value\":\"B\"}"))
            .AddResponse(HttpStatusCode.ServiceUnavailable);

        using var client = new HttpClient(ExtractMessageHandler(Handler))
        {
            BaseAddress = new Uri("https://stub.url.com")
        };

        var responseA = await client.GetAsync("/a", TestContext.CancellationToken);
        var responseB = await client.GetAsync("/b", TestContext.CancellationToken);

        Assert.AreEqual(HttpStatusCode.OK, responseA.StatusCode);
        Assert.AreEqual(HttpStatusCode.OK, responseB.StatusCode);
    }

    private static StringContent JsonContent(string json)
    {
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
