using Moq;
using Wurs.Test.Utilities.Helpers.Http;

namespace Wurs.Test.Utilities.Moq;

public class HttpClientFactory : HttpClientFactoryBuilderBase<HttpClientFactory, Mock<IHttpClientFactory>>
{
    private readonly Mock<IHttpClientFactory> _factory;

    protected override Mock<IHttpClientFactory> Factory => _factory;

    public HttpClientFactory()
    {
        _factory = new Mock<IHttpClientFactory>();
    }

    public override Mock<IHttpClientFactory> Create()
    {
        return _factory;
    }

    public HttpClientFactory VerifyClientRequested(string clientName, int expectedCalls = 1)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clientName);

        if (expectedCalls < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedCalls));
        }

        _factory.Verify(cf => cf.CreateClient(clientName), Times.Exactly(expectedCalls));
        return this;
    }

    public HttpClientFactory VerifyAllConfiguredClientsRequested()
    {
        _factory.VerifyAll();
        return this;
    }

    protected override void ConfigureClient(Mock<IHttpClientFactory> factory, string clientName, HttpClient client)
    {
        factory.Setup(cf => cf.CreateClient(clientName)).Returns(client).Verifiable();
    }
}
