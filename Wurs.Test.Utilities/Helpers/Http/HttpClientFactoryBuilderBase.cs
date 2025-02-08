namespace Wurs.Test.Utilities.Helpers.Http;
public abstract class HttpClientFactoryBuilderBase<TFactory>
{
    protected static readonly string DefaultBaseAddress = "https://stub.url.com";

    public abstract TFactory Create();

    public HttpClientFactoryBuilderBase<TFactory> AddClient(
        TFactory factory,
        HttpMessageHandler handler,
        string clientName = "default")
    {
        ConfigureClient(factory, clientName, HttpClientHelper.CreateClient(handler, DefaultBaseAddress));
        return this;
    }

    public HttpClientFactoryBuilderBase<TFactory> AddClient(
        TFactory factory,
        HttpMessageHandler handler,
        string address,
        string clientName = "default")
    {
        ConfigureClient(factory, clientName, HttpClientHelper.CreateClient(handler, address));
        return this;
    }

    public HttpClientFactoryBuilderBase<TFactory> AddClient(
        TFactory factory,
        HttpMessageHandler handler,
        string address,
        Action<HttpClient> configureClient,
        string clientName = "default")
    {
        ConfigureClient(factory, clientName, HttpClientHelper.CreateClient(handler, address, configureClient));
        return this;
    }

    protected abstract void ConfigureClient(TFactory factory, string clientName, HttpClient client);
}
