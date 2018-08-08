namespace SimpleMvc.Framework.ActionResults
{
    using SimpleMvc.Framework.Contracts;
    using System;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class RedirectResult : IRedirectable
    {
        public RedirectResult(string redirectUrl)
        {
            this.RedirectUrl = redirectUrl;
        }

        public string RedirectUrl { get; set; }

        public IHttpResponse Invoke()
        {
            return new RedirectResponse(this.RedirectUrl);
        }
    }
}
