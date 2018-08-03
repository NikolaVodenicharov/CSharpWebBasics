namespace WebServer.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using WebServer.Server.Enums;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public abstract class Controller
    {
        protected const string HtmlAuthenticateDisplay = "authDisplay";
        protected const string HtmlAnonymousDisplay = "anonymousDisplay";

        protected const string HtmlBlock = "block";
        protected const string HtmlNone = "none";
        protected const string HtmlFlex = "flex";

        protected const string MessageInvalidUserDetails = "Invalid user details";

        private const string DefaultPathFormat = @"..\..\..\{0}\Resources\{1}.html";
        private const string Layout = "layout";
        private const string ContentPlaceholder = "{{{content}}}";

        private const string HtmlShowError = "showError";
        private const string HtmlError = "error";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                [HtmlAnonymousDisplay] = HtmlNone,
                [HtmlAuthenticateDisplay] = HtmlBlock
            };

            this.HideErrorDiv();
        }

        protected abstract string ApplicationDirectory { get; }

        protected IDictionary<string, string> ViewData { get; private set; }
        protected IHttpResponse FileViewResponse(string fileName)
        {
            var result = this.ProcessFileHtml(fileName);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(result));
        }

        protected IHttpResponse RedirectResponse(string route)
        {
            return new RedirectResponse(route);
        }

        protected bool ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, context, results, true) == false)
            {
                foreach (var result in results)
                {
                    if (result != ValidationResult.Success)
                    {
                        this.ShowErrorDiv(result.ErrorMessage);
                        return false;
                    }
                }
            }

            return true;
        }

        protected void HideErrorDiv()
        {
            this.ViewData[HtmlShowError] = HtmlNone;
        }
        protected void ShowErrorDiv(string errorMessage)
        {
            this.ViewData[HtmlShowError] = HtmlBlock;
            this.ViewData[HtmlError] = errorMessage;
        }

        private string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(String.Format(DefaultPathFormat, this.ApplicationDirectory, Layout));
            var fileHtml = File.ReadAllText(String.Format(DefaultPathFormat, this.ApplicationDirectory, fileName));
            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }
    }
}
