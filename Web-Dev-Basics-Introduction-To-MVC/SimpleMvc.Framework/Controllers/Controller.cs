namespace SimpleMvc.Framework.Controllers
{
    using System;
    using System.Runtime.CompilerServices;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Contracts.Generics;
    using SimpleMvc.Framework.Helpers;
    using SimpleMvc.Framework.ViewEngine;
    using SimpleMvc.Framework.ViewEngine.Generics;

    public abstract class Controller
    {
        protected IActionResult View([CallerMemberName] string caller ="")
        {
            var controllerName = ControllerHelper
                .GetControllerName(this);

            var fullQualifiedName = ControllerHelper
                .GetViewFullQualifiedName(controllerName, caller);

            return new ActionResult(fullQualifiedName);
        }

        protected IActionResult View(
            string controller, 
            string action)
        {
            var fullQualifiedName = ControllerHelper
                .GetViewFullQualifiedName(controller, action);

            return new ActionResult(fullQualifiedName);
        }

        protected IActionResult<TModel> View<TModel>(
            TModel model, 
            [CallerMemberName] string caller ="")
        {
            var controllerName = ControllerHelper
                .GetControllerName(this);

            var fullQualifiedName = ControllerHelper
                .GetViewFullQualifiedName(controllerName, caller);

            return new ActionResult<TModel>(fullQualifiedName, model);
        }

        protected IActionResult<TModel> View<TModel>(
            TModel model, 
            string controller, 
            string action)
        {
            var fullQualifiedName = ControllerHelper
                .GetViewFullQualifiedName(controller, action);

            return new ActionResult<TModel>(fullQualifiedName, model);
        }
    }
}
