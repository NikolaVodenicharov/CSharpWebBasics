namespace WebServer.Server.Routing.Contracts
{
    using System;
    using System.Collections.Generic;
    using WebServer.Server.Enums;
    using WebServer.Server.Handlers.Contracts;
    using WebServer.Server.Http.Contracts;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes { get; }

        void Get(string route, Func<IHttpRequest, IHttpResponse> handler);
        void Post(string route, Func<IHttpRequest, IHttpResponse> handler);
        void AddRoute(string route, HttpRequestMethod method, IRequestHandler httpHandler);
    }
}
