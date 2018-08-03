namespace WebServer.GameStoreApplication.Controllers
{
    using WebServer.GameStoreApplication.Services;
    using WebServer.GameStoreApplication.Services.Contracts;
    using WebServer.GameStoreApplication.ViewModels.Account;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class AccountController : AbsractGameStoreController
    {
        private const string RegisterPath = @"account\register";
        private const string LoginPath = @"account\login";

        private const string InvalidUserDetailsMessage = "Invalid user details.";

        private readonly IUserService userService;

        public AccountController(IHttpRequest request)
            :base(request)
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
            return this.FileViewResponse(RegisterPath);
        }
        public IHttpResponse Register(RegisterViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Register();
            }

            var isSuccess = this.userService.Create(model.Email, model.FullName, model.Password);

            if (isSuccess)
            {
                this.LoginUser(model.Email);
                return this.HomeRedirect();
            }
            else
            {
                this.ShowErrorDiv("E-mail is taken.");
                return this.Register();
            }
        }

        public IHttpResponse Login()
        {
            return this.FileViewResponse(LoginPath);
        }
        public IHttpResponse Login(LoginViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Login();
            }

            var isSuccessful = this.userService.Find(model.Email, model.Password);

            if (isSuccessful)
            {
                this.LoginUser(model.Email);
                return this.HomeRedirect();
            }
            else
            {
                this.ShowErrorDiv(InvalidUserDetailsMessage);
                return this.Login();
            }
        }
        private void LoginUser(string email)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, email);
        }

        public IHttpResponse Logout()
        {
            this.Request.Session.Clear();
            return this.HomeRedirect();
        }
    }
}
