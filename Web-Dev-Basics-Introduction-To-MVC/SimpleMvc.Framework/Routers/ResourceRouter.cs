namespace SimpleMvc.Framework.Routers
{
    using System;
    using System.IO;
    using System.Linq;
    using WebServer.Contracts;
    using WebServer.Enums;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class ResourceRouter : IHandleable
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            string fileName = request.Path.Split('/').Last();
            string fileExtension = fileName.Split('.').Last();

            try
            {
                byte[] fileContent = this.ReadFile(fileName, fileExtension);

                return new FileResponse(HttpStatusCode.Found, fileContent);
            }
            catch (Exception ex)
            {
                return new NotFoundResponse();
            }
        }

        private byte[] ReadFile(string fileFullName, string fileExtension)
        {
            return File.ReadAllBytes(string.Format(
                "{0}\\{1}\\{2}",
                MvcContext.Get.ResourcesFolder,
                fileExtension,
                fileFullName));
        }
    }
}
