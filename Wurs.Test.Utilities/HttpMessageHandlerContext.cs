using System.Net;
using System.Text.Json;

namespace Wurs.Test.Utilities;
public sealed class HttpMessageHandlerContext(HttpMessageHandler handler)
{
    public HttpMessageHandler Handler { get; } = handler;
    public Queue<object> Queue { get; } = new Queue<object>();

    public const string AddResponseMethodName = nameof(AddResponse);

    public static JsonSerializerOptions DefaultJsonSerializerOptions { get; set; } = new JsonSerializerOptions();

    public HttpMessageHandlerContext AddResponse(HttpResponseMessage response)
    {
        Queue.Enqueue(response);
        return this;
    }

    public HttpMessageHandlerContext AddResponse(HttpStatusCode statusCode)
    {
        return AddResponse(new HttpResponseMessage(statusCode));
    }

    public HttpMessageHandlerContext AddResponse(HttpStatusCode statusCode, HttpContent content)
    {
        return AddResponse(new HttpResponseMessage(statusCode) { Content = content });
    }

    public HttpMessageHandlerContext AddResponse<T>(T content, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        return content is Exception exception
            ? AddException(exception)
            : AddResponse(statusCode, new StringContent(JsonSerializer.Serialize(content, DefaultJsonSerializerOptions)));
    }

    private HttpMessageHandlerContext AddException(Exception exception)
    {
        Queue.Enqueue(exception);
        return this;
    }
}
