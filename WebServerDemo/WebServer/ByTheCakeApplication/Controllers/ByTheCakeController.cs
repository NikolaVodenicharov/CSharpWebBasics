namespace WebServer.ByTheCakeApplication.Controllers
{
    using WebServer.Infrastructure;

    public abstract class ByTheCakeController : Controller
    {
        protected override string ApplicationDirectory => "ByTheCakeApplication";
    }
}
