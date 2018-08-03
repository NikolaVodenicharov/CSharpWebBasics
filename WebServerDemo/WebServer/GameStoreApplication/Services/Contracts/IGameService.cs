namespace WebServer.GameStoreApplication.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using WebServer.GameStoreApplication.ViewModels.Admin;

    public interface IGameService
    {
        void Create(
            string title,
            string description,
            string image,
            decimal price,
            double size,
            string trailerId,
            DateTime releaseDate);

        IEnumerable<AdminListGameViewModel> All();
    }
}
