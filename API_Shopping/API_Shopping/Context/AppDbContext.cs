using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Detail> Details { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<ItemShoppingCart> ItemShoppingCarts { get; set; }
    }
}
