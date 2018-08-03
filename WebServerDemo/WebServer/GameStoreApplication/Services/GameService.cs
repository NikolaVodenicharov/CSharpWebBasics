namespace WebServer.GameStoreApplication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.GameStoreApplication.Data;
    using WebServer.GameStoreApplication.Data.Models;
    using WebServer.GameStoreApplication.Services.Contracts;
    using WebServer.GameStoreApplication.ViewModels.Admin;

    public class GameService : IGameService
    {
        public void Create(
            string title, 
            string description, 
            string image, 
            decimal price, 
            double size, 
            string trailerId, 
            DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = new Game
                {
                    Title = title,
                    Description = description,
                    ImageUrl = image,
                    Price = price,
                    SizeGigabytes = size,
                    TrailerId = trailerId,
                    ReleaseDate = releaseDate
                };

                db.Games.Add(game);
                db.SaveChanges();
            }
        }

        public IEnumerable<AdminListGameViewModel> All()
        {
            using (var db = new GameStoreDbContext())
            {
                return db
                    .Games
                    .Select(g => new AdminListGameViewModel
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Price = g.Price,
                        Size = g.SizeGigabytes
                    })
                    .ToList();
            }
        }
    }
}
