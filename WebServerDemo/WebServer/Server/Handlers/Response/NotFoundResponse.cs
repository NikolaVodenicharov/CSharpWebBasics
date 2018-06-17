namespace WebServer.Server.Handlers.Response
{
    using WebServer.Server.Enums;
    using WebServer.Server.Http.Response;

    public class NotFoundResponse : HttpResponse
    {
        public NotFoundResponse()
        {
            this.StatusCode = HttpStatusCode.NotFound;
        }
    }
}
