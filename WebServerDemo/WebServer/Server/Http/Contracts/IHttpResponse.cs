namespace WebServer.Server.Http.Contracts
{
    using WebServer.Server.Enums;

    public interface IHttpResponse
    {
        IHttpHeaderCollection Headers { get; }
        IHttpCookieCollection Cookies { get; }
        HttpStatusCode StatusCode { get; }
    }
}
