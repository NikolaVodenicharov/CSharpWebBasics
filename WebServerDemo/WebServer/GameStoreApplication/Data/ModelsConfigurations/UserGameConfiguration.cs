namespace WebServer.GameStoreApplication.Data.ModelsConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using WebServer.GameStoreApplication.Data.Models;

    public class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
    {
        public void Configure(EntityTypeBuilder<UserGame> builder)
        {
            builder
                .HasKey(ug => new { ug.UserId, ug.GameId });

            builder
                .HasOne(ug => ug.User)
                .WithMany(u => u.UsersGames)
                .HasForeignKey(ug => ug.UserId);

            builder
                .HasOne(ug => ug.Game)
                .WithMany(g => g.UsersGames)
                .HasForeignKey(ug => ug.GameId);
        }
    }
}
