namespace WebServer.ByTheCakeApplication
{
    using System;
    using WebServer.ByTheCakeApplication.Controllers;
    using WebServer.Server.Contracts;
    using WebServer.Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get(
                "/",
                req => new HomeController().Index());

            appRouteConfig.Get(
                "/about",
                request => new HomeController().About());

            appRouteConfig.Get(
                "/add",
                request => new CakesController().Add());

            appRouteConfig.Post(
                "/add",
                request => new CakesController().Add(
                    request.FormData["name"], 
                    request.FormData["price"]));

            appRouteConfig.Get(
                "/search",
                request => new CakesController().Search(request.UrlParameters));
        }
    }
}
