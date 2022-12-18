using Redeo.Data.Base;
using Redeo.Models;

namespace Redeo.Data.Services
{
    public class MovieService : EntityBaseRepository<Movie>, IMovieService
    {
        public MovieService(AppDbContext context) : base(context) { }
    }
}
