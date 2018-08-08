namespace SimpleMvc.Framework.ViewEngine
{
    using SimpleMvc.Framework.Contracts;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class View : IRenderable
    {
        public const string BaseLayoutFileName = "Layout";
        public const string ContentPlaceholder = "{{{content}}}";
        public const string HtmlExtension = ".html";
        public const string LocalErrorPath = "\\SimpleMvc.Framework\\Errors\\Error.html";

        private readonly string templateFullQualifiedName;
        private readonly IDictionary<string, string> viewData;

        public View(string templateFullQualifiedName, IDictionary<string, string> viewData)
        {
            this.templateFullQualifiedName = templateFullQualifiedName;
            this.viewData = viewData;
        }

        public string Render()
        {
            string fileHtml = this.ReadFile();

            if (this.viewData.Any())
            {
                foreach (KeyValuePair<string, string> data in this.viewData)
                {
                    fileHtml = fileHtml.Replace($"{{{{{{{data.Key}}}}}}}", data.Value);
                }
            }

            return fileHtml;
        }
        private string ReadFile()
        {
            string layoutHtml = this.ReadLayoutFile();

            string templateFullFilePath = $"{this.templateFullQualifiedName}{HtmlExtension}";

            if (!File.Exists(templateFullFilePath))
            {
                this.viewData.Add("error", $"Requested view ({templateFullFilePath}) does not exist.");

                return this.GetErrorHtml() ;
            }

            string templateHtml = File.ReadAllText(templateFullFilePath);
            return layoutHtml.Replace(ContentPlaceholder, templateHtml);
        }
        private string ReadLayoutFile()
        {
            string layoutHtmlFile = string.Format(
                "{0}\\{1}{2}",
                MvcContext.Get.ViewsFolder,
                BaseLayoutFileName,
                HtmlExtension);

            if (!File.Exists(layoutHtmlFile))
            {
                this.viewData.Add("error", $"Layout view ({layoutHtmlFile}) does not exist.");

                return this.GetErrorHtml();
            }

            string layoutHtmlFileContent = File.ReadAllText(layoutHtmlFile);

            return layoutHtmlFileContent;
        }
        private string GetErrorHtml()
        {
            string errorPath = this.GetErrorPath();
            string errorHtml = File.ReadAllText(errorPath);

            return errorHtml;
        }
        private string GetErrorPath()
        {
            string appDirectoryPath = Directory.GetCurrentDirectory();
            DirectoryInfo parentDirectory = Directory.GetParent(appDirectoryPath);
            string parentDirectoryPath = parentDirectory.FullName;

            return $"{parentDirectoryPath}{LocalErrorPath}";
        }
    }
}
