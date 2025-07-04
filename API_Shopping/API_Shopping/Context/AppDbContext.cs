using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

    }
}
