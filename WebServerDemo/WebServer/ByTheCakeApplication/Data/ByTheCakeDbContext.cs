namespace WebServer.ByTheCakeApplication.Data
{
    using Microsoft.EntityFrameworkCore;
    using WebServer.ByTheCakeApplication.Data.Models;

    public class ByTheCakeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrdersProducts { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.\\SQLEXPRESS;Database=ByTheCakeDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            builder
                .Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.Userid);

            builder
                .Entity<Product>()
                .HasMany(pr => pr.OrdersProducts)
                .WithOne(op => op.Product)
                .HasForeignKey(op => op.ProductId);

            builder
                .Entity<Order>()
                .HasMany(o => o.OrdersProducts)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);


        }
    }
}
