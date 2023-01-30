using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Areas.Admin.Models;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Models;

namespace Redeo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _userManager.Users.OrderByDescending(n => n.Id).Take(5).ToList();
        }
        public List<Movie> GetMostWachedMovies()
        {
            var avgViews = _context.movies.Average(n => n.Clicks);

            var topMovies = _context.movies.Where(m => m.Clicks > avgViews).OrderByDescending(v => v.Clicks).Take(4).ToList();

            return topMovies;
        }
        public List<TvShow> GetMostWachedTvShows()
        {
            var avgViews = _context.tvShows.Average(n => n.Clicks);

            var topTvShows = _context.tvShows.Where(m => m.Clicks > avgViews).OrderByDescending(v => v.Clicks).Take(4).ToList();

            return topTvShows;
        }
        [Route("/Dashboard")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Users = GetUsers();

            ViewBag.MostWatchedMovies = GetMostWachedMovies();

            ViewBag.MostWachedTvShows = GetMostWachedTvShows();

            var countUsers = await _userManager.Users.CountAsync();
            var countMovies = await _context.movies.CountAsync();
            var countTvShows = await _context.tvShows.CountAsync();
            var countGenres = await _context.categories.CountAsync();
            var countActors = await _context.actors.CountAsync();
            var countProducers = await _context.producers.CountAsync();

            var result = new Dashboard()
            {
                CountUsers = countUsers,
                CountMovies = countMovies,
                CountTvShows = countTvShows,
                CountGenres = countGenres,
                CountActors = countActors,
                CountProducers = countProducers
            };

            return View(result);
        }

        
    }
}
