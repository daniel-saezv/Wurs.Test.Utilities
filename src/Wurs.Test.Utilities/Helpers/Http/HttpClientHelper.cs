namespace Wurs.Test.Utilities.Helpers.Http;
internal static class HttpClientHelper
{
    public static HttpClient CreateClient(
    HttpMessageHandler handler,
    string address)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentException.ThrowIfNullOrWhiteSpace(address);

        return new HttpClient(handler)
        {
            BaseAddress = new Uri(address)
        };
    }

    public static HttpClient CreateClient(HttpMessageHandler handler, string address, Action<HttpClient> configureClient)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentException.ThrowIfNullOrWhiteSpace(address);
        ArgumentNullException.ThrowIfNull(configureClient);

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri(address)
        };
        configureClient(client);
        return client;
    }
}
