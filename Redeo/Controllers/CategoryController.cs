using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Data.Services;
using Redeo.Models;
using X.PagedList;

namespace Redeo.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Editor)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        private readonly AppDbContext _context;

        public CategoryController(ICategoryService service, AppDbContext context)
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
            var pageSize = 2;


            ViewData["CurrentFilter"] = searchString;

            var categories = from a in _context.categories
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(a => a.CategoryName.Contains(searchString));
            }

            return View(await categories.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
        }

        //GET: Category/Create
        public IActionResult Create()
        {
            ViewBag.Category = GetCategory();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CategoryName")]Category category)
        {
            ModelState.Remove("Movies_Categories");
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var alreadyExists = await _context.categories.AnyAsync(x => x.CategoryName == category.CategoryName);
            if (alreadyExists)
            {
                ModelState.AddModelError("CategoryName", "Category already exists");
                return View(category);
            }

            await _service.AddAsync(category);
            TempData["success"] = "Category added successfully";
            return RedirectToAction("Index", "Category");
        }

        //GET: Category/Details/id
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Category = GetCategory();

            var categoryDetails = await _service.GetByIdAsync(id);
            if (categoryDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(categoryDetails);
        }
            
        //GET: Category/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Category = GetCategory();

            var categoryDetails = await _service.GetByIdAsync(id);
            if (categoryDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(categoryDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, CategoryName")]Category category)
        {
            ModelState.Remove("Movies_Categories");
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var alreadyExists = await _context.categories.AnyAsync(x => x.CategoryName == category.CategoryName);
            if (alreadyExists)
            {
                ModelState.AddModelError("CategoryName", "Category already exists");
                return View(category);
            }

            await _service.UpdateAsync(id, category);
            TempData["success"] = "Category updated successfully";

            return RedirectToAction("Index", "Category");
        }


        //GET: Category/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Category = GetCategory();

            var categoryDetails = await _service.GetByIdAsync(id);
            if (categoryDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(categoryDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryDetails = await _service.GetByIdAsync(id);
            if (categoryDetails == null) return RedirectToAction("NotFound", "Error");


            await _service.DeleteAsync(id);
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction("Index", "Category");
        }

        //Checking if category exists
        public JsonResult NameAvailability(String name)
        {
            System.Threading.Thread.Sleep(450);
            var data = _context.categories.Where(x => x.CategoryName.Equals(name)).SingleOrDefault();

            if (data != null)
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
