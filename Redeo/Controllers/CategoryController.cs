using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Services;
using Redeo.Models;
using X.PagedList;

namespace Redeo.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        private readonly AppDbContext _context;

        public CategoryController(ICategoryService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _service.GetAllAsync());
        //}

        public async Task<IActionResult> Index(string searchString/*int? page*/)
        {
            //var pageNumber = page ?? 1;
            //var pageSize = 2;
            //var a = await _context.categories.ToPagedListAsync(pageNumber, pageSize);
            //return View(a);

            ViewData["CurrentFilter"] = searchString;

            var categories = from a in _context.categories
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(a => a.CategoryName.Contains(searchString));
            }

            return View(await categories.AsNoTracking().ToListAsync());
        }

        //GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CategoryName")]Category category)
        {
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
        public async Task<IActionResult> Details(int id)
        {
            var categoryDetails = await _service.GetByIdAsync(id);
            if (categoryDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(categoryDetails);
        }
            
        //GET: Category/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var categoryDetails = await _service.GetByIdAsync(id);
            if (categoryDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(categoryDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, CategoryName")]Category category)
        {
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
