﻿namespace WebServer.ByTheCakeApplication.Views
{
    using System;
    using WebServer.Server.Contracts;

    public class FileView : IView
    {
        private readonly string htmlFile;

        public FileView(string htmlFile)
        {
            this.htmlFile = htmlFile;
        }

        public string View()
        {
            return this.htmlFile;
        }
    }
}