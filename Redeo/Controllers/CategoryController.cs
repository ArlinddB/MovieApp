using Microsoft.AspNetCore.Mvc;
using Redeo.Data.Services;
using Redeo.Models;

namespace Redeo.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service; 
        }
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
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

            await _service.AddAsync(category);
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
            await _service.UpdateAsync(id, category);
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
            return RedirectToAction("Index", "Category");
        }
    }
}
