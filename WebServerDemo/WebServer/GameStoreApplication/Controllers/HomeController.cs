namespace WebServer.GameStoreApplication.Controllers
{
    using WebServer.Server.Http.Contracts;

    public class HomeController : AbsractGameStoreController
    {
        private const string HomePath = @"home\index";

        public HomeController(IHttpRequest request)
            :base(request)
        {

        }

        public IHttpResponse Index ()
        {
            return this.FileViewResponse(HomePath);
        }
    }
}
