namespace WebServer.GameStoreApplication
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using WebServer.GameStoreApplication.Controllers;
    using WebServer.GameStoreApplication.Data;
    using WebServer.GameStoreApplication.ViewModels.Account;
    using WebServer.GameStoreApplication.ViewModels.Admin;
    using WebServer.Server.Contracts;
    using WebServer.Server.Routing.Contracts;

    public class GameStoreApp : IApplication
    {
        private const string AccountRegisterRoute = "/account/register";
        private const string AccountLoginRoute = "/account/login";
        public const string HomeRoute = "/";
        private const string AdminGamesAdd = "/admin/games/add";
        public const string AdminAllGames = "/admin/games/list";

        public void InitializeDatabase()
        {
            using (var db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add(HomeRoute);
            appRouteConfig.AnonymousPaths.Add(AccountRegisterRoute);
            appRouteConfig.AnonymousPaths.Add(AccountLoginRoute);

            appRouteConfig.Get(
                HomeRoute,
                request => new HomeController(request).Index());

            appRouteConfig.Get(
                AccountRegisterRoute,
                request => new AccountController(request).Register());

            appRouteConfig.Post(
                AccountRegisterRoute,
                request => new AccountController(request).Register(
                    new RegisterViewModel
                    {
                        Email = request.FormData["email"],
                        FullName = request.FormData["full-name"],
                        Password = request.FormData["password"],
                        ConfirmPassword = request.FormData["confirm-password"]
                    }));

            appRouteConfig.Get(
                AccountLoginRoute,
                request => new AccountController(request).Login());

            appRouteConfig.Post(
                AccountLoginRoute,
                request => new AccountController(request).Login(
                    new LoginViewModel
                    {
                        Email = request.FormData["email"],
                        Password = request.FormData["password"]
                    }));

            appRouteConfig.Get(
                "account/logout",
                request => new AccountController(request).Logout());

            appRouteConfig.Get(
                AdminGamesAdd,
                request => new AdminController(request).Add());

            appRouteConfig.Post(
                AdminGamesAdd,
                request => new AdminController(request).Add(
                    new AdminAddGameViewModel
                    {
                        Title = request.FormData["title"],
                        Description = request.FormData["description"],
                        ImageUrl = request.FormData["thumbnail"],
                        Price = decimal.Parse(request.FormData["price"]),
                        SizeGigabytes = double.Parse(request.FormData["size"]),
                        TrailerId = request.FormData["trailer-id"],
                        ReleaseDate = DateTime.ParseExact(
                            request.FormData["release-date"],
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture)
                    }));

            appRouteConfig.Get(
                AdminAllGames,
                request => new AdminController(request).List());
        }
    }
}
