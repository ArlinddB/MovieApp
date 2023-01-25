using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Models;
using Redeo.ViewModels;

namespace Redeo.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<ApplicationUser> passwordHasher, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            ViewBag.Category = GetCategory();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Movie");
            }

            return View(new LoginVM());
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            ViewBag.Category = GetCategory();

            if (!ModelState.IsValid) return View(login);

            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, login.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movie");
                    }
                }
            }
            return View(login);
        }

        [Route("Register")]
        public IActionResult Register()
        {
            ViewBag.Category = GetCategory();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Movie");
            }

            return View(new RegisterVM());
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
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
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, register.Password);

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (newUserResponse.Succeeded && await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult CreateEditor()
        {
            ViewBag.Category = GetCategory();

            return View(new RegisterVM());
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateEditor(RegisterVM register)
        {
            ModelState.Remove("ConfirmPassword");
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
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, register.Password);

            if (!await _roleManager.RoleExistsAsync(UserRoles.Editor))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Editor));

            if (newUserResponse.Succeeded && await _roleManager.RoleExistsAsync(UserRoles.Editor))
                await _userManager.AddToRoleAsync(newUser, UserRoles.Editor);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movie");
        }

        [Route("Profile/{name}")]
        public async Task<IActionResult> Profile(string? name)
        {
            ViewBag.Category = GetCategory();

            var a = await _userManager.FindByNameAsync(name);

            var res = new UserVM
            {
                FullName = a.FullName,
                UserName = a.UserName,
                Email = a.Email,
                Birthdate = a.Birthdate,
                Password = null,
                ConfirmPassword = null

            };
            return View(res);
        }

        [Route("Profile/{name}")]
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(UserVM user)
        {
            ViewBag.Category = GetCategory();

            var userEdit = await _userManager.FindByNameAsync(user.UserName);

            if (userEdit != null)
            {
                userEdit.FullName = user.FullName;
                userEdit.UserName = user.UserName;
                userEdit.Email = userEdit.Email;
                userEdit.Birthdate = user.Birthdate;
                if (user.Password != null)
                {
                    userEdit.PasswordHash = _passwordHasher.HashPassword(userEdit, user.Password);
                }
            }

            await _userManager.UpdateAsync(userEdit);

            TempData["success"] = "Updated successfully";

            return Redirect("/Profile/" + user.UserName);
        }
    }
}
