namespace WebServer.Server.Http.Response
{
    using WebServer.Server.Enums;
    using WebServer.Server.Http.Contracts;

    public class BadRequestResponse : HttpResponse
    {
        public BadRequestResponse()
        {
            this.StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
