using WebServer.Server.Contracts;
using WebServer.Server.Enums;

namespace WebServer.Server.Http.Contracts
{
    public interface IHttpResponse
    {
        HttpHeaderCollection Headers { get; }
        HttpStatusCode StatusCode { get; }
    }
}
