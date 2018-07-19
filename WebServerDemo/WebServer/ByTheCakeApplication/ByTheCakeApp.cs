namespace WebServer.ByTheCakeApplication
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using WebServer.ByTheCakeApplication.Controllers;
    using WebServer.ByTheCakeApplication.Data;
    using WebServer.ByTheCakeApplication.ViewModels.Account;
    using WebServer.ByTheCakeApplication.ViewModels.Products;
    using WebServer.Server.Contracts;
    using WebServer.Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new ByTheCakeDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get(
                "/",
                request => new HomeController().Index());

            appRouteConfig.Get(
                "/about",
                request => new HomeController().About());

            appRouteConfig.Get(
                "/add",
                request => new ProductsController().Add());

            appRouteConfig.Post(
                "/add",
                request => new ProductsController().Add(
                    new AddProductViewModel(
                        request.FormData["name"], 
                        decimal.Parse(request.FormData["price"]),
                        request.FormData["imageUrl"])));

            appRouteConfig.Get(
                "/search",
                request => new ProductsController().Search(request));

            appRouteConfig.Get(
                "/products/{(?<id>[0-9]+)}",
                request => new ProductsController().Details(int.Parse(request.UrlParameters["id"])));

            appRouteConfig.Get(
                "/register",
                request => new AccountController().Register());

            appRouteConfig.Post(
                "/register",
                request => new AccountController().Register(
                    request, 
                    new RegisterUserViewModel(
                        request.FormData["username"],
                        request.FormData["password"],
                        request.FormData["confirmPassword"])));

            appRouteConfig.Get(
                "/login",
                request => new AccountController().Login());

            appRouteConfig.Post(
                "/login",
                request => new AccountController().Login(
                    request,
                    new LoginViewModel(
                        request.FormData["username"],
                        request.FormData["password"])));

            appRouteConfig.Get(
                "/profile",
                request => new AccountController().Profile(request));

            appRouteConfig.Post(
                "/logout",
                request => new AccountController().Logout(request));

            appRouteConfig.Get(
                "/shopping/add/{(?<id>[0-9]+)}",
                request => new ShoppingController().AddToCard(request));

            appRouteConfig.Get(
                "/cart",
                request => new ShoppingController().ShowCart(request));

            appRouteConfig.Post(
                "/shopping/finish-order",
                request => new ShoppingController().FinishOrder(request));


        }
    }
}
