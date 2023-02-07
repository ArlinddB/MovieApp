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
                EpisodeUrl = data.EpisodeUrl,
                EpisodeDescription = data.EpisodeDescription,
                SeasonId = data.SeasonId,
                TvShowId = data.TvShowId
            };

            await _context.episodes.AddAsync(newEpisode);
            await _context.SaveChangesAsync();
        }

        public async Task<EpisodeDropdownsVM> GetNewEpisodeDropdownsValues()
        {
            var response = new EpisodeDropdownsVM()
            {
                Seasons = await _context.seasons.OrderBy(n => n.Season).ToListAsync(),
                TvShows = await _context.tvShows.OrderBy(m => m.Name).ToListAsync()
            };
            return response;
        }

        public async Task<TEpisodes> GetEpisodeByIdAsync(int id)
        {
            var episodeDetails = await _context.episodes
                .Include(n => n.Season)
                .ThenInclude(m => m.TvShow)
                .Include(v => v.TvShow.Producers)
                .Include(b => b.TvShow.TvShows_Actors)
                .Include(c => c.TvShow.TvShows_Categories)
                .FirstOrDefaultAsync(v => v.Id == id);
            return episodeDetails;
        }

        public async Task UpdateEpisodeAsync(EpisodeVM data)
        {
            var episode = await _context.episodes.FirstOrDefaultAsync(n => n.Id == data.Id);

            if(episode != null)
            {
                episode.Episode = data.Episode;
                episode.EpisodeUrl = data.EpisodeUrl;
                episode.EpisodeDescription = data.EpisodeDescription;
                episode.SeasonId = data.SeasonId;
                episode.TvShowId = data.TvShowId;

                await _context.SaveChangesAsync();
            }
         
            await _context.SaveChangesAsync();
        }
    }
}
