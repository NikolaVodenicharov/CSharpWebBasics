namespace WebServer.Server.Handlers.Contracts
{
    using WebServer.Server.Http.Contracts;

    public interface IRequestHandler
    {
        IHttpResponse Handle(IHttpContext context);
    }
}
