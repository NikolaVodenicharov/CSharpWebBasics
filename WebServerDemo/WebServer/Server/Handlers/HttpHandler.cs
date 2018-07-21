namespace WebServer.Server.Handlers
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using WebServer.Server.Common;
    using WebServer.Server.Handlers.Contracts;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;
    using WebServer.Server.Routing.Contracts;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig routeConfig)
        {
            CoreValidator.ThrowIfNull(routeConfig, nameof(routeConfig));
            this.serverRouteConfig = routeConfig;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            try
            {
                // check if user is authenticated
                var anonymousPaths = new[] { "/login", "/register" };

                bool containsPath = anonymousPaths.Contains(context.Request.Path);

                bool containsSession = false;               // not sure why we have that part with session contains.
                if (context.Request.Session != null)
                {
                    containsSession = context.Request.Session.Contains(SessionStore.CurrentUserKey);
                }

                if (!containsPath &&
                    !containsSession)
                {
                    return new RedirectResponse(anonymousPaths.First());
                }

                //if (!anonymousPaths.Contains(context.Request.Path) &&
                //    !context.Request.Session.Contains(SessionStore.CurrentUserKey))
                //{
                //    return new RedirectResponse(anonymousPaths.First());
                //}

                var requestMethod = context.Request.RequestMethod;
                var requestPath = context.Request.Path;
                var registeredRoutes = this.serverRouteConfig.Routes[requestMethod];

                foreach (var registeredRoute in registeredRoutes)
                {
                    var routePattern = registeredRoute.Key;
                    var routingContext = registeredRoute.Value;

                    var routeRegex = new Regex(routePattern);
                    var match = routeRegex.Match(requestPath);

                    if (!match.Success)
                    {
                        continue;
                    }

                    var parameters = routingContext.Parameters;

                    foreach (var parameter in parameters)
                    {
                        var parameterValue = match.Groups[parameter].Value;
                        context.Request.AddUrlParameter(parameter, parameterValue);
                    }

                    return routingContext.Handler.Handle(context);
                }
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResponse(ex);
            }

            return new NotFoundResponse();
        }
    }
}
