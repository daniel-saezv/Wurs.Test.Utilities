using System.Net;
using System.Text.Json;

namespace Wurs.Test.Utilities;

public sealed class HttpMessageHandlerContext
{
    private readonly Lock _syncRoot = new();
    private readonly List<HttpRequestRule> _rules = [];
    private readonly HttpRequestRule _defaultRule = new(static _ => true);
    private readonly DefaultRuleBuilder _defaultRuleBuilder;
    public static JsonSerializerOptions DefaultJsonSerializerOptions
    {
        get;
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    } = new();

    public HttpMessageHandlerContext(HttpMessageHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        _defaultRuleBuilder = new DefaultRuleBuilder(this, _defaultRule);
    }

    internal Lock SyncRoot => _syncRoot;

    internal IEnumerable<HttpRequestRule> GetResolutionRules()
    {
        foreach (var rule in _rules)
        {
            yield return rule;
        }

        yield return _defaultRule;
    }

    public HttpMessageHandlerContext AddResponse(HttpResponseMessage response)
    {
        return _defaultRuleBuilder.AddResponse(response);
    }

    public HttpMessageHandlerContext AddResponse(HttpStatusCode statusCode)
    {
        return _defaultRuleBuilder.AddResponse(statusCode);
    }

    public HttpMessageHandlerContext AddResponse(HttpStatusCode statusCode, HttpContent content)
    {
        return _defaultRuleBuilder.AddResponse(statusCode, content);
    }

    public HttpMessageHandlerContext AddResponse<T>(T content, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        return _defaultRuleBuilder.AddResponse(content, statusCode);
    }

    public HttpRequestRuleBuilder When(Func<HttpRequestMessage, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        var rule = new HttpRequestRule(condition);

        lock (_syncRoot)
        {
            _rules.Add(rule);
        }

        return new HttpRequestRuleBuilder(this, rule);
    }

    private HttpMessageHandlerContext AddRuleResponse(HttpRequestRule rule, object response)
    {
        lock (_syncRoot)
        {
            rule.Responses.Enqueue(response);
        }

        return this;
    }

    public abstract class ResponseBuilderBase
    {
        private readonly HttpMessageHandlerContext _context;
        private readonly HttpRequestRule _rule;

        internal ResponseBuilderBase(HttpMessageHandlerContext context, HttpRequestRule rule)
        {
            _context = context;
            _rule = rule;
        }

        public HttpMessageHandlerContext AddResponse(HttpResponseMessage response)
        {
            return _context.AddRuleResponse(_rule, response);
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
            return _context.AddRuleResponse(_rule, exception);
        }
    }

    public sealed class HttpRequestRuleBuilder : ResponseBuilderBase
    {
        internal HttpRequestRuleBuilder(HttpMessageHandlerContext context, HttpRequestRule rule)
            : base(context, rule)
        {
        }
    }

    private sealed class DefaultRuleBuilder : ResponseBuilderBase
    {
        internal DefaultRuleBuilder(HttpMessageHandlerContext context, HttpRequestRule rule)
            : base(context, rule)
        {
        }
    }

    internal sealed class HttpRequestRule(Func<HttpRequestMessage, bool> condition)
    {
        internal Func<HttpRequestMessage, bool> Condition { get; } = condition;
        internal Queue<object> Responses { get; } = new();
    }
}
