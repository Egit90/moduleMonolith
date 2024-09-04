using System.Reflection;
using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.Data;

public class BasketDbContext(DbContextOptions<BasketDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("basket");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
