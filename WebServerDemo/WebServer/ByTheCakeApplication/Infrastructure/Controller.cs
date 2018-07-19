namespace WebServer.ByTheCakeApplication.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Views;
    using WebServer.Server.Enums;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public abstract class Controller
    {
        private const string DefaultPath = @"..\..\..\ByTheCakeApplication\Resources\{0}.html";
        private const string Layout = "layout";
        private const string ContentPlaceholder = "{{{content}}}";

        protected const string HttpAuthenticateDisplay = "authDisplay";

        private const string HtmlShowError = "showError";
        private const string HtmlError = "error";

        protected const string HtmlBlock = "block";
        protected const string HtmlNone = "none";

        protected const string MessageInvalidUserDetails = "Invalid user details";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                [HttpAuthenticateDisplay] = HtmlBlock
            };
        }

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

        protected void HideErrorDiv()
        {
            this.ViewData[HtmlShowError] = HtmlNone;
        }
        protected void ShowError(string errorMessage)
        {
            this.ViewData[HtmlShowError] = HtmlBlock;
            this.ViewData[HtmlError] = errorMessage;
        }

        private string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(String.Format(DefaultPath, Layout));
            var fileHtml = File.ReadAllText(String.Format(DefaultPath, fileName));
            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }
    }
}
