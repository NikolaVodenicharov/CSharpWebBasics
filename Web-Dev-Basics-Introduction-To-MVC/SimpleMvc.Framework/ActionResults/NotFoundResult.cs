namespace SimpleMvc.Framework.ActionResults
{
    using SimpleMvc.Framework.Contracts;
    using System;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class NotFoundResult : IActionResult
    {
        public IHttpResponse Invoke()
        {
            return new NotFoundResponse();
        }
    }
}
