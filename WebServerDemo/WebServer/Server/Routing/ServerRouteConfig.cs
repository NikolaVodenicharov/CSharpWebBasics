namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
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

            this.AnonymousPaths = new List<string>(appRouteConfig.AnonymousPaths);

            this.routes = this.InitializeRoutes();
            this.InitializeRouteConfig(appRouteConfig);
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => this.routes;
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

        public ICollection<string> AnonymousPaths { get; private set; }

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
            result.Append("^/");

            if (route == "/")
            {
                result.Append("$");
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
                // block 1
                var isLastToken = i == (tokens.Length - 1);
                var suffix = isLastToken ? "$" : "/";

                var currentToken = tokens[i];

                if (!currentToken.StartsWith('{') && !currentToken.EndsWith('}'))
                {
                    result.Append($"{currentToken}{suffix}");
                    continue;
                }

                // block 2
                var parameterRegex = new Regex("<\\w+>");
                var parameterMatch = parameterRegex.Match(currentToken);

                if (!parameterMatch.Success)
                {
                    throw new InvalidOperationException($"Route parameter in '{currentToken}' is not valid.");
                }

                // block 3
                var match = parameterMatch.Value;  // .Groups[0].Value ?
                var parameter = match.Substring(1, match.Length - 2);
                parameters.Add(parameter);

                // block 4
                var currentTokenWithoutCurlyBrackets = currentToken.Substring(1, currentToken.Length - 2);
                result.Append($"{currentTokenWithoutCurlyBrackets}{suffix}");
            }
        }
    }
}
