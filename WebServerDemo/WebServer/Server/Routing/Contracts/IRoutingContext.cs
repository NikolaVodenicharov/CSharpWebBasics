namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using WebServer.Server.Handlers.Contracts;

    public interface IRoutingContext
    {
        IEnumerable<string> Parameters { get; }
        IRequestHandler Handler { get; } 
    }
}
