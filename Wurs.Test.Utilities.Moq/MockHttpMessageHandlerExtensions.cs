using Moq;
using Moq.Protected;
using Wurs.Test.Utilities.Constants.Reflection;
using Wurs.Test.Utilities.Http;
namespace Wurs.Test.Utilities.Moq;
internal static class MockHttpMessageHandlerExtensions
{
    internal static HttpMessageHandlerContext SetupContext(this Mock<HttpMessageHandler> mockHandler)
    {
        var context = new HttpMessageHandlerContext(mockHandler.Object);

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(MethodNames.SendAsyncMethodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
            {
                return QueueResponseHelper.DequeueResponse(context.Queue);
            });

        return context;
    }
}
