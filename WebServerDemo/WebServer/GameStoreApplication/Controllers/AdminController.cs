namespace WebServer.GameStoreApplication.Controllers
{
    using System;
    using System.Linq;
    using WebServer.GameStoreApplication.Services;
    using WebServer.GameStoreApplication.Services.Contracts;
    using WebServer.GameStoreApplication.ViewModels.Admin;
    using WebServer.Server.Http.Contracts;

    public class AdminController : AbsractGameStoreController
    {
        private const string AddGamePath = @"admin\add-game";
        private const string ListGamesPath = @"admin\list-games";

        private readonly IGameService gameService;

        public AdminController(IHttpRequest request) 
            : base(request)
        {
            this.gameService = new GameService();
        }

        public IHttpResponse Add()
        {
            if (this.Authentication.IsAdmin)
            {
                return this.FileViewResponse(AddGamePath);
            }

            return this.HomeRedirect(); 
        }

        public IHttpResponse Add(AdminAddGameViewModel model)
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.HomeRedirect();
            }

            if (!this.ValidateModel(model))
            {
                return this.Add();
            }

            this.gameService.Create(
                model.Title,
                model.Description,
                model.ImageUrl,
                model.Price,
                model.SizeGigabytes,
                model.TrailerId,
                model.ReleaseDate.Value);

            return this.RedirectResponse(GameStoreApp.AdminAllGames);
        }

        public IHttpResponse List()
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.HomeRedirect();
            }

            var result = this.gameService
                .All()
                .Select(g => 
                $@"<tr>
                    <td>{g.Id}</td>
                    <td>{g.Title}</td>
                    <td>{g.Size:F2}</td>
                    <td>{g.Price:F2} &euro;</td>
                    <td>
                        <a class=""btn btn-warning"" href=""/admin/games/edit/{g.Id}"">Edit</a>
                        <a class=""btn btn-danger"" href=""/admin/games/delete/{g.Id}"">Delete</a>
                    </td>
                </tr>");

            var gamesAsHtml = string.Join(Environment.NewLine, result);
            this.ViewData["games"] = gamesAsHtml;

            return this.FileViewResponse(ListGamesPath);
        }
    }
}
