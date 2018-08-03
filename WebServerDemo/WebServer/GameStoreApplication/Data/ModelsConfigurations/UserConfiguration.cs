namespace WebServer.GameStoreApplication.Data.ModelsConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using WebServer.GameStoreApplication.Common;
    using WebServer.GameStoreApplication.Data.Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(ValidationConstants.Account.EmailMaxLength);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(ValidationConstants.Account.PasswordMaxLength);

            builder
                .Property(u => u.FullName)
                .HasMaxLength(ValidationConstants.Account.NameMaxLength);
        }
    }
}
