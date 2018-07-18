namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using WebServer.ByTheCakeApplication.Infrastructure;
    using WebServer.ByTheCakeApplication.Models;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class AccountController : Controller
    {
        private const string AccountLogin = @"account\login";
        private const string FormNameKey = "name";
        private const string FormPasswordKey = "password";

        private const string ShowError = "showError";
        private const string Error = "error";
        private const string EmptyFieldMessage = "You have empty fields";

        private const string AuthenticateDisplay = "authDisplay";

        private const string None = "none";     // duplicated with CakesController constant
        private const string Block = "block";   // duplicated with CakesController constant

        public IHttpResponse Login()
        {
            this.ViewData[ShowError] = None;
            this.ViewData[AuthenticateDisplay] = None;

            return FileViewResponse(AccountLogin);
        }
        
        public IHttpResponse Login(IHttpRequest request)
        {         
            if (!request.FormData.ContainsKey(FormNameKey) ||
                !request.FormData.ContainsKey(FormPasswordKey))
            {
                return new BadRequestResponse();
            }

            var name = request.FormData[FormNameKey];
            var password = request.FormData[FormPasswordKey];

            // this never execute if we give empty string login
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(password))
            {
                this.ViewData[Error] = EmptyFieldMessage;
                this.ViewData[ShowError] = Block;

                return FileViewResponse(AccountLogin);
            }

            request.Session.Add(SessionStore.CurrentUserKey, name);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());

            return new RedirectResponse("/");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }
    }
}
