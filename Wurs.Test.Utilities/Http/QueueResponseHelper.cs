namespace Wurs.Test.Utilities.Http;
internal static class QueueResponseHelper
{
    public static HttpResponseMessage DequeueResponse(Queue<object> queue)
    {
        return queue.Dequeue() switch
        {
            HttpResponseMessage response => response,
            Exception exception => throw exception,
            _ => throw new InvalidOperationException($"Invalid response type. Check your {HttpMessageHandlerContext.AddResponseMethodName} calls")
        };
    }
}
