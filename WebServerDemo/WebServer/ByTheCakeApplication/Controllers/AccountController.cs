namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using WebServer.ByTheCakeApplication.Infrastructure;
    using WebServer.ByTheCakeApplication.Services;
    using WebServer.ByTheCakeApplication.ViewModels;
    using WebServer.ByTheCakeApplication.ViewModels.Account;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class AccountController : Controller
    {
        private const string HtmlAccountLogin = @"account\login";
        private const string HtmlAccountRegister = @"account\register";
        private const string HtmlAccountProfile = @"account\profile";

        private const string FormUsernameKey = "username";
        private const string FormPasswordKey = "password";

        private const string ShowError = "showError";
        private const string Error = "error";

        private const string MessageEmptyField = "You have empty fields";
        private const string MessageInvalidUserDetails = "Invalid user details";
        private const string MessageUsernameIsBusy = "This username is taken.";

        private const string AuthenticateDisplay = "authDisplay";   // duplicated with Controller constant

        private const string None = "none";     // duplicated with CakesController constant
        private const string Block = "block";   // duplicated with CakesController constant

        private readonly IUserService userService;

        public AccountController()
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(HtmlAccountRegister);
        }
        public IHttpResponse Register(IHttpRequest request, RegisterUserViewModel model)
        {
            this.SetDefaultViewData();

            // Validate the model.
            if (model.Username.Length < 3 ||
                model.Password.Length < 3 ||
                model.ConfirmPassword != model.Password)
            {
                this.ViewData[ShowError] = Block;
                this.ViewData[Error] = MessageInvalidUserDetails;

                return this.FileViewResponse(HtmlAccountRegister);
            }

            var success = this.userService.Create(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ViewData[ShowError] = Block;
                this.ViewData[Error] = MessageUsernameIsBusy;

                return this.FileViewResponse(HtmlAccountRegister);
            }
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();

            return FileViewResponse(HtmlAccountLogin);
        }
        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                this.ViewData[ShowError] = Block;
                this.ViewData[Error] = MessageEmptyField;

                return FileViewResponse(HtmlAccountLogin);
            }

            var success = this.userService.Find(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ViewData[ShowError] = Block;
                this.ViewData[Error] = MessageInvalidUserDetails;

                return FileViewResponse(HtmlAccountLogin);
            }
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

            return this.FileViewResponse(HtmlAccountProfile);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData()
        {
            this.ViewData[ShowError] = None;
            this.ViewData[AuthenticateDisplay] = None;
        }
        private void LoginUser(IHttpRequest request, string username)
        {
            request.Session.Add(SessionStore.CurrentUserKey, username);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}
