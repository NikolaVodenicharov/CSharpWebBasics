namespace WebServer.Server.Http
{
    using System;
    using WebServer.Server.Common;
    using WebServer.Server.Http.Contracts;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.request = new HttpRequest(requestString);
        }

        public IHttpRequest Request => this.request;
    }
}
