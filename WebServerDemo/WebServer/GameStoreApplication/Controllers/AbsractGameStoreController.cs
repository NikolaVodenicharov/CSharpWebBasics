namespace WebServer.GameStoreApplication.Controllers
{
    using WebServer.GameStoreApplication.Common;
    using WebServer.GameStoreApplication.Services;
    using WebServer.GameStoreApplication.Services.Contracts;
    using WebServer.Infrastructure;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;

    public abstract class AbsractGameStoreController : Controller
    {
        private readonly IUserService userService;

        protected AbsractGameStoreController(IHttpRequest request)
        {
            this.Request = request;

            this.userService = new UserService();
            this.Authentication = new Authentication();
             
            this.ApplyAuthenticationViewData();
        }

        protected IHttpRequest Request { get; set; }
        protected Authentication Authentication { get; private set; }
        protected override string ApplicationDirectory => "GameStoreApplication";

        public void ApplyAuthenticationViewData()
        {
            var anonymousDisplay = HtmlFlex;
            var authDisplay = HtmlNone;
            var adminDisplay = HtmlNone;

            var authenticatedUserEmail = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);
            if (authenticatedUserEmail != null)
            {
                anonymousDisplay = HtmlNone;
                authDisplay = HtmlFlex;

                var isAdmin = this.userService.IsAdmin(authenticatedUserEmail);
                if (isAdmin)
                {
                    adminDisplay = HtmlFlex;
                }

                this.Authentication = new Authentication(true, isAdmin);
            }

            this.ViewData["anonymousDisplay"] = anonymousDisplay;
            this.ViewData["authDisplay"] = authDisplay;
            this.ViewData["adminDisplay"] = adminDisplay;
        }

        protected IHttpResponse HomeRedirect()
        {
            return this.RedirectResponse(GameStoreApp.HomeRoute);
        }
    }
}
