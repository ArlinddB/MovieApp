﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Data.Services;
using Redeo.Models;
using System.Linq.Dynamic.Core;
using X.PagedList;

namespace Redeo.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Editor)]
    public class ActorController : Controller
    {
        private readonly IActorService _service;

        private readonly AppDbContext _context;


        public ActorController(IActorService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? page)
        {

            var pageNumber = page ?? 1;
            var pageSize = 10;
            

            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var actors = from a in _context.actors
                         select a;

            ViewBag.Category = GetCategory();

            if(!String.IsNullOrEmpty(searchString))
            {
                actors = actors.Where(a => a.FullName.Contains(searchString));
            }

            switch(sortOrder)
            {
                case "name_desc":
                    actors = actors.OrderByDescending(a => a.FullName);
                    break;
                case "Date":
                    actors = actors.OrderBy(a => a.Birthdate);
                    break;
                case "date_desc":
                    actors = actors.OrderByDescending(a => a.Birthdate);
                    break;
                    default:
                    actors = actors.OrderBy(a => a.FullName);
                    break;
            }
            return View(await actors.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
        }



        //GET: Actor/Create
        public IActionResult Create()
        {
            ViewBag.Category = GetCategory();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL", "FullName", "Birthdate", "Bio")] Actor actor)
        {
            ModelState.Remove("Movies_Actors");
            ModelState.Remove("TvShows_Actors");
            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            var alreadyExists1 = await _context.actors.AnyAsync(x => x.FullName == actor.FullName);
            var alreadyExists2 = await _context.actors.AnyAsync(x => x.Birthdate == actor.Birthdate);


            if (alreadyExists1 && alreadyExists2)
            {
                ModelState.AddModelError("FullName", "Actor already exists");
                ModelState.AddModelError("Birthdate", "Actor already exists");
                return View(actor);
            }

            await _service.AddAsync(actor);
            TempData["success"] = "Actor added successfully";
            return RedirectToAction("Index", "Actor");
        }

        [AllowAnonymous]
        //GET: Actor/Details/id
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Category = GetCategory();

            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(actorDetails);
        }


        // GET: Actor/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Category = GetCategory();

            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
                return RedirectToAction("NotFound", "Error");

            return View(actorDetails);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id","ProfilePictureURL", "FullName", "Birthdate", "Bio")] Actor actor)
        {
            ModelState.Remove("Movies_Actors");
            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            var alreadyExists1 = await _context.actors.AnyAsync(x => x.FullName == actor.FullName);
            var alreadyExists2 = await _context.actors.AnyAsync(x => x.Birthdate == actor.Birthdate);


            if (alreadyExists1 && alreadyExists2)
            {
                ModelState.AddModelError("FullName", "Actor already exists");
                ModelState.AddModelError("Birthdate", "Actor already exists");
                return View(actor);
            }

            await _service.UpdateAsync(id, actor);
            TempData["success"] = "Actor updated successfully";

            return RedirectToAction("Index", "Actor");
        }




        //GET: Actor/Delete/id

        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Category = GetCategory();

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
        public JsonResult ActorAvailability(string? name, DateTime? birthdate)
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
