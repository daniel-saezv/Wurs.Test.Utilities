using Moq;
using Wurs.Test.Utilities.Helpers.Http;

namespace Wurs.Test.Utilities.Moq.Behaviour;

public class HttpClientFactoryMockBuilder : HttpClientFactoryBuilderBase<Mock<IHttpClientFactory>>
{
    private readonly Mock<IHttpClientFactory> _factory;

    public HttpClientFactoryMockBuilder()
    {
        _factory = new Mock<IHttpClientFactory>();
    }

    public override Mock<IHttpClientFactory> Create()
    {
        return _factory;
    }

    public HttpClientFactoryMockBuilder AddClient(
        HttpMessageHandler handler,
        string clientName = "default")
    {
        AddClient(_factory, handler, clientName);
        return this;
    }

    public HttpClientFactoryMockBuilder AddClient(
        HttpMessageHandler handler,
        string address,
        string clientName = "default")
    {
        AddClient(_factory, handler, address, clientName);
        return this;
    }

    public HttpClientFactoryMockBuilder AddClient(
        HttpMessageHandler handler,
        string address,
        Action<HttpClient> configureClient,
        string clientName = "default")
    {
        AddClient(_factory, handler, address, configureClient, clientName);
        return this;
    }

    protected override void ConfigureClient(Mock<IHttpClientFactory> factory, string clientName, HttpClient client)
    {
        factory.Setup(cf => cf.CreateClient(clientName)).Returns(client);
    }
}
