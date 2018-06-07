namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using WebServer.Server.Common;
    using WebServer.Server.Enums;
    using WebServer.Server.Routing.Contracts;

    public class ServerRouteConfig : IServerRouteConfig
    {
        private IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;
        //  private IAppRouteConfig appRouteConfig;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            CoreValidator.ThrowIfNull(appRouteConfig, nameof(appRouteConfig));

            this.routes = InitializeRoutes();
            this.InitializeRouteConfig(appRouteConfig);
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => throw new NotImplementedException();

        private IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> InitializeRoutes()
        {
            var initializedRoutes = 
                new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            var requestMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var requestMethod in requestMethods)
            {
                initializedRoutes[requestMethod] = new Dictionary<string, IRoutingContext>();
            }

            return initializedRoutes;
        }

        private void InitializeRouteConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var registeredRoute in appRouteConfig.Routes)
            {
                var requestMethod = registeredRoute.Key;
                var routesWithHandlers = registeredRoute.Value;

                foreach (var routeWithHandler in routesWithHandlers)
                {
                    var route = routeWithHandler.Key;
                    var handler = routeWithHandler.Value;

                    var parameters = new List<string>();
                    var parsedRouteRegex = this.ParseRoute(route, parameters);
                    var routingContext = new RoutingContext(parameters, handler);

                    this.routes[requestMethod].Add(parsedRouteRegex, routingContext);
                }
            }
        }
        private string ParseRoute(string route, List<string> parameters)
        {
            var result = new StringBuilder();
            result.Append("^");

            if (route == "/")
            {
                result.Append("/$");
                return result.ToString();
            }

            var tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            this.ParseTokens(tokens, parameters, result);

            return result.ToString();
        }

        private void ParseTokens(string[] tokens, List<string> parameters, StringBuilder result)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                var suffix = i == tokens.Length - 1 ? "$" : "/";
                var currentToken = tokens[i];

                if (!currentToken.StartsWith('{') && !currentToken.EndsWith('}'))
                {
                    result.Append($"{currentToken}{suffix}");
                }
            }
        }
    }
}
