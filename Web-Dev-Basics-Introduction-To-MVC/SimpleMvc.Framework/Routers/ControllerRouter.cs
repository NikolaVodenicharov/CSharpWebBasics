namespace SimpleMvc.Framework.Routers
{
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using WebServer.Contracts;
    using WebServer.Exceptions;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class ControllerRouter : IHandleable
    {
        private IDictionary<string, string> getParameters;
        private IDictionary<string, string> postParameters;
        private string requestMethod;
        private Controller controllerInstance;
        private string controllerName;
        private string actionName;
        private object[] methodParameters;

        public IHttpResponse Handle(IHttpRequest request)
        {
            this.getParameters = new Dictionary<string, string>(request.UrlParameters);
            this.postParameters = new Dictionary<string, string>(request.FormData);

            this.requestMethod = request
                .Method
                .ToString()
                .ToUpper();

            this.PrepareControllerAndActionNames(request);

            var methodInfo = this.GetActionForExecution();
            if (methodInfo == null)
            {
                return new NotFoundResponse();
            }

            this.PrepareMethodParameters(methodInfo);

            try
            {
                if (this.controllerInstance != null)
                {
                    this.controllerInstance.Request = request;
                    this.controllerInstance.InitializeController();
                }

                var response = this.GetResponse(methodInfo, this.controllerInstance);
                return response;
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResponse(ex);
            }
        }

        private void PrepareControllerAndActionNames(IHttpRequest request)
        {
            var pathParts = request.Path.Split(
                new[] { '/', '?' },
                StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length < 2)
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.controllerName = $"{pathParts[0].Capitalize()}{MvcContext.Get.ControllerSuffix}";
            this.actionName = pathParts[1].Capitalize();
        }

        private MethodInfo GetActionForExecution()
        {
            var methods = this.GetSuitableMethods();

            foreach (var method in methods)
            {
                var httpMethodAttributes = method
                    .GetCustomAttributes()
                    .Where(a => a is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!httpMethodAttributes.Any() && this.requestMethod == "GET")
                {
                    return method;
                }

                foreach (var httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.IsValid(this.requestMethod))
                    {
                        return method;
                    }
                }
            }

            return null;
        }
        private IEnumerable<MethodInfo> GetSuitableMethods()
        {
            this.SetControllerInstance();

            if (this.controllerInstance == null)
            {
                return new MethodInfo[0];
            }

            return this.controllerInstance
               .GetType()
               .GetMethods()
               .Where(m => m.Name == actionName)
               .ToList();
        }
        private void SetControllerInstance()
        {
            if (this.controllerInstance == null)
            {
                string controllerFullQualifiedName = string.Format(
                    "{0}.{1}.{2}, {0}",
                    MvcContext.Get.AssemblyName,
                    MvcContext.Get.ControllersFolder,
                    this.controllerName);

                Type controllerType = Type.GetType(controllerFullQualifiedName);

                this.controllerInstance = Activator
                    .CreateInstance(controllerType)
                    as Controller;
            }
        }

        private void PrepareMethodParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();

            this.methodParameters = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];

                if (parameter.ParameterType.IsPrimitive ||
                    parameter.ParameterType == typeof(string))
                {
                    this.ProcessPrimitiveParameter(parameter, i);
                }
                else
                {
                    this.ProcessModelMarameter(parameter, i);
                }
            }
        }
        private void ProcessPrimitiveParameter(ParameterInfo parameter, int index)
        {
            string parameterValue = this.getParameters[parameter.Name];

            object value = Convert
                .ChangeType(
                    parameterValue,
                    parameter.ParameterType);

            this.methodParameters[index] = value;
        }
        private void ProcessModelMarameter(ParameterInfo parameter, int index)
        {
            Type modelType = parameter.ParameterType;
            object modelInstance = Activator.CreateInstance(modelType);
            PropertyInfo[] modelProperties = modelType.GetProperties();

            foreach (var modelProperty in modelProperties)
            {
                var postParameterValue = this.postParameters[modelProperty.Name];

                var value = Convert
                    .ChangeType(
                        postParameterValue,
                        modelProperty.PropertyType);

                modelProperty
                    .SetValue(
                        modelInstance,
                        value);
            }

            this.methodParameters[index] = Convert
                .ChangeType(
                    modelInstance,
                    modelType);
        }

        private IHttpResponse GetResponse(MethodInfo methodInfo, object controller)
        {
            IActionResult actionResult = methodInfo
                .Invoke(
                    controller, 
                    this.methodParameters)
                as IActionResult;

            if (actionResult == null)
            {
                var actionResultAsHttpResponse = actionResult as HttpResponse;

                if (actionResultAsHttpResponse != null)
                {
                    return actionResultAsHttpResponse;
                }
                else
                {
                    throw new InvalidOperationException("Controller action should return either IActionResul or IHttpResponse.");
                }
            }

            return actionResult.Invoke();
        }
    }
}
