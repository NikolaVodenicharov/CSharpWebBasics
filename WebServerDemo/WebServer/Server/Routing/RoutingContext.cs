namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using WebServer.Server.Common;
    using WebServer.Server.Handlers.Contracts;
    using WebServer.Server.Routing.Contracts;

    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(IEnumerable<string> parameters, IRequestHandler handler)
        {
            CoreValidator.ThrowIfNull(parameters, nameof(parameters));
            this.Parameters = parameters;

            CoreValidator.ThrowIfNull(handler, nameof(handler));
            this.Handler = handler;
        }

        public IEnumerable<string> Parameters { get; private set; }
        public IRequestHandler Handler { get; private set; }
    }
}
