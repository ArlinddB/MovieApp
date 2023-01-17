using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services
{
    public interface ISeasonService : IEntityBaseRepository<TSeason>
    {
        Task<TSeason> GetSeasonByIdAsync(int id);
        Task<SeasonDropdownsVM> GetNewSeasonDropdownsValues();
        Task AddNewSeasonAsync(SeasonVM data);
        Task UpdateSeasonAsync(SeasonVM data);
    }
}
