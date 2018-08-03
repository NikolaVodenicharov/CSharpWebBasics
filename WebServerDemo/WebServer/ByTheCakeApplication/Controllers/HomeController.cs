namespace WebServer.ByTheCakeApplication.Controllers
{
    using WebServer.Server.Http.Contracts;

    public class HomeController : ByTheCakeController
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
