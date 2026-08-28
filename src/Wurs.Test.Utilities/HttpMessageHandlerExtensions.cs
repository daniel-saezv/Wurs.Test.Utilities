namespace Wurs.Test.Utilities;

public static class HttpMessageHandlerExtensions
{
    public static HttpMessageHandlerContext SetupContext(this HttpMessageHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        return new HttpMessageHandlerContext(handler);
    }
}
