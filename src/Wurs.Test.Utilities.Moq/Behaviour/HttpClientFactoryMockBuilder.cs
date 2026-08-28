using Moq;
using Wurs.Test.Utilities.Helpers.Http;

namespace Wurs.Test.Utilities.Moq.Behaviour;

public class HttpClientFactoryMockBuilder : HttpClientFactoryBuilderBase<HttpClientFactoryMockBuilder, Mock<IHttpClientFactory>>
{
    private readonly Mock<IHttpClientFactory> _factory;

    protected override Mock<IHttpClientFactory> Factory => _factory;

    public HttpClientFactoryMockBuilder()
    {
        _factory = new Mock<IHttpClientFactory>();
    }

    public override Mock<IHttpClientFactory> Create()
    {
        return _factory;
    }

    protected override void ConfigureClient(Mock<IHttpClientFactory> factory, string clientName, HttpClient client)
    {
        factory.Setup(cf => cf.CreateClient(clientName)).Returns(client);
    }
}
