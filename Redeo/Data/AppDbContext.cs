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
        public DbSet<Movie> movies { get; set; }
        public DbSet<Movie_Category> movies_categories { get; set; }

        public DbSet<Movie_Actor> movie_actors { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Movie with Categories
            modelBuilder.Entity<Movie_Category>().HasKey(mc => new
            {
                mc.MovieId,
                mc.CategoryId
            });

            modelBuilder.Entity<Movie_Category>().HasOne(m => m.Movie).WithMany(mc => mc.Movies_Categories).HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<Movie_Category>().HasOne(m => m.Category).WithMany(mc => mc.Movies_Categories).HasForeignKey(m => m.CategoryId);

            //Movie with Actors
            modelBuilder.Entity<Movie_Actor>().HasKey(ma => new
            {
                ma.MovieId,
                ma.ActorId
            });

            modelBuilder.Entity<Movie_Actor>().HasOne(m => m.Movie).WithMany(ma => ma.Movies_Actors).HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<Movie_Actor>().HasOne(m => m.Actor).WithMany(ma => ma.Movies_Actors).HasForeignKey(m => m.ActorId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
