using Microsoft.EntityFrameworkCore;
using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services
{
    public class MovieService : EntityBaseRepository<Movie>, IMovieService
    {
        public readonly AppDbContext _context;
        public MovieService(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task AddNewMovieAsync(MovieVM data)
        {
            var newMovie = new Movie()
            {
                Name = data.Name,
                MoviePicture = data.MoviePicture,
                Description = data.Description,
                DateOfRelease = data.DateOfRelease,
                Duration = data.Duration,
                Quality = data.Quality,
                MovieUrl = data.MovieUrl,
                ProducerId = data.ProducerId
            };

            await _context.movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();

            //Add Movie Categories
            foreach (var categoryId in data.CategoryIds)
            {
                var newCategoryMovie = new Movie_Category()
                {
                    MovieId = newMovie.Id,
                    CategoryId = categoryId
                };
                await _context.movies_categories.AddAsync(newCategoryMovie);
            }

            //Add Movie Actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Movie_Actor()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                };
                await _context.movies_actors.AddAsync(newActorMovie);
            }

            await _context.SaveChangesAsync(); 
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _context.movies
                .Include(m => m.Producers)
                .Include(b => b.Movies_Actors)
                .ThenInclude(ab => ab.Actor)
                .Include(am => am.Movies_Categories)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }

        public async Task<MovieDropdownsVM> GetNewMovieDropdownsValues()
        {
            var response = new MovieDropdownsVM()
            {
                Categories = await _context.categories.OrderBy(n => n.CategoryName).ToListAsync(),
                Producers = await _context.producers.OrderBy(n => n.ProducersName).ToListAsync(),
                Actors = await _context.actors.OrderBy(n => n.FullName).ToListAsync()
            };
            return response;
        }

        public async Task UpdateMovieAsync(MovieVM data)
        {
            var dbMovie = await _context.movies.FirstOrDefaultAsync(n => n.Id == data.Id);

            if (dbMovie != null)
            {
                dbMovie.Name = data.Name;
                dbMovie.MoviePicture = data.MoviePicture;
                dbMovie.Description = data.Description;
                dbMovie.DateOfRelease = data.DateOfRelease;
                dbMovie.Duration = data.Duration;
                dbMovie.Quality = data.Quality;
                dbMovie.MovieUrl = data.MovieUrl;
                dbMovie.ProducerId = data.ProducerId;

                await _context.SaveChangesAsync();
            }

            //Remove Existing Categories
            var existingCategoriesDb = _context.movies_categories.Where(n => n.MovieId == data.Id).ToList();
            _context.movies_categories.RemoveRange(existingCategoriesDb);

            var existingActorsDb = _context.movies_actors.Where(n => n.MovieId == data.Id).ToList();
            _context.movies_actors.RemoveRange(existingActorsDb);


            //Add Movie Categories
            foreach(var categoryId in data.CategoryIds)
            {
                var newCategoryMovie = new Movie_Category()
                {
                    MovieId = data.Id,
                    CategoryId = categoryId
                };

                await _context.movies_categories.AddAsync(newCategoryMovie);
            }
            // Add movie actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Movie_Actor()
                {
                    MovieId = data.Id,
                    ActorId = actorId
                };

                await _context.movies_actors.AddAsync(newActorMovie);
            }

            await _context.SaveChangesAsync();
        }
    }
}
