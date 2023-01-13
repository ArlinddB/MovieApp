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
        public async Task<IActionResult> Index(string s)
        {
            

            var movies = await _context.movies.ToListAsync();


            if (!string.IsNullOrEmpty(s))
            {
                var filteredMovie = movies.Where(n => n.Name.ToLower().Contains(s.ToLower()) || n.Description.ToLower().Contains(s.ToLower())).ToList();

                

                if (filteredMovie.Count == 0 )
                {
                    return View(nameof(NotFound404));
                }

                var result = new MovieTvShowViewModel()
                {
                    Movie = filteredMovie,
                    
                };
                return View(result);
            }
            return View("Index", "Movie");
        }

        public IActionResult NotFound404()
        {
            

            return View();
        }
    }
}