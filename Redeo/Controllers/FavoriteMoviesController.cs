using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Migrations;
using Redeo.Models;
using X.PagedList;

namespace Redeo.Controllers
{
    public class FavoriteMoviesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoriteMoviesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }

        // GET: FavoriteMovies
        public async Task<IActionResult> Index(int? page)
        {
            ViewBag.Category = GetCategory();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favMovies = _context.FavoriteMovies.Include(f => f.Movie).Include(f => f.User).Where(n => n.UserId == userId);

            var pageNumber = page ?? 1;
            var pageSize = 10;

            return View(await favMovies.ToPagedListAsync(pageNumber, pageSize));
        }
        public JsonResult Add(int movieId)
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
        public JsonResult CheckFavorite(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favMovie = _context.FavoriteMovies.Where(n => n.UserId == userId && n.MovieId == movieId).FirstOrDefault();

            if(favMovie != null)
            {
                return Json(new { isFavorite = true });
            }
            else
            {
                return Json(new { isFavorite = false });
            }
        }


        //[HttpPost]
        public JsonResult Remove(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var FavMovie = _context.FavoriteMovies.Include(m => m.User).Include(v => v.Movie).Where(n => n.MovieId == movieId && n.UserId == userId).FirstOrDefault();

            _context.Remove(FavMovie);
            _context.SaveChanges();

            return Json(new { isRemoved = true });
        }
    }
}
