using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Services;
using Redeo.Models;
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

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var a = await _context.moives.ToPagedListAsync(pageNumber, pageSize);
            return View(a);
        }

        //Get:Movie/Create
        public IActionResult Create()
        { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name", "Description", "DateOfRelease", "Movies_Categories")]Movie movie)
        {
            if(!ModelState.IsValid)
            {
                return View(movie);
            }
            var alreadyExists1 = await _context.moives.AnyAsync(x => x.Name == movie.Name);
            var alreadyExists2 = await _context.moives.AnyAsync(x => x.Description == movie.Description);

            if(alreadyExists1 && alreadyExists2)
            {
                ModelState.AddModelError("Name", "Movie already exists");
                ModelState.AddModelError("Description", "Movie already exists");
                return View(movie);
            }

            await _service.AddAsync(movie);
            TempData["success"] = "Movie added successfully";
            return RedirectToAction("Index", "Movie");
        }

        //GET:Movie/Details/id
        public async Task<IActionResult> Details(int id)
        {
            var movieDatails = await _service.GetByIdAsync(id);
            if (movieDatails == null)
                return RedirectToAction("NotFound", "Error");

            return View(movieDatails);
        }

        // GET: Actor/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var movieDatails = await _service.GetByIdAsync(id);
            if (movieDatails == null)
                return RedirectToAction("NotFound", "Error");

            return View(movieDatails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id", "Name", "Description", "DateOfRelease", "Movies_Categories")] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            var alreadyExists1 = await _context.moives.AnyAsync(x => x.Name == movie.Name);
            var alreadyExists2 = await _context.moives.AnyAsync(x => x.Description == movie.Description);


            if (alreadyExists1 && alreadyExists2)
            {
                ModelState.AddModelError("Name", "Movie already exists");
                ModelState.AddModelError("Description", "Movie already exists");
                return View(movie);
            }

            await _service.UpdateAsync(id, movie);
            TempData["success"] = "Movie updated successfully";

            return RedirectToAction("Index", "Movie");
        }

        //GET: Actor/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var movieDetails = await _service.GetByIdAsync(id);
            if (movieDetails == null) return RedirectToAction("NotFound", "Error");


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
            var data = _context.moives.Where(x => x.Name.Equals(name)).SingleOrDefault();
            var data1 = _context.moives.Where(x => x.Description.Equals(description)).SingleOrDefault();

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
