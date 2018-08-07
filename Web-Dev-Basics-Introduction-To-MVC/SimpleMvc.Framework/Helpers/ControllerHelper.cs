using SimpleMvc.Framework.Controllers;

namespace SimpleMvc.Framework.Helpers
{
    public static class ControllerHelper
    {
        public static string GetControllerName(Controller controller)
        {
            return controller
                .GetType()
                .Name
                .Replace(MvcContext.Get.ControllerSuffix, string.Empty);
        }

        public static string GetViewFullQualifiedName(string controller, string action)
        {
            return string.Format(
                "{0}.{1}.{2}.{3}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ViewsFolder,
                controller,
                action);
        }
    }
}
