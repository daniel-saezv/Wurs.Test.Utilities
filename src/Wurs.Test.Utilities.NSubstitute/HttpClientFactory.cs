using NSubstitute;
using Wurs.Test.Utilities.Helpers.Http;

namespace Wurs.Test.Utilities.NSubstitute;

public class HttpClientFactory : HttpClientFactoryBuilderBase<HttpClientFactory, IHttpClientFactory>
{
    private readonly IHttpClientFactory _factory;

    protected override IHttpClientFactory Factory => _factory;

    public HttpClientFactory()
    {
        _factory = Substitute.For<IHttpClientFactory>();
    }

    public override IHttpClientFactory Create()
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

        _factory.Received(expectedCalls).CreateClient(clientName);
        return this;
    }

    protected override void ConfigureClient(IHttpClientFactory factory, string clientName, HttpClient client)
    {
        factory.CreateClient(clientName).Returns(client);
    }
}
