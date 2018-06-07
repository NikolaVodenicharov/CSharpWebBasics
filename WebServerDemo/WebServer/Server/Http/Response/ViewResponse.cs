namespace WebServer.Server.Http.Response
{
    using System;
    using WebServer.Server.Contracts;
    using WebServer.Server.Enums;
    using WebServer.Server.Exceptions;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode responseCode, IView view)
        {

            this.StatusCode = responseCode;
            this.view = view;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {this.view.View()}";
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var statusCodeNumber = (int)statusCode;

            if (299 < statusCodeNumber && statusCodeNumber < 400) // <= 400 ?
            {
                throw new InvalidResponseException("View responses need a status code below 300 or above 400.");
            }
        }
    }
}
