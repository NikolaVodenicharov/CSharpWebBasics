namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.IO;
    using WebServer.ByTheCakeApplication.Infrastructure;
    using WebServer.ByTheCakeApplication.Views;
    using WebServer.Server.Enums;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class HomeController : Controller
    {
        public IHttpResponse Index()
        {
            return this.FileViewResponse(@"Home\Index");
        }

        public IHttpResponse About()
        {
            return this.FileViewResponse(@"Home\About");
        }
    }
}
