using Microsoft.EntityFrameworkCore;
using Redeo.Models;

namespace Redeo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> categories { get; set; }
        public DbSet<Actor> actors { get; set; }
    }
}
