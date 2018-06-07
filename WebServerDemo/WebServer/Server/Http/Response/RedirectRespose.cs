namespace WebServer.Server.Http.Response
{
    using System;
    using WebServer.Server.Common;
    using WebServer.Server.Enums;

    public class RedirectRespose : HttpResponse
    {
        public RedirectRespose(string redirectUrl) 
        {
            CoreValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));

            this.StatusCode = HttpStatusCode.Found;
            this.Headers.Add(new HttpHeader("Location", redirectUrl));
        }
    }
}
