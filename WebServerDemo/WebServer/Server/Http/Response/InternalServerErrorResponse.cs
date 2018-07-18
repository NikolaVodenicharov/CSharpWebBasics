namespace WebServer.Server.Http.Response
{
    using System;
    using WebServer.Server.Common;
    using WebServer.Server.Enums;

    public class InternalServerErrorResponse : ViewResponse
    {
        public InternalServerErrorResponse(Exception ex)
            : base(HttpStatusCode.InternalServerError, new InternalServerErrorView(ex))
        {
            
        }
    }
}
