namespace Wurs.Test.Utilities.Helpers.Http;
internal static class HttpClientHelper
{
    public static HttpClient CreateClient(
    HttpMessageHandler handler,
    string address) => new(handler)
    {
        BaseAddress = new Uri(address)
    };

    public static HttpClient CreateClient(HttpMessageHandler handler, string address, Action<HttpClient> configureClient)
    {
        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri(address)
        };
        configureClient(client);
        return client;
    }
}
