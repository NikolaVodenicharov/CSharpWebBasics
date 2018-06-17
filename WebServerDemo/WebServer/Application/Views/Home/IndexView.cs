namespace WebServer.Application.Views.Home
{
    using System;
    using WebServer.Server.Contracts;

    public class IndexView : IView
    {
        public string View()
        {
            return "<h1>Welcome!<h1>";
        }
    }
}
