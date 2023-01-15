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
    public class SeasonController : Controller
    {
        private readonly ISeasonService _service;
        private readonly AppDbContext _context;
        public SeasonController(ISeasonService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }
        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }

        public async Task<IActionResult> Index(string searchString, int? page)
        {

            ViewBag.Category = GetCategory();

            var pageNumber = page ?? 1;
            var pageSize = 10;

            ViewData["CurrentFilter"] = searchString;

            var seasons = from a in _context.seasons
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                seasons = seasons.Where(a => a.Season.Contains(searchString) || a.TvShow.Name.Contains(searchString));
            }

            return View(await seasons.AsNoTracking().Include(n => n.TvShow).ToPagedListAsync(pageNumber, pageSize));
        }

        //Get:Season/Create
        public async Task<IActionResult> Create()
        {
            var seasonDropdowns = await _service.GetNewSeasonDropdownsValues();

            ViewBag.Category = GetCategory();

            ViewBag.TvShows = new SelectList(seasonDropdowns.TvShows, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeasonVM seasonn)
        {
            if (!ModelState.IsValid)
            {
                var seasonDropdowns = await _service.GetNewSeasonDropdownsValues();

                ViewBag.TvShows = new SelectList(seasonDropdowns.TvShows, "Id", "Name");

                return View(seasonn);
            }

            await _service.AddNewSeasonAsync(seasonn);
            TempData["success"] = "Season added successfully";
            return RedirectToAction(nameof(Index));
        }

        //GET:TvShow/Details/id
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Category = GetCategory();

            var seasonDetails = await _service.GetSeasonByIdAsync(id);

            await _service.UpdateAsync(id, seasonDetails);

            if (seasonDetails == null)
                return BadRequest("NotFound");

            return View(seasonDetails);
        }

        // GET: TvShow/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var seasonDetails = await _service.GetSeasonByIdAsync(id);
            if (seasonDetails == null)
                return RedirectToAction("NotFound", "Error");

            ViewBag.Category = GetCategory();

            var response = new SeasonVM()
            {
                Season = seasonDetails.Season,
                TvShowId = seasonDetails.TvShowId
            };

            var seasonDropdowns = await _service.GetNewSeasonDropdownsValues();

            ViewBag.TvShows = new SelectList(seasonDropdowns.TvShows, "Id", "Name");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SeasonVM seasonn)
        {
            if (id != seasonn.Id)
                return RedirectToAction("NotFound", "Error");

            if (!ModelState.IsValid)
            {
                var seasonDropdowns = await _service.GetNewSeasonDropdownsValues();

                ViewBag.TvShows = new SelectList(seasonDropdowns.TvShows, "Id", "Name");

                return View(seasonn);
            }

            await _service.UpdateSeasonAsync(seasonn);
            TempData["success"] = "Season updated successfully";

            return RedirectToAction(nameof(Index));
        }

        //GET: TvShow/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Category = GetCategory();

            var season = await _service.GetSeasonByIdAsync(id);
            if (season == null)
                return RedirectToAction("NotFound", "Error");

            return View(season);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var season = await _service.GetSeasonByIdAsync(id);
            if (season == null) return RedirectToAction("NotFound", "Error");

            await _service.DeleteAsync(id);
            TempData["success"] = "Season deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
