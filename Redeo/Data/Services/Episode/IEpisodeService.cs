using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services
{
    public interface IEpisodeService : IEntityBaseRepository<TEpisodes>
    {
        Task<TEpisodes> GetEpisodeByIdAsync(int id);
        Task<EpisodeDropdownsVM> GetNewEpisodeDropdownsValues();
        Task AddNewEpisodeAsync(EpisodeVM data);
        Task UpdateEpisodeAsync(EpisodeVM data);
    }
}
