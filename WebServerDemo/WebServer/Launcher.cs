namespace WebServer
{
    using WebServer.Application;
    using WebServer.Server;
    using WebServer.Server.Contracts;
    using WebServer.Server.Routing;

    public class Launcher : IRunnable
    {
        public static void Main(string[] args)
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var mainApplication = new MainApplication();
            var appRouteConfig = new AppRouteConfig();
            mainApplication.Configure(appRouteConfig);

            var webServerClass = new WebServerClass(1337, appRouteConfig);

            webServerClass.Run();
        }
    }
}
