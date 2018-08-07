namespace SimpleMvc.Framework.ViewEngine.Generics
{
    using SimpleMvc.Framework.Contracts.Generics;
    using System;

    public class ActionResult<TModel> : IActionResult<TModel>
    {
        public ActionResult(string viewFullQualifiedname, TModel model)
        {
            this.Action = (IRenderable<TModel>)Activator
                .CreateInstance(Type.GetType(viewFullQualifiedname));

            this.Action.Model = model;
        }

        public IRenderable<TModel> Action { get; set; }

        public string Invoke()
        {
            return this.Action.Render();
        }
    }
}
