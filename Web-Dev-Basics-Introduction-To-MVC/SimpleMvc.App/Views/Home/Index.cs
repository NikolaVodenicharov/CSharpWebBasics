namespace SimpleMvc.App.Views.Home
{
    using System;

    using SimpleMvc.Framework.Contracts;

    public class Index : IRenderable
    {
        public string Render()
        {
            return "<h1>Test</h1>";
        }
    }
}
