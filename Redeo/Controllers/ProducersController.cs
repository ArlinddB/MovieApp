using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Services;
using Redeo.Models;
using X.PagedList;

namespace Redeo.Controllers
{
    public class ProducersController : Controller
    {

        private readonly IProducersService _service;
        private readonly AppDbContext _context;

        public ProducersController(IProducersService service, AppDbContext context)
        {
          

            _service = service;
            _context = context; 

        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var a = await _context.producers.ToPagedListAsync(pageNumber, pageSize);
            return View(a);
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
                ModelState.AddModelError("ProducersName", "Producer alreday exists!");
                return View(producers);
            }
            await _service.AddAsync(producers);
            TempData["success"] = "Producer added succesfully!";
            return RedirectToAction("Index", "Producers");



        }
        //GET: Category/Details/id
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
        public JsonResult ProducerAvailability(string name, DateTime dob)
        {
            System.Threading.Thread.Sleep(450);
            var alreadyExists = _context.producers.Where(x => x.ProducersName.Equals(name)).SingleOrDefault();
            var alreadyExists2 = _context.producers.Where(x => x.Birthdate.Equals(dob)).SingleOrDefault();
            if(alreadyExists != null && alreadyExists2 != null)
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
