namespace WebServer.Application
{
    using WebServer.Application.Controllers;
    using WebServer.Server.Contracts;
    using WebServer.Server.Handlers;
    using WebServer.Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get(
                "/", 
                request => new HomeController().Index());

            appRouteConfig.Get(
                "testsession",
                request => new HomeController().SessionTest(request));
        }

    }
}
