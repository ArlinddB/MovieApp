using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Redeo.Models;

namespace Redeo.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> categories { get; set; }
        public DbSet<Producers> producers { get; set; }
        public DbSet<Actor> actors { get; set; }
        public DbSet<Movie> movies { get; set; }
        public DbSet<Movie_Category> movies_categories { get; set; }
        public DbSet<Movie_Actor> movies_actors { get; set; }
        public DbSet<TvShow> tvShows { get; set; }
        public DbSet<TSeason> seasons { get; set; }
        public DbSet<TEpisodes> episodes { get; set; }
        public DbSet<TvShow_Category> tvShows_categories { get; set; }
        public DbSet<TvShow_Actor> tvShows_actors { get; set; }
        public DbSet<SliderContent> SliderContents { get; set; }

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

            //Movie with Actor
            modelBuilder.Entity<Movie_Actor>().HasKey(mc => new
            {
                mc.MovieId,
                mc.ActorId
            });

            modelBuilder.Entity<Movie_Actor>().HasOne(m => m.Movie).WithMany(mc => mc.Movies_Actors).HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<Movie_Actor>().HasOne(m => m.Actor).WithMany(mc => mc.Movies_Actors).HasForeignKey(m => m.ActorId);

            //TvShow with Categories
            modelBuilder.Entity<TvShow_Category>().HasKey(t => new
            {
                t.TvShowId,
                t.CategoryId
            });

            modelBuilder.Entity<TvShow_Category>().HasOne(a => a.TvShow).WithMany(mc => mc.TvShows_Categories).HasForeignKey(m => m.TvShowId);
            modelBuilder.Entity<TvShow_Category>().HasOne(b => b.Category).WithMany(mc => mc.TvShows_Categories).HasForeignKey(m => m.CategoryId);

            //TvShow with Actor
            modelBuilder.Entity<TvShow_Actor>().HasKey(mc => new
            {
                mc.TvShowId,
                mc.ActorId
            });

            modelBuilder.Entity<TvShow_Actor>().HasOne(a => a.TvShow).WithMany(mc => mc.TvShows_Actors).HasForeignKey(m => m.TvShowId);
            modelBuilder.Entity<TvShow_Actor>().HasOne(b => b.Actor).WithMany(mc => mc.TvShows_Actors).HasForeignKey(m => m.ActorId);

            modelBuilder.Entity<TEpisodes>()
            .HasOne(e => e.TvShow)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
