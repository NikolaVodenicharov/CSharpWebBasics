namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.Server.Enums;
    using WebServer.Server.Handlers;
    using WebServer.Server.Handlers.Contracts;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Routing.Contracts;

    public class AppRouteConfig : IAppRouteConfig
    {
        private IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> routes;

        public AppRouteConfig()
        {
            this.AnonymousPaths = new List<string>();

            this.routes = InitializeRoutes();
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes => this.routes;

        public ICollection<string> AnonymousPaths { get; private set; }

        private Dictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> InitializeRoutes()
        {
            var initializedRoutes =
                new Dictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>>();

            var requestMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (HttpRequestMethod requestMethod in requestMethods)
            {
                initializedRoutes[requestMethod] = new Dictionary<string, IRequestHandler>();
            }

            return initializedRoutes;
        }

        public void Get(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
            this.AddRoute(route, HttpRequestMethod.Get, new RequestHandler(handler));
        }

        public void Post(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
            this.AddRoute(route, HttpRequestMethod.Post, new RequestHandler(handler));
        }

        public void AddRoute(string route, HttpRequestMethod method, IRequestHandler handler)
        {
            this.routes[method].Add(route, handler);
        }
    }
}
