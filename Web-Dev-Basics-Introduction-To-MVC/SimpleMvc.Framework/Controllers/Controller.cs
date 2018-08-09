namespace SimpleMvc.Framework.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using SimpleMvc.Framework.ActionResults;
    using SimpleMvc.Framework.Attributes.Validation;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Helpers;
    using SimpleMvc.Framework.Models;
    using SimpleMvc.Framework.Security;
    using SimpleMvc.Framework.ViewEngine;
    using WebServer.Http;
    using WebServer.Http.Contracts;

    public abstract class Controller
    {
        public Controller()
        {
            this.ViewModel = new ViewModel();
            this.User = new Authentication();
        }

        protected ViewModel ViewModel { get; set; }
        protected internal IHttpRequest Request { get; internal set; }
        protected internal Authentication User { get; private set; }

        protected IViewable View([CallerMemberName] string caller ="")
        {
            var controllerName = ControllerHelper
                .GetControllerName(this);

            var fullQualifiedName = ControllerHelper
                .GetViewFullQualifiedName(controllerName, caller);

            IRenderable view = new View(fullQualifiedName, this.ViewModel.Data);

            return new ViewResult(view);
        }

        protected IRedirectable Redirect(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }
        protected IActionResult NotFound()
        {
            return new NotFoundResult();
        }

        protected bool IsValidModel(object model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                IEnumerable<PropertyValidationAttribute> attributes = property
                    .GetCustomAttributes()
                    .Where(a => a is PropertyValidationAttribute)
                    .Cast<PropertyValidationAttribute>();   

                foreach (PropertyValidationAttribute attribute in attributes)
                {
                    object propertyValue = property.GetValue(model);

                    if (!attribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }
             
            return true;
        }

        protected internal void InitializeController()
        {
            string name = this
                .Request
                .Session
                .Get<string>(SessionStore.CurrentUserKey);

            if (name != null)
            {
                this.User = new Authentication(name);
            }
        }
        protected void SignIn (string name)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, name);
        }
        protected void SignOut ()
        {
            this.Request.Session.Clear();
        }
    }
}
