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
                Description = data.Description,
                DateOfRelease = data.DateOfRelease
            };

            await _context.moives.AddAsync(newMovie);
            await _context.SaveChangesAsync();

            //Add Movie Actors
            foreach (var categoryId in data.CategoryIds)
            {
                var newActorMovie = new Movie_Category()
                {
                    MovieId = newMovie.Id,
                    CategoryId = categoryId
                };
                await _context.movies_categories.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync(); 
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _context.moives
                .Include(am => am.Movies_Categories).ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }

        public async Task<MovieDropdownsVM> GetNewMovieDropdownsValues()
        {
            var response = new MovieDropdownsVM()
            {
                Categories = await _context.categories.OrderBy(n => n.CategoryName).ToListAsync()
            };
            return response;
        }

        public async Task UpdateMovieAsync(MovieVM data)
        {
            var dbMovie = await _context.moives.FirstOrDefaultAsync(n => n.Id == data.Id);

            if (dbMovie != null)
            {
                dbMovie.Name = data.Name;
                dbMovie.Description = data.Description;
                dbMovie.DateOfRelease = data.DateOfRelease;

                await _context.SaveChangesAsync();
            }

            //Remove Existing Categories
            var existingActorsDb = _context.movies_categories.Where(n => n.MovieId == data.Id).ToList();
            _context.movies_categories.RemoveRange(existingActorsDb);
            await _context.SaveChangesAsync();

            //Add Movie Categories
            foreach(var categoryId in data.CategoryIds)
            {
                var newActorMovie = new Movie_Category()
                {
                    MovieId = data.Id,
                    CategoryId = categoryId
                };

                await _context.movies_categories.AddAsync(newActorMovie);
            }

            await _context.SaveChangesAsync();
        }
    }
}
