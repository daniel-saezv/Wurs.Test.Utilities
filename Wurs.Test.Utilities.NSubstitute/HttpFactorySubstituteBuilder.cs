using NSubstitute;
using System.Net.Http;
using Wurs.Test.Utilities.Helpers.Http;

namespace Wurs.Test.Utilities.NSubstitute;

public class HttpClientFactorySubstituteBuilder : HttpClientFactoryBuilderBase<IHttpClientFactory>
{
    private readonly IHttpClientFactory _factory;

    public HttpClientFactorySubstituteBuilder()
    {
        _factory = Substitute.For<IHttpClientFactory>();
    }

    public override IHttpClientFactory Create()
    {
        return _factory;
    }

    public HttpClientFactorySubstituteBuilder AddClient(
        HttpMessageHandler handler,
        string clientName = "default")
    {
        AddClient(_factory, handler, clientName);
        return this;
    }

    public HttpClientFactorySubstituteBuilder AddClient(
        HttpMessageHandler handler,
        string address,
        string clientName = "default")
    {
        AddClient(_factory, handler, address, clientName);
        return this;
    }

    public HttpClientFactorySubstituteBuilder AddClient(
        HttpMessageHandler handler,
        string address,
        Action<HttpClient> configureClient,
        string clientName = "default")
    {
        AddClient(_factory, handler, address, configureClient, clientName);
        return this;
    }

    protected override void ConfigureClient(IHttpClientFactory factory, string clientName, HttpClient client)
    {
        factory.CreateClient(clientName).Returns(client);
    }
}
