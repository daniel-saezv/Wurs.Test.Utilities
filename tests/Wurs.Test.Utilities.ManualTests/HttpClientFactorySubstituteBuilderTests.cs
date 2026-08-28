using Wurs.Test.Utilities.NSubstitute;

namespace Wurs.Test.Utilities.ManualTests;

[TestClass]
public sealed class HttpClientFactorySubstituteBuilderTests
{
    private readonly HttpMessageHandler _handler;

    public HttpClientFactorySubstituteBuilderTests()
    {
        _handler = new HttpClientHandler();
    }

    [TestMethod]
    public void Creates_Configured_Client()
    {
        var clientName = "inventory";
        var factory = new HttpClientFactorySubstituteBuilder()
            .AddClient(_handler, clientName)
            .Create();

        var client = factory.CreateClient(clientName);

        Assert.AreEqual("https://stub.url.com/", client.BaseAddress?.ToString());
    }
}
