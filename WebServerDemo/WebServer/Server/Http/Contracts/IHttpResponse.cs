namespace WebServer.Server.Http.Contracts
{
    using WebServer.Server.Enums;

    public interface IHttpResponse
    {
        HttpHeaderCollection Headers { get; }
        HttpStatusCode StatusCode { get; }
    }
}
