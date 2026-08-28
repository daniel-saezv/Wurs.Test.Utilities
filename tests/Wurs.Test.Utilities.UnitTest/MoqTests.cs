using Moq;
using Wurs.Test.Utilities.Moq;

namespace Wurs.Test.Utilities.UnitTest;

[TestClass]
public sealed class MoqTests : HttpMessageHandlerFrameworkTestsBase<Mock<HttpMessageHandler>>
{
    private HttpClientFactory? _factory;

    protected override Mock<HttpMessageHandler> CreateHandler()
    {
        return new Mock<HttpMessageHandler>();
    }

    protected override HttpMessageHandler ExtractMessageHandler(Mock<HttpMessageHandler> handler)
    {
        return handler.Object;
    }

    protected override HttpMessageHandlerContext SetupContext(Mock<HttpMessageHandler> handler)
    {
        return handler.SetupContext();
    }

    protected override HttpClient CreateConfiguredClient(Mock<HttpMessageHandler> handler, string clientName)
    {
        _factory = new HttpClientFactory()
            .AddClient(handler.Object, "https://example.test", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(3);
            }, clientName);

        var builtFactory = _factory.Create();

        var client = builtFactory.Object.CreateClient(clientName);
        Assert.AreEqual(TimeSpan.FromSeconds(3), client.Timeout);
        return client;
    }

    protected override void VerifyConfiguredMocks()
    {
        this.Verify();
    }

    protected override string ClientName => "orders";
    protected override string RequestPath => "/orders";
    protected override string ExpectedBaseAddress => "https://example.test/";
    protected override string ExpectedSource => "moq";
}
