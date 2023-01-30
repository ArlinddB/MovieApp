using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Controllers
{
    public class FilterController : Controller
    {
        private readonly AppDbContext _context;

        public FilterController(AppDbContext context)
        {
            _context = context;
        }
        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }
        public async Task<IActionResult> Index(string s)
        {
            ViewBag.Category = GetCategory();

            var movies = await _context.movies.ToListAsync();
            var tvShows = await _context.tvShows.ToListAsync();


            if (!string.IsNullOrEmpty(s))
            {
                var filteredMovie = movies.Where(n => n.Name.ToLower().Contains(s.ToLower()) || n.Description.ToLower().Contains(s.ToLower())).ToList();
                
                var filteredTvShows = tvShows.Where(n => n.Name.ToLower().Contains(s.ToLower()) || n.Description.ToLower().Contains(s.ToLower())).ToList();


                if (filteredMovie.Count == 0 && filteredTvShows.Count == 0)
                {
                    return View(nameof(NotFound404));
                }

                var result = new MovieTvShowViewModel()
                {
                    Movie = filteredMovie,
                    TvShows = filteredTvShows
                    
                };
                return View(result);
            }
            return View("Index", "Movie");
        }

        public IActionResult NotFound404()
        {
            ViewBag.Category = GetCategory();

            return View();
        }
    }
}