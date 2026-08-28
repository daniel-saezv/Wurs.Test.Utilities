using NSubstitute;
using Wurs.Test.Utilities.NSubstitute;

namespace Wurs.Test.Utilities.UnitTest;

[TestClass]
public sealed class NSubstituteTests : HttpMessageHandlerFrameworkTestsBase<HttpMessageHandler>
{
    protected override HttpMessageHandler CreateHandler()
    {
        return Substitute.For<HttpMessageHandler>();
    }

    protected override HttpMessageHandler ExtractMessageHandler(HttpMessageHandler handler)
    {
        return handler;
    }

    protected override HttpMessageHandlerContext SetupContext(HttpMessageHandler handler)
    {
        return handler.SetupContext();
    }

    protected override HttpClient CreateConfiguredClient(HttpMessageHandler handler, string clientName)
    {
        var factory = new HttpClientFactory()
            .AddClient(handler, clientName)
            .Create();

        return factory.CreateClient(clientName);
    }

    protected override string ClientName => "inventory";
    protected override string RequestPath => "/inventory";
    protected override string ExpectedBaseAddress => "https://stub.url.com/";
    protected override string ExpectedSource => "nsubstitute";
}
