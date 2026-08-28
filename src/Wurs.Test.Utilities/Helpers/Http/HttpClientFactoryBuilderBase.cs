namespace Wurs.Test.Utilities.Helpers.Http;
public abstract class HttpClientFactoryBuilderBase<TBuilder, TFactory>
    where TBuilder : HttpClientFactoryBuilderBase<TBuilder, TFactory>
{
    protected static readonly string DefaultBaseAddress = "https://stub.url.com";

    protected abstract TFactory Factory { get; }

    public abstract TFactory Create();

    public TBuilder AddClient(
        HttpMessageHandler handler,
        string clientName = "default")
    {
        ConfigureClient(Factory, clientName, HttpClientHelper.CreateClient(handler, DefaultBaseAddress));
        return (TBuilder)this;
    }

    public TBuilder AddClient(
        HttpMessageHandler handler,
        string address,
        string clientName = "default")
    {
        ConfigureClient(Factory, clientName, HttpClientHelper.CreateClient(handler, address));
        return (TBuilder)this;
    }

    public TBuilder AddClient(
        HttpMessageHandler handler,
        string address,
        Action<HttpClient> configureClient,
        string clientName = "default")
    {
        ConfigureClient(Factory, clientName, HttpClientHelper.CreateClient(handler, address, configureClient));
        return (TBuilder)this;
    }

    protected abstract void ConfigureClient(TFactory factory, string clientName, HttpClient client);
}
