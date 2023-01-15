using Microsoft.EntityFrameworkCore;
using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services.Episode
{
    public class EpisodeService : EntityBaseRepository<TEpisodes>, IEpisodeService
    {
        private readonly AppDbContext _context;
        public EpisodeService(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddNewEpisodeAsync(EpisodeVM data)
        {
            var newEpisode = new TEpisodes()
            {
                Episode = data.Episode,
                SeasonId = data.SeasonId,
            };

            await _context.episodes.AddAsync(newEpisode);
            await _context.SaveChangesAsync();
        }

        public async Task<EpisodeDropdownsVM> GetNewEpisodeDropdownsValues()
        {
            var response = new EpisodeDropdownsVM()
            {
                Seasons = await _context.seasons.OrderBy(n => n.Season).ToListAsync()
            };
            return response;
        }

        public async Task<TEpisodes> GetEpisodeByIdAsync(int id)
        {
            var episodeDetails = await _context.episodes
                .Include(n => n.Season)
                .ThenInclude(m => m.TvShow)
                .FirstOrDefaultAsync(v => v.Id == id);
            return episodeDetails;
        }

        public async Task UpdateEpisodeAsync(EpisodeVM data)
        {
            var episode = await _context.episodes.FirstOrDefaultAsync(n => n.Id == data.Id);

            if(episode != null)
            {
                episode.Episode = data.Episode;
                episode.SeasonId = data.SeasonId;

                await _context.SaveChangesAsync();
            }
         
            await _context.SaveChangesAsync();
        }
    }
}
