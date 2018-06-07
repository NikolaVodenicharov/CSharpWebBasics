namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using WebServer.Server.Enums;
    using WebServer.Server.Handlers.Contracts;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes
        {
            get;
        }

        void AddRoute(string route, IRequestHandler httpHandler);
    }
}
