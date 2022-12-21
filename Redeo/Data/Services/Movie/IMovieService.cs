using Redeo.Data.Base;
using Redeo.Migrations;
using Redeo.Models;

namespace Redeo.Data.Services
{
    public interface IMovieService : IEntityBaseRepository<Movie> 
    {
        Task<Movie> GetMovieByIdAsync(int id);
        
    }
}
