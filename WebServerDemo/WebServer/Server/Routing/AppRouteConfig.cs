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
            this.routes = InitializeRoutes();
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes => this.routes;
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
            this.AddRoute(route, new GetHandler(handler));
        }

        public void Post(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
            this.AddRoute(route, new PostHandler(handler));
        }

        public void AddRoute(string route, IRequestHandler handler)
        {
            var handlerName = handler.GetType().Name.ToLower();

            if (handlerName.Contains("get"))
            {
                this.routes[HttpRequestMethod.Get].Add(route, handler);
            }
            else if (handlerName.Contains("post"))
            {
                this.routes[HttpRequestMethod.Post].Add(route, handler);
            }
            else
            {
                throw new InvalidOperationException("Invalid handler.");
            }
        }
    }
}
