namespace WebServer.Application.Controllers
{
    using System;
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

        public IHttpResponse SessionTest(IHttpRequest request)
        {
            var session = request.Session;

            const string sessionDateKey = "saved_date";

            if (session.Get(sessionDateKey) == null)
            {
                session.Add(
                    sessionDateKey, 
                    DateTime.UtcNow);
            }

            return new ViewResponse(
                HttpStatusCode.Ok, 
                new SessionTestView(session.Get<DateTime>(sessionDateKey)));
        }
    }
}
