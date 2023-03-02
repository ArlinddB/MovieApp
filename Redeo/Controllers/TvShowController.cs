using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Data.Services;
using Redeo.Models;
using Redeo.ViewModels;
using X.PagedList;

namespace Redeo.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Editor)]
    public class TvShowController : Controller
    {
        private readonly ITvShowService _service;
        private readonly AppDbContext _context;

        public TvShowController(ITvShowService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }
        public List<TvShow> MostWatchedTvShows()
        {
            var avgViews = 0.0;

            if (_context.tvShows.Any())
            {
                avgViews = _context.tvShows.Average(n => n.Clicks);
            }

            var topTvShows = _context.tvShows.Where(m => m.Clicks > avgViews).OrderByDescending(v => v.Clicks).Take(6).ToList();

            return topTvShows;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? page)
        {
            ViewBag.Category = GetCategory();

            ViewBag.MostWatchedTvShows = MostWatchedTvShows();

            var pageNumber = page ?? 1;
            var pageSize = 10;

            return View(await _context.tvShows.ToPagedListAsync(pageNumber, pageSize));
        }

        public async Task<IActionResult> List(string searchString, int? page)
        {
            ViewBag.Category = GetCategory();

            var pageNumber = page ?? 1;
            var pageSize = 10;

            ViewData["CurrentFilter"] = searchString;

            var tvShows = from a in _context.tvShows
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                tvShows = tvShows.Where(a => a.Name.Contains(searchString));
            }

            return View(await tvShows.AsNoTracking().Include(n => n.Seasons).ToPagedListAsync(pageNumber, pageSize));
        }

        //Get:TvShow/Create
        public async Task<IActionResult> Create()
        {
            var tvShowDropDowns = await _service.GetNewTvShowDropdownsValues();

            ViewBag.Category = GetCategory();

            ViewBag.Categories = new SelectList(tvShowDropDowns.Categories, "Id", "CategoryName");
            ViewBag.Producers = new SelectList(tvShowDropDowns.Producers, "Id", "ProducersName");
            ViewBag.Actors = new SelectList(tvShowDropDowns.Actors, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TvShowVM tvShow)
        {
            if (!ModelState.IsValid)
            {
                var tvShowDropDowns = await _service.GetNewTvShowDropdownsValues();

                ViewBag.Categories = new SelectList(tvShowDropDowns.Categories, "Id", "CategoryName");
                ViewBag.Producers = new SelectList(tvShowDropDowns.Producers, "Id", "ProducersName");
                ViewBag.Actors = new SelectList(tvShowDropDowns.Actors, "Id", "FullName");

                return View(tvShow);
            }

            await _service.AddNewTvShowAsync(tvShow);
            TempData["success"] = "Tv show added successfully";
            return RedirectToAction(nameof(Index));
        }

        //GET:TvShow/Details/id
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Category = GetCategory();

            var tvShowDetails = await _service.GetTvShowByIdAsync(id);

            tvShowDetails.Clicks += 1;

            //Similar Tv Shows

            var genreId = tvShowDetails.TvShows_Categories.Select(tsh => tsh.CategoryId).ToList();
            var similarTvShows = _context.tvShows
                .Where(t => t.TvShows_Categories.Any(tsh => genreId.Contains(tsh.CategoryId)) && t.Id != id)
                .ToList().Take(6);

            ViewBag.SimilarTvShows = similarTvShows;

            await _service.UpdateAsync(id, tvShowDetails);

            if (tvShowDetails == null)
                return BadRequest("NotFound");

            return View(tvShowDetails);
        }

        // GET: TvShow/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var tvShowDetails = await _service.GetTvShowByIdAsync(id);
            if (tvShowDetails == null)
                return RedirectToAction("NotFound", "Error");

            ViewBag.Category = GetCategory();

            var response = new TvShowVM()
            {
                Id = tvShowDetails.Id,
                Name = tvShowDetails.Name,
                TvShowPicture = tvShowDetails.TvShowPicture,
                Description = tvShowDetails.Description,
                DateOfRelease = tvShowDetails.DateOfRelease,
                Duration = tvShowDetails.Duration,
                Quality = tvShowDetails.Quality,
                CategoryIds = tvShowDetails.TvShows_Categories.Select(x => x.CategoryId).ToList(),
                ProducerId = tvShowDetails.ProducerId,
                ActorIds = tvShowDetails.TvShows_Actors.Select(c => c.ActorId).ToList()
            };

            var tvShowDropdowns = await _service.GetNewTvShowDropdownsValues();

            ViewBag.Categories = new SelectList(tvShowDropdowns.Categories, "Id", "CategoryName");
            ViewBag.Producers = new SelectList(tvShowDropdowns.Producers, "Id", "ProducersName");
            ViewBag.Actors = new SelectList(tvShowDropdowns.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TvShowVM tvShow)
        {
            if (id != tvShow.Id)
                return RedirectToAction("NotFound", "Error");

            if (!ModelState.IsValid)
            {
                var tvShowDropdowns = await _service.GetNewTvShowDropdownsValues();

                ViewBag.Categories = new SelectList(tvShowDropdowns.Categories, "Id", "CategoryName");
                ViewBag.Producers = new SelectList(tvShowDropdowns.Producers, "Id", "ProducersName");
                ViewBag.Actors = new SelectList(tvShowDropdowns.Actors, "Id", "FullName");

                return View(tvShow);
            }

            await _service.UpdateTvShowAsync(tvShow);
            TempData["success"] = "Tv show updated successfully";

            return RedirectToAction("Index", "TvShow");
        }

        //GET: TvShow/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Category = GetCategory();

            var tvShow = await _service.GetTvShowByIdAsync(id);
            if (tvShow == null)
                return RedirectToAction("NotFound", "Error");

            return View(tvShow);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tvShow = await _service.GetTvShowByIdAsync(id);
            if (tvShow == null) return RedirectToAction("NotFound", "Error");

            await _service.DeleteAsync(id);
            TempData["success"] = "Tv show deleted successfully";

            return RedirectToAction("Index", "Movie");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ClueTip(int id)
        {
            return View(await _context.tvShows.Where(x => x.Id == id).SingleOrDefaultAsync());
        }
    }
}
