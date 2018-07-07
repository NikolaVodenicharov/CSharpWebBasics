namespace WebServer.Application.Controllers
{
    using WebServer.Application.Views.Home;
    using WebServer.Server.Enums;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            var response = new ViewResponse(HttpStatusCode.Ok, new IndexView());
            response.Cookies.Add("lang", "en");

            return response;
        }
    }
}
