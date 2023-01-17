using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services
{
    public interface ITvShowService : IEntityBaseRepository<TvShow>
    {
        Task<TvShow> GetTvShowByIdAsync(int id);
        Task<TvShowDropdownsVM> GetNewTvShowDropdownsValues();
        Task AddNewTvShowAsync(TvShowVM data);
        Task UpdateTvShowAsync(TvShowVM data);
    }
}
