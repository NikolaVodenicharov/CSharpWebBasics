namespace WebServer.Server.Http
{
    using WebServer.Server.Common;
    using WebServer.Server.Http.Contracts;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest httpRequest;

        public HttpContext(IHttpRequest httpRequest)
        {
            CoreValidator.ThrowIfNull(httpRequest, nameof(httpRequest));

            this.httpRequest = httpRequest;
        }

        public IHttpRequest Request => this.httpRequest;
    }
}
