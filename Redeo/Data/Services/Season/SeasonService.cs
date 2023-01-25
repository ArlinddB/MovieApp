using Microsoft.EntityFrameworkCore;
using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services
{
    public class SeasonService : EntityBaseRepository<TSeason>, ISeasonService
    {
        private readonly AppDbContext _context;
        public SeasonService(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddNewSeasonAsync(SeasonVM data)
        {
            var newSeason = new TSeason()
            {
                Season = data.Season,
                SeasonPoster = data.SeasonPoster,
                TvShowId = data.TvShowId
            };

            await _context.seasons.AddAsync(newSeason);
            await _context.SaveChangesAsync();
        }

        public async Task<SeasonDropdownsVM> GetNewSeasonDropdownsValues()
        {
            var response = new SeasonDropdownsVM()
            {
                TvShows = await _context.tvShows.OrderBy(n => n.Name).ToListAsync()
            };
            return response;
        }

        public async Task<TSeason> GetSeasonByIdAsync(int id)
        {
            var seasonDetails = await _context.seasons
                 .Include(m => m.TvShow)              
                 .FirstOrDefaultAsync(n => n.Id == id);

            return seasonDetails;
        }

        public async Task UpdateSeasonAsync(SeasonVM data)
        {
            var season = await _context.seasons.FirstOrDefaultAsync(n => n.Id == data.Id);

            if(season != null)
            {
                season.Season = data.Season;
                season.SeasonPoster = data.SeasonPoster;
                season.TvShowId = data.TvShowId;

                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }
    }
}