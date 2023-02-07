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
    public class EpisodeController : Controller
    {
        private readonly IEpisodeService _service;
        private readonly AppDbContext _context;

        public EpisodeController(IEpisodeService service, AppDbContext context)
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

            var episodes = from a in _context.episodes
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                episodes = episodes.Where(a => a.Episode.Contains(searchString) || a.Season.TvShow.Name.Contains(searchString));
            }

            return View(await episodes.AsNoTracking()
                .Include(n => n.Season)
                .Include(m => m.TvShow)
                .ToPagedListAsync(pageNumber, pageSize));
        }

        //Get:Episode/Create
        public async Task<IActionResult> Create()
        {
            var episodeDropdowns = await _service.GetNewEpisodeDropdownsValues();

            ViewBag.Category = GetCategory();

            ViewBag.Seasons = new SelectList(episodeDropdowns.Seasons, "Id", "Season");
            ViewBag.TvShows = new SelectList(episodeDropdowns.TvShows, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EpisodeVM episodee)
        {
            if (!ModelState.IsValid)
            {
                var episodeDropdowns = await _service.GetNewEpisodeDropdownsValues();

                ViewBag.Seasons = new SelectList(episodeDropdowns.Seasons, "Id", "Season");
                ViewBag.TvShows = new SelectList(episodeDropdowns.TvShows, "Id", "Name");

                return View(episodee);
            }

            await _service.AddNewEpisodeAsync(episodee);
            TempData["success"] = "Episode added successfully";
            return RedirectToAction(nameof(Index));
        }

        public List<TSeason> GetSeasons()
        {
            return _context.seasons.ToList();
        }
        public List<TEpisodes> GetEpisodes()
        {
            return _context.episodes.ToList();
        }

        //GET:Episode/Details/id
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Category = GetCategory();

            ViewBag.Seasons = GetSeasons();

            ViewBag.Episodes = GetEpisodes();

            var episodeDetails = await _service.GetEpisodeByIdAsync(id);

            await _service.UpdateAsync(id, episodeDetails);

            if (episodeDetails == null)
                return BadRequest("NotFound");

            return View(episodeDetails);
        }

        // GET: Episode/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var episodeDetails = await _service.GetEpisodeByIdAsync(id);

            if (episodeDetails == null)
                return RedirectToAction("NotFound", "Error");

            ViewBag.Category = GetCategory();

            var response = new EpisodeVM()
            {
                Episode = episodeDetails.Episode,
                SeasonId = episodeDetails.SeasonId
            };

            var episodeDropdowns = await _service.GetNewEpisodeDropdownsValues();

            ViewBag.Seasons = new SelectList(episodeDropdowns.Seasons, "Id", "Season");
            ViewBag.TvShows = new SelectList(episodeDropdowns.TvShows, "Id", "Name");


            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EpisodeVM episodee)
        {
            if (id != episodee.Id)
                return RedirectToAction("NotFound", "Error");

            if (!ModelState.IsValid)
            {
                var episodeDropdowns = await _service.GetNewEpisodeDropdownsValues();

                ViewBag.Seasons = new SelectList(episodeDropdowns.Seasons, "Id", "Season");
                ViewBag.TvShows = new SelectList(episodeDropdowns.TvShows, "Id", "Name");

                return View(episodee);
            }

            await _service.UpdateEpisodeAsync(episodee);
            TempData["success"] = "Episode updated successfully";

            return RedirectToAction(nameof(Index));
        }

        //GET: Episode/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Category = GetCategory();

            var episode = await _service.GetEpisodeByIdAsync(id);
            if (episode == null)
                return RedirectToAction("NotFound", "Error");

            return View(episode);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var episode = await _service.GetEpisodeByIdAsync(id);
            if (episode == null) return RedirectToAction("NotFound", "Error");

            await _service.DeleteAsync(id);
            TempData["success"] = "Episode deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
