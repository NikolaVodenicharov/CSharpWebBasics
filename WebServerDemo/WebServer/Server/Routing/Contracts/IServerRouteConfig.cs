namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using WebServer.Server.Enums;

    public interface IServerRouteConfig
    {
        IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes { get; }

        ICollection<string> AnonymousPaths { get; }
    }
}
