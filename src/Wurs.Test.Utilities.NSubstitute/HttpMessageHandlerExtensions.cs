using NSubstitute;
using System.Reflection;
using Wurs.Test.Utilities.Constants.Reflection;
using Wurs.Test.Utilities.Helpers.Http;

namespace Wurs.Test.Utilities.NSubstitute;
internal static class HttpMessageHandlerExtensions
{
    private static readonly MethodInfo? SendAsyncMethod = typeof(HttpMessageHandler)
        .GetMethod(MethodNames.SendAsyncMethodName, BindingFlags.NonPublic | BindingFlags.Instance);

    internal static HttpMessageHandlerContext SetupContext(this HttpMessageHandler handler)
    {
        if (SendAsyncMethod is null)
        {
            throw new InvalidOperationException($"Critical error: {nameof(MethodNames.SendAsyncMethodName)} method not found. Contact with contributors on github");
        }

        var context = new HttpMessageHandlerContext(handler);

        SendAsyncMethod
            .Invoke(handler, [null, null])
            .ReturnsForAnyArgs(x =>
            {
                try
                {
                    return Task.FromResult(QueueResponseHelper.DequeueResponse(context, x.ArgAt<HttpRequestMessage>(0)));
                }
                catch (Exception ex)
                {
                    return Task.FromException<HttpResponseMessage>(ex);
                }
            });

        return context;
    }
}
