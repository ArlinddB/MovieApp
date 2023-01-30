using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redeo.Areas.Admin.ViewModels;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Models;
using System.Linq;
using System.Linq.Dynamic.Core;
using X.PagedList;

namespace Redeo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(AppDbContext context, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
        }
        [Route("/User")]
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            
            return View(await _userManager.Users.ToPagedListAsync(pageNumber, pageSize));
        }

        [Route("/User/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var userDetails = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            var user = new UserVM()
            {
                FullName = userDetails.FullName,
                UserName = userDetails.UserName,
                Email = userDetails.Email,
                Birthdate = userDetails.Birthdate,
                Password = null
            };

            return View(user);
        }

        [Route("/User/Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserVM user)
        {
            var userDetails = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userDetails != null)
            {
                userDetails.FullName = user.FullName;
                userDetails.UserName = user.UserName;
                userDetails.Email = user.Email;
                userDetails.Birthdate = user.Birthdate;
                if(user.Password != null)
                {
                    userDetails.PasswordHash = _passwordHasher.HashPassword(userDetails, user.Password);
                }
            }

            await _userManager.UpdateAsync(userDetails);

            TempData["success"] = "Updated successfully";

            return RedirectToAction("Index", "User");
        }

        [Route("/NormalUsers")]
        public IActionResult NormalUser(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;

            var normalUsers = _userManager.GetUsersInRoleAsync("User").Result;
            return View(normalUsers.ToPagedList(pageNumber, pageSize));
        }

        [Route("/User/CreateUser")]
        public IActionResult CreateUser()
        {
            return View(new RegisterVM());
        }

        [Route("/User/CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);

            var user = await _userManager.FindByNameAsync(register.UserName);
            if (user != null)
            {
                return View(register);
            }

            var newUser = new ApplicationUser()
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Birthdate = register.Birthdate,
                Email = register.Email,
                EmailConfirmed = true
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, register.Password);


            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (newUserResponse.Succeeded && await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            //if (newUserResponse.Succeeded)
            //{
            //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            //    var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = register.Email }, Request.Scheme);
            //    EmailHelper emailHelper = new EmailHelper();
            //    bool emailResponse = emailHelper.SendEmailConfirmation(register.Email, confirmationLink);

            //    if (emailResponse)
            //    {
            //        TempData["success"] = "Check your email to verify your account!";
            //        return RedirectToActionPermanent("NormalUser", "User");
            //    }
            //}
            return RedirectToActionPermanent("NormalUser", "User");
        }

        [Route("/Editors")]
        public IActionResult Editors(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;

            var normalUsers = _userManager.GetUsersInRoleAsync("Editor").Result;
            return View(normalUsers.ToPagedList(pageNumber, pageSize));
        }

        [Route("/User/CreateEditor")]
        public IActionResult CreateEditor()
        {
            return View(new RegisterVM());
        }
        
        [Route("/User/CreateEditor")]
        [HttpPost]
        public async Task<IActionResult> CreateEditor(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);

            var user = await _userManager.FindByNameAsync(register.UserName);
            if (user != null)
            {
                return View(register);
            }

            var newUser = new ApplicationUser()
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Birthdate = register.Birthdate,
                Email = register.Email,
                EmailConfirmed = true
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, register.Password);

            if (!await _roleManager.RoleExistsAsync(UserRoles.Editor))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Editor));

            if (newUserResponse.Succeeded && await _roleManager.RoleExistsAsync(UserRoles.Editor))
                await _userManager.AddToRoleAsync(newUser, UserRoles.Editor);

            return RedirectToAction("Editors", "User");
        }

        [Route("/Admins")]
        public IActionResult Admins(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;

            var normalUsers = _userManager.GetUsersInRoleAsync("Admin").Result;
            return View(normalUsers.ToPagedList(pageNumber, pageSize));
        }

        [Route("/User/CreateAdmin")]
        public IActionResult CreateAdmin()
        {
            return View(new RegisterVM());
        }

        [Route("/User/CreateAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);

            var user = await _userManager.FindByNameAsync(register.UserName);
            if (user != null)
            {
                return View(register);
            }

            var newUser = new ApplicationUser()
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Birthdate = register.Birthdate,
                Email = register.Email,
                EmailConfirmed = true
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, register.Password);

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (newUserResponse.Succeeded && await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);

            return RedirectToAction("Admins", "User");
        }
    }
}
