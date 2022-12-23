using Redeo.Data.Base;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Data.Services
{
    public interface IMovieService : IEntityBaseRepository<Movie> 
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<MovieDropdownsVM> GetNewMovieDropdownsValues();
        Task AddNewMovieAsync(MovieVM data);
        Task UpdateMovieAsync(MovieVM data);
    }
}
