namespace WebServer.GameStoreApplication.Data
{
    using Microsoft.EntityFrameworkCore;
    using WebServer.GameStoreApplication.Data.Models;
    using WebServer.GameStoreApplication.Data.ModelsConfigurations;

    public class GameStoreDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGame> UsersGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GameStoreDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new GameConfiguration());
            builder.ApplyConfiguration(new UserGameConfiguration());
        }
    }
}
