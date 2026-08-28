using NSubstitute;
using Wurs.Test.Utilities.Helpers.Http;

namespace Wurs.Test.Utilities.NSubstitute;

public class HttpClientFactorySubstituteBuilder : HttpClientFactoryBuilderBase<HttpClientFactorySubstituteBuilder, IHttpClientFactory>
{
    private readonly IHttpClientFactory _factory;

    protected override IHttpClientFactory Factory => _factory;

    public HttpClientFactorySubstituteBuilder()
    {
        _factory = Substitute.For<IHttpClientFactory>();
    }

    public override IHttpClientFactory Create()
    {
        return _factory;
    }

    protected override void ConfigureClient(IHttpClientFactory factory, string clientName, HttpClient client)
    {
        factory.CreateClient(clientName).Returns(client);
    }
}
