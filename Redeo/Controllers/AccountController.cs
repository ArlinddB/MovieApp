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
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Login()
        {
            return View(new LoginVM());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
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

        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

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
    }
}
