﻿namespace WebServer.Server.Http.Response
{
    using WebServer.Server.Contracts;
    using WebServer.Server.Enums;
    using WebServer.Server.Exceptions;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode statusCode, IView view)
        {
            this.ValidateStatusCode(statusCode);

            this.StatusCode = statusCode;
            this.view = view;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {this.view.View()}";
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var statusCodeNumber = (int)statusCode;

            // is for redirect, not for VIewResponse
            if (299 < statusCodeNumber && statusCodeNumber < 400) // <= 400 ?
            {
                throw new InvalidResponseException("View responses need a status code below 300 or above 400.");
            }
        }
    }
}
