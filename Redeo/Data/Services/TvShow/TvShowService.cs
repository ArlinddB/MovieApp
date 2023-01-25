using Microsoft.EntityFrameworkCore;
using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services
{
    public class TvShowService : EntityBaseRepository<TvShow>, ITvShowService
    {
        private readonly AppDbContext _context;
        public TvShowService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewTvShowAsync(TvShowVM data)
        {
            var newTvShow = new TvShow()
            {
                Name = data.Name,
                TvShowPicture = data.TvShowPicture,
                Description = data.Description,
                DateOfRelease = data.DateOfRelease,
                Duration = data.Duration,
                Quality = data.Quality,
                ProducerId = data.ProducerId,
                Clicks = 0
            };

            await _context.tvShows.AddAsync(newTvShow);
            await _context.SaveChangesAsync();

            //Add Tv Show Categories
            foreach(var categoryId in data.CategoryIds)
            {
                var newCategoryTvShow = new TvShow_Category()
                {
                    TvShowId = newTvShow.Id,
                    CategoryId = categoryId
                };
                await _context.tvShows_categories.AddAsync(newCategoryTvShow);
            }

            //Add Tv Show Actors
            foreach(var actorId in data.ActorIds)
            {
                var newActorTvShow = new TvShow_Actor()
                {
                    TvShowId = newTvShow.Id,
                    ActorId = actorId
                };
                await _context.tvShows_actors.AddAsync(newActorTvShow);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<TvShowDropdownsVM> GetNewTvShowDropdownsValues()
        {
            var response = new TvShowDropdownsVM()
            {
                Categories = await _context.categories.OrderBy(n => n.CategoryName).ToListAsync(),
                Producers = await _context.producers.OrderBy(n => n.ProducersName).ToListAsync(),
                Actors = await _context.actors.OrderBy(n => n.FullName).ToListAsync()
            };
            return response;
        }

        public async Task<TvShow> GetTvShowByIdAsync(int id)
        {
            var tvShowDetails = await _context.tvShows
                 .Include(m => m.Producers)
                 .Include(b => b.TvShows_Actors)
                 .ThenInclude(ab => ab.Actor)
                 .Include(am => am.TvShows_Categories)
                 .ThenInclude(a => a.Category)
                 .Include(v => v.Seasons.OrderByDescending(g => g.Id))
                 .ThenInclude(z => z.Episodes.OrderByDescending(g => g.Id))
                 .FirstOrDefaultAsync(n => n.Id == id);

            return tvShowDetails;
        }

        public async Task UpdateTvShowAsync(TvShowVM data)
        {
            var tvShow = await _context.tvShows.FirstOrDefaultAsync(n => n.Id == data.Id);

            if(tvShow != null)
            {
                tvShow.Name = data.Name;
                tvShow.TvShowPicture = data.TvShowPicture;
                tvShow.Description = data.Description;
                tvShow.DateOfRelease = data.DateOfRelease;
                tvShow.Duration = data.Duration;
                tvShow.Quality = data.Quality;
                tvShow.ProducerId = data.ProducerId;

                await _context.SaveChangesAsync();
            }

            var existingCategories = _context.tvShows_categories.Where(n => n.TvShowId == data.Id).ToList();
            _context.tvShows_categories.RemoveRange(existingCategories);

            var existingActors = _context.tvShows_actors.Where(n => n.TvShowId == data.Id).ToList();
            _context.tvShows_actors.RemoveRange(existingActors);

            foreach(var categoryId in data.CategoryIds)
            {
                var newCategoryTvShow = new TvShow_Category()
                {
                    TvShowId = data.Id,
                    CategoryId = categoryId
                };
                await _context.tvShows_categories.AddAsync(newCategoryTvShow);
            }

            foreach (var actorId in data.ActorIds)
            {
                var newActorTvShow = new TvShow_Actor()
                {
                    TvShowId = data.Id,
                    ActorId = actorId
                };

                await _context.tvShows_actors.AddAsync(newActorTvShow);
            }

            await _context.SaveChangesAsync();
        }
    }
}
