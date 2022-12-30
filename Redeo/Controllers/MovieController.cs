using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Services;
using Redeo.Models;
using Redeo.ViewModels;
using X.PagedList;

namespace Redeo.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _service;
        private readonly AppDbContext _context;

        public MovieController(IMovieService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? page)
        {
            ViewData["CurrentFilter"] = searchString;

            var movies = from m in _context.movies
                         select m;

            var pageNumber = page ?? 1;
            var pageSize = 10;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Name.Contains(searchString));
            }

            return View(await movies.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
            
        }

        //Get:Movie/Create
        public async Task<IActionResult> Create()
        {
            var movieDropDowns = await _service.GetNewMovieDropdownsValues();

            ViewBag.Categories = new SelectList(movieDropDowns.Categories, "Id", "CategoryName");
            ViewBag.Producers = new SelectList(movieDropDowns.Producers, "Id", "ProducersName");
            ViewBag.Actors = new SelectList(movieDropDowns.Actors, "Id", "FullName");

            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieVM movie)
        {
            if (!ModelState.IsValid)
            {
                var movieDropDowns = await _service.GetNewMovieDropdownsValues();

                ViewBag.Categories = new SelectList(movieDropDowns.Categories, "Id", "CategoryName");
                ViewBag.Producers = new SelectList(movieDropDowns.Producers, "Id", "ProducersName");
                ViewBag.Actors = new SelectList(movieDropDowns.Actors, "Id", "FullName");

                return View(movie);
            }

            await _service.AddNewMovieAsync(movie);
            TempData["success"] = "Movie added successfully";
            return RedirectToAction(nameof(Index));
        }

        //GET:Movie/Details/id
        public async Task<IActionResult> Details(int id)
        {
            var movieDatails = await _service.GetMovieByIdAsync(id);
            if (movieDatails == null)
                return BadRequest("NotFound");

            return View(movieDatails);
        }

        // GET: Actor/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var movieDatails = await _service.GetMovieByIdAsync(id);
            if (movieDatails == null)
                return RedirectToAction("NotFound", "Error");

            var response = new MovieVM()
            {
                Id = movieDatails.Id,
                Name = movieDatails.Name,
                MoviePicture = movieDatails.MoviePicture,
                Description = movieDatails.Description,
                DateOfRelease = movieDatails.DateOfRelease,
                Duration = movieDatails.Duration,
                Quality = movieDatails.Quality,
                MovieUrl = movieDatails.MovieUrl,
                CategoryIds = movieDatails.Movies_Categories.Select(x => x.CategoryId).ToList(),
                ProducerId = movieDatails.ProducerId,
                ActorIds = movieDatails.Movies_Actors.Select(c => c.ActorId).ToList()
            };

            var movieDropDowns = await _service.GetNewMovieDropdownsValues();

            ViewBag.Categories = new SelectList(movieDropDowns.Categories, "Id", "CategoryName");
            ViewBag.Producers = new SelectList(movieDropDowns.Producers, "Id", "ProducersName");
            ViewBag.Actors = new SelectList(movieDropDowns.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieVM movie)
        {
            if (id != movie.Id)
                return RedirectToAction("NotFound", "Error");

            if (!ModelState.IsValid)
            {
                var movieDropDowns = await _service.GetNewMovieDropdownsValues();

                ViewBag.Categories = new SelectList(movieDropDowns.Categories, "Id", "CategoryName");
                ViewBag.Producers = new SelectList(movieDropDowns.Producers, "Id", "ProducersName");
                ViewBag.Actors = new SelectList(movieDropDowns.Actors, "Id", "FullName");

                return View(movie);
            }

            await _service.UpdateMovieAsync(movie);
            TempData["success"] = "Movie updated successfully";

            return RedirectToAction("Index", "Movie");
        }

        //GET: Actor/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var movieDetails = await _service.GetByIdAsync(id);
            if (movieDetails == null) 
                return RedirectToAction("NotFound", "Error");

            return View(movieDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieDetails = await _service.GetByIdAsync(id);
            if (movieDetails == null) return RedirectToAction("NotFound", "Error");

            await _service.DeleteAsync(id);
            TempData["success"] = "Movie deleted successfully";

            return RedirectToAction("Index", "Movie");
        }

        //Checking if category exists
        public JsonResult MovieAvailability(string? name, String? description)
        {
            System.Threading.Thread.Sleep(450);
            var data = _context.movies.Where(x => x.Name.Equals(name)).SingleOrDefault();
            var data1 = _context.movies.Where(x => x.Description.Equals(description)).SingleOrDefault();

            if (data != null && data1 != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
    }
}
