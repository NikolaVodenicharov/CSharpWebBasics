namespace WebServer.Server.Http.Response
{
    using System;
    using WebServer.Server.Common;
    using WebServer.Server.Enums;

    public class NotFoundResponse : ViewResponse
    {
        public NotFoundResponse()
            : base(HttpStatusCode.NotFound, new NotFoundView())
        {

        }
    }
}
