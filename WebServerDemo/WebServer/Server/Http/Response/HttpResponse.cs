namespace WebServer.Server.Http.Response
{
    using System;
    using System.Text;
    using WebServer.Server.Common;
    using WebServer.Server.Contracts;
    using WebServer.Server.Enums;

    public abstract class HttpResponse
    {
        private readonly IView view;

        private HttpResponse(HttpStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.StatusCode = statusCode;
        }
        protected HttpResponse(string redirectUrl)
            :this(HttpStatusCode.Found)
        {
            CoreValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));

            this.Headers.Add(new HttpHeader("Location", redirectUrl));
        }
        protected HttpResponse(HttpStatusCode responseCode, IView view)
            :this(responseCode)
        {
            this.view = view;
        }

        private HttpHeaderCollection Headers { get; set; }
        private HttpStatusCode StatusCode { get; set; }
        private string StatusCodeMessage => this.StatusCode.ToString();

        public override string ToString()
        {
            var response = new StringBuilder();

            response.Append($"HTTP/1.1 {this.StatusCode} {this.StatusCodeMessage}")

            return null;
        }
    }
}
