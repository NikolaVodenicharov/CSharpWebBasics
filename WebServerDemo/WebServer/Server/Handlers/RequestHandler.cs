namespace WebServer.Server.Handlers
{
    using System;
    using Common;
    using Handlers.Contracts;
    using Http;
    using Http.Contracts;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handingFunc;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> handingFunc)
        {
            CoreValidator.ThrowIfNull(handingFunc, nameof(handingFunc));

            this.handingFunc = handingFunc;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            var response = this.handingFunc(context.Request);
            response.Headers.Add(new HttpHeader("Content-Type", "text/plain"));

            return response;
        }
    }
}
