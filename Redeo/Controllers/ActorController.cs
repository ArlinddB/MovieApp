    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Services;
using Redeo.Models;

namespace Redeo.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorService _service;

        private readonly AppDbContext _context;


        public ActorController(IActorService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        //GET: Actor/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create([Bind("ProfilePictureURL", "FullName","Birthdate","Bio")]Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            var alreadyExists1 = await _context.actors.AnyAsync(x => x.FullName == actor.FullName);
            var alreadyExists2 = await _context.actors.AnyAsync(x => x.Birthdate == actor.Birthdate);
            

            if (alreadyExists1 && alreadyExists2 )
            {
                ModelState.AddModelError("FullName", "Actor already exists");
                return View(actor);
            }

            await _service.AddAsync(actor);
            TempData["success"] = "Actor added successfully";
            return RedirectToAction("Index", "Actor");
        }



        //GET: Actor/Details/id
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(actorDetails);
        }


        // GET: Actor/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(actorDetails);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ProfilePictureURL", "FullName", "Birthdate", "Bio")] Actor actor)
        {
            if(!ModelState.IsValid)
            {
                return View(actor);
            }

            var alreadyExists = await _context.actors.AnyAsync(x => x.Id == actor.Id);

            if(alreadyExists)
            {
                ModelState.AddModelError("FullName", "Already exist");
                return View(actor);
            }

            await _service.UpdateAsync(id, actor);
            TempData["success"] = "Actor updated successfully";

            return RedirectToAction("Index", "Actor");
        }




        //GET: Actor/Delete/id

        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return RedirectToAction("NotFound", "Error");


            return View(actorDetails);
        }


        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return RedirectToAction("NotFound", "Error");

            await _service.DeleteAsync(id);
            TempData["success"] = "Actor deleted successfully";

            return RedirectToAction("Index", "Actor");

        }



        //Checking if category exists
        public JsonResult ActorAvailability(String name, DateTime birthdate)
        {
            System.Threading.Thread.Sleep(450);
            var data = _context.actors.Where(x => x.FullName.Equals(name)).SingleOrDefault();
            var data1 = _context.actors.Where(x => x.Birthdate.Equals(birthdate)).SingleOrDefault();

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
