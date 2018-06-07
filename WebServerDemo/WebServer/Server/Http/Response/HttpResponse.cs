namespace WebServer.Server.Http.Response
{
    using System;
    using System.Text;
    using WebServer.Server.Common;
    using WebServer.Server.Contracts;
    using WebServer.Server.Enums;
    using WebServer.Server.Http.Contracts;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public HttpHeaderCollection Headers { get; set; }
        public HttpStatusCode StatusCode { get; protected set; }
        public override string ToString()
        {
            var statusCodeNumber = (int)this.StatusCode;

            var response = new StringBuilder();
            response.AppendLine($"HTTP/1.1 {statusCodeNumber} {this.StatusCodeMessage}");
            response.AppendLine(this.Headers.ToString());
            response.AppendLine();

            return response.ToString();
        }

        private string StatusCodeMessage => this.StatusCode.ToString();
    }
}
