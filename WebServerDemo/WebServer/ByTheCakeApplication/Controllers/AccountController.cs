namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using WebServer.ByTheCakeApplication.Services;
    using WebServer.ByTheCakeApplication.ViewModels;
    using WebServer.ByTheCakeApplication.ViewModels.Account;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class AccountController : ByTheCakeController
    {
        private const string PathAccountProfile = @"account\profile";

        private const string FormUsernameKey = "username";
        private const string FormPasswordKey = "password";

        private const string MessageEmptyField = "You have empty fields";
        private const string MessageUsernameIsBusy = "This username is taken.";

        private readonly IUserService userService;

        public AccountController()
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();
            return this.RegisterResponse();
        }
        public IHttpResponse Register(IHttpRequest request, RegisterUserViewModel model)
        {
            this.SetDefaultViewData();

            // Validate the model.
            if (model.Username.Length < 3 ||
                model.Password.Length < 3 ||
                model.ConfirmPassword != model.Password)
            {
                this.ShowErrorDiv(MessageInvalidUserDetails);

                return this.RegisterResponse();
            }

            var success = this.userService.Create(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ShowErrorDiv(MessageUsernameIsBusy);

                return this.RegisterResponse();
            }
        }
        private IHttpResponse RegisterResponse()
        {
            return FileViewResponse(@"account\register");
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();

            return this.LoginResponse();
        }
        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                this.ShowErrorDiv(MessageEmptyField);

                return this.LoginResponse();
            }

            var success = this.userService.Find(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ShowErrorDiv(MessageInvalidUserDetails);

                return this.LoginResponse();
            }
        }
        private IHttpResponse LoginResponse()
        {
            return FileViewResponse(@"account\login");
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            if (!request.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var profile = this.userService.Profile(username);

            if (profile == null)
            {
                throw new InvalidOperationException($"The user {username} could not be found in the database.");
            }

            this.ViewData["username"] = profile.Username;
            this.ViewData["registeredDate"] = profile.RegistrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = profile.TotalOrders.ToString();

            return this.FileViewResponse(PathAccountProfile);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData()
        {
            this.HideErrorDiv();
            this.ViewData[HtmlAuthenticateDisplay] = HtmlNone;
        }
        private void LoginUser(IHttpRequest request, string username)
        {
            request.Session.Add(SessionStore.CurrentUserKey, username);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}
