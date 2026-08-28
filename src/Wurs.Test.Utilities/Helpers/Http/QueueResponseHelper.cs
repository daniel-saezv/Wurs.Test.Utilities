namespace Wurs.Test.Utilities.Helpers.Http;
internal static class QueueResponseHelper
{
    public static HttpResponseMessage DequeueResponse(HttpMessageHandlerContext context, HttpRequestMessage request)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);

        lock (context.SyncRoot)
        {
            foreach (var rule in context.GetResolutionRules())
            {
                if (!rule.Condition(request))
                {
                    continue;
                }

                if (rule.Responses.Count == 0)
                {
                    throw new InvalidOperationException("A matching condition was found but no configured responses are available for that condition.");
                }

                return ResolveRuleResponse(rule.Responses.Dequeue());
            }

            throw new InvalidOperationException("No configured responses available. Check your AddResponse calls.");
        }
    }

    private static HttpResponseMessage ResolveRuleResponse(object item)
    {
        return item switch
        {
            HttpResponseMessage response => response,
            Exception exception => throw exception,
            _ => throw new InvalidOperationException("Invalid response type. Check your AddResponse calls")
        };
    }
}
