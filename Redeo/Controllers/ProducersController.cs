using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Data.Services;
using Redeo.Models;
using X.PagedList;

namespace Redeo.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Editor)]
    public class ProducersController : Controller
    {

        private readonly IProducersService _service;
        private readonly AppDbContext _context;

        public ProducersController(IProducersService service, AppDbContext context)
        {
          

            _service = service;
            _context = context; 

        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;

            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var producers = from a in _context.producers
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                producers = producers.Where(a => a.ProducersName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    producers = producers.OrderByDescending(a => a.ProducersName);
                    break;
                case "Date":
                    producers = producers.OrderBy(a => a.Birthdate);
                    break;
                case "date_desc":
                    producers = producers.OrderByDescending(a => a.Birthdate);
                    break;
                default:
                    producers = producers.OrderBy(a => a.ProducersName);
                    break;
            }
            return View(await producers.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
        }

        //GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProducersName", "ProfilePicture", "Biography", "Birthdate")] Producers producers)
        {
            if (!ModelState.IsValid)
            {
                return View(producers); 
            }

            var alreadyExists= await _context.producers.AnyAsync(x => x.ProducersName == producers.ProducersName);
            var alreadyExists2 = await _context.producers.AnyAsync(x => x.Birthdate == producers.Birthdate);
            if (alreadyExists && alreadyExists2)
            {
                ModelState.AddModelError("ProducersName", "Producer already exists!");
                ModelState.AddModelError("Birthdate", "Producer already exists!");
                return View(producers);
            }
            await _service.AddAsync(producers);
            TempData["success"] = "Producer added succesfully!";
            return RedirectToAction("Index", "Producers");



        }
        //GET: Category/Details/id
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var producersDetails = await _service.GetByIdAsync(id);
            if (producersDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(producersDetails);
        }

        //GET: Category/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var producersDetails = await _service.GetByIdAsync(id);
            if (producersDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(producersDetails);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("Id","ProducersName", "ProfilePicture", "Biography", "Birthdate")]Producers producers )
        {
            if (!ModelState.IsValid)
            {
                return View(producers);
            }

            var alreadyExists= await _context.producers.AnyAsync(x => x.ProducersName == producers.ProducersName);
            var alreadyExists2 = await _context.producers.AnyAsync(x => x.Birthdate == producers.Birthdate);

            if(alreadyExists && alreadyExists2)
            {
                ModelState.AddModelError("ProducersName", "Producer already exists!");
                ModelState.AddModelError("Birthdate", "Producer already exists!");
                return View(producers);
            }

            await _service.UpdateAsync(id, producers);

            TempData["success"] = "Producer updated succesfully!";
            return RedirectToAction("Index", "Producers");

        }

        //GET: Category/Delete/id

        public async Task<IActionResult> Delete(int id)
        {
            var producersDetails= await _service.GetByIdAsync(id);
            if(producersDetails == null)
            
                return RedirectToAction("NotFound", "Error");

                return View(producersDetails);
            
        }

        [HttpPost]

        public async Task<IActionResult>DeleteConfirmed(int id)
        {
            var producersDetail=await _service.GetByIdAsync(id);
            if (producersDetail == null) return RedirectToAction("NotFound", "Error");

            await _service.DeleteAsync(id);
            TempData["success"] = "Producer deleted succesfully!";
            return RedirectToAction("Index", "Producers");

        }

        //Checking if category exists
        public JsonResult ProducerAvailability(string? name, DateTime? birthdate)
        {
            System.Threading.Thread.Sleep(450);
            var data = _context.producers.Where(x => x.ProducersName.Equals(name)).SingleOrDefault();
            var data1 = _context.producers.Where(x => x.Birthdate.Equals(birthdate)).SingleOrDefault();

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
