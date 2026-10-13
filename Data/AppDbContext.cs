using Microsoft.EntityFrameworkCore;
using TestApi.Models;

namespace TestApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);

        // Seed data so GET returns something right away
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop",   Description = "14-inch laptop",      Price = 999.99m, Quantity = 10,  CreatedAt = seedDate },
            new Product { Id = 2, Name = "Mouse",    Description = "Wireless mouse",      Price = 19.99m,  Quantity = 100, CreatedAt = seedDate },
            new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 59.50m,  Quantity = 50,  CreatedAt = seedDate }
        );
    }
}
