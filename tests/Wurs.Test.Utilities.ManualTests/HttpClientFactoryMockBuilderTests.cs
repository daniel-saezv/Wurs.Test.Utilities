using Wurs.Test.Utilities.Moq.Behaviour;

namespace Wurs.Test.Utilities.ManualTests;

[TestClass]
public sealed class HttpClientFactoryMockBuilderTests
{
    private readonly HttpMessageHandler _handler;

    public HttpClientFactoryMockBuilderTests()
    {
        _handler = new HttpClientHandler();
    }

    [TestMethod]
    public void Creates_Configured_Client()
    {
        var clientName = "orders";
        var factory = new HttpClientFactoryMockBuilder()
            .AddClient(_handler, "https://example.test", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(3);
            }, clientName)
            .Create();

        var client = factory.Object.CreateClient(clientName);

        Assert.AreEqual("https://example.test/", client.BaseAddress?.ToString());
        Assert.AreEqual(TimeSpan.FromSeconds(3), client.Timeout);
    }
}
