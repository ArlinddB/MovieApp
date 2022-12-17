using Microsoft.EntityFrameworkCore;
using Redeo.Models;

namespace Redeo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> categories { get; set; }
        public DbSet<Producers> producers { get; set; }
        public DbSet<Actor> actors { get; set; }
        public DbSet<Movie> moives { get; set; }
        public DbSet<Movie_Category> movies_categories { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie_Category>().HasKey(mc => new
            {
                mc.MovieId,
                mc.CategoryId
            });

            modelBuilder.Entity<Movie_Category>().HasOne(m => m.Movie).WithMany(mc => mc.Movies_Categories).HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<Movie_Category>().HasOne(m => m.Category).WithMany(mc => mc.Movies_Categories).HasForeignKey(m => m.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
