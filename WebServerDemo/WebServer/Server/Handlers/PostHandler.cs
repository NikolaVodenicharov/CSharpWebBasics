namespace WebServer.Server.Handlers
{
    using System;
    using WebServer.Server.Http.Contracts;

    public class PostHandler : RequestHandler
    {
        public PostHandler(Func<IHttpRequest, IHttpResponse> handingFunc) 
            : base(handingFunc)
        {
        }
    }
}
