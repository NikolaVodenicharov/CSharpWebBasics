namespace WebServer.GameStoreApplication.Data.ModelsConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using WebServer.GameStoreApplication.Data.Models;

    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .HasKey(g => g.Id);

            builder
                .Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(g => g.TrailerId)
                .HasMaxLength(11);
        }
    }
}