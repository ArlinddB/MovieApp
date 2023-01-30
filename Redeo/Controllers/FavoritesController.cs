using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Models;
using X.PagedList;

namespace Redeo.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }
        public IPagedList<FavoriteTvShow> GetFavoriteTvShows(int? page)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favTvShow = _context.FavoriteTvShows.Include(f => f.TvShow).Include(f => f.User).Where(n => n.UserId == userId);

            var pageNumber = page ?? 1;
            var pageSize = 10;

            return favTvShow.ToPagedList(pageNumber, pageSize);
        }
        // GET: FavoriteMovies
        public async Task<IActionResult> Index(int? page)
        {
            ViewBag.Category = GetCategory();

            ViewBag.FavoriteTvShows = GetFavoriteTvShows(page);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favMovies = _context.FavoriteMovies.Include(f => f.Movie).Include(f => f.User).Where(n => n.UserId == userId);

            var pageNumber = page ?? 1;
            var pageSize = 10;

            return View(await favMovies.ToPagedListAsync(pageNumber, pageSize));
        }

        public JsonResult AddFavoriteMovie(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var FavMovie = new FavoriteMovie
            {
                MovieId = movieId,
                UserId = userId,
                isFavorite = true
            };

            _context.Add(FavMovie);
            _context.SaveChanges();

            return Json(new { isAdded = true });
        }
        
        public JsonResult CheckFavoriteMovie(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favMovie = _context.FavoriteMovies.Where(n => n.UserId == userId && n.MovieId == movieId).FirstOrDefault();

            if(favMovie != null)
            {
                return Json(new { isFavorite = true });
            }
            else
            {
                return Json(new {isFavorite = false });
            }
        }

        public JsonResult RemoveFavoriteMovie(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var FavMovie = _context.FavoriteMovies.Include(m => m.User).Include(v => v.Movie).Where(n => n.MovieId == movieId && n.UserId == userId).FirstOrDefault();

            _context.Remove(FavMovie);
            _context.SaveChanges();

            return Json(new { isRemoved = true });
        }
        public JsonResult AddFavoriteTvShow(int tvShowId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var FavTvShow = new FavoriteTvShow
            {
                TvShowId = tvShowId,
                UserId = userId,
                isFavorite = true
            };

            _context.Add(FavTvShow);
            _context.SaveChanges();

            return Json(new { isAdded = true });
        }

        public JsonResult CheckFavoriteTvShow(int tvShowId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favTvShow = _context.FavoriteTvShows.Where(n => n.UserId == userId && n.TvShowId == tvShowId).FirstOrDefault();

            if (favTvShow != null)
            {
                return Json(new { isFavorite = true });
            }
            else
            {
                return Json(new { isFavorite = false });
            }
        }

        public JsonResult RemoveFavoriteTvShow(int tvShowId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favTvShow = _context.FavoriteTvShows.Include(m => m.User).Include(v => v.TvShow).Where(n => n.TvShowId == tvShowId && n.UserId == userId).FirstOrDefault();

            _context.Remove(favTvShow);
            _context.SaveChanges();

            return Json(new { isRemoved = true });
        }
    }
}
