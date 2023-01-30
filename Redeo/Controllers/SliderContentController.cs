using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Models;
using X.PagedList;

namespace Redeo.Controllers
{
    public class SliderContentController : Controller
    {
        private readonly AppDbContext _context;

        public SliderContentController(AppDbContext context)
        {
            _context = context;
        }
        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }
        // GET: SliderContent
        public async Task<IActionResult> Index(int? page)
        {
            ViewBag.Category = GetCategory();

            var pageNumber = page ?? 1;
            var pageSize = 10;

            return View(await _context.SliderContents.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: SliderContent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Category = GetCategory();

            if (id == null || _context.SliderContents == null)
            {
                return NotFound();
            }

            var sliderContent = await _context.SliderContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sliderContent == null)
            {
                return NotFound();
            }

            return View(sliderContent);
        }

        // GET: SliderContent/Create
        public IActionResult Create()
        {
            ViewBag.Category = GetCategory();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,Description,Picture,Quality")] SliderContent sliderContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sliderContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sliderContent);
        }

        // GET: SliderContent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Category = GetCategory();

            if (id == null || _context.SliderContents == null)
            {
                return NotFound();
            }

            var sliderContent = await _context.SliderContents.FindAsync(id);
            if (sliderContent == null)
            {
                return NotFound();
            }
            return View(sliderContent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,Description,Picture,Quality")] SliderContent sliderContent)
        {
            if (id != sliderContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sliderContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderContentExists(sliderContent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sliderContent);
        }

        // GET: SliderContent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Category = GetCategory();

            if (id == null || _context.SliderContents == null)
            {
                return NotFound();
            }

            var sliderContent = await _context.SliderContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sliderContent == null)
            {
                return NotFound();
            }

            return View(sliderContent);
        }

        // POST: SliderContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SliderContents == null)
            {
                return Problem("Entity set 'AppDbContext.SliderContents'  is null.");
            }
            var sliderContent = await _context.SliderContents.FindAsync(id);
            if (sliderContent != null)
            {
                _context.SliderContents.Remove(sliderContent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderContentExists(int id)
        {
            return _context.SliderContents.Any(e => e.Id == id);
        }
    }
}
