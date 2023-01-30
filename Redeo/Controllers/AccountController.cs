using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redeo.Data;
using Redeo.Data.Roles;
using Redeo.Models;
using Redeo.ViewModels;
using System.ComponentModel.DataAnnotations;

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
                bool emailStatus = await _userManager.IsEmailConfirmedAsync(user);
                if (emailStatus == false)
                {
                    TempData["error"] = "Email is unconfirmed, please confirm it first";
                    return View("Login");
                }

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
            ViewBag.Category = GetCategory();

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
            
            if (newUserResponse.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = register.Email }, Request.Scheme);
                EmailHelper emailHelper = new EmailHelper();
                bool emailResponse = emailHelper.SendEmailConfirmation(register.Email, confirmationLink);

                if (emailResponse)
                {
                    TempData["success"] = "Check your email to verify your account!";
                    return RedirectToAction("Login", "Account");
                }
                    
                else
                {
                    // log email failed 
                }
            }
            return View("Register");
        }

        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult CreateEditor()
        {
            ViewBag.Category = GetCategory();

            return View(new RegisterVM());
        }        

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movie");
        }

        [Route("Profile/{name}")]
        [Authorize]
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
        [Authorize]
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

        public IActionResult ForgotPassword()
        {
            ViewBag.Category = GetCategory();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            ViewBag.Category = GetCategory();

            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmailPasswordReset(user.Email, link);

            if (emailResponse)
            {
                TempData["success"] = "The reset password link has been sent. Please check your email to continue.";
                return View();
            }
            else
            {
                // log email failed 
            }
            return View("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Category = GetCategory();

            return View();
        }

        public IActionResult ResetPassword(string token, string email)
        {
            ViewBag.Category = GetCategory();

            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                TempData["error"] = "User was not found";
                return View(resetPassword);
            }


            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return View();
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        public IActionResult ResetPasswordConfirmation()
        {
            ViewBag.Category = GetCategory();

            return View();
        }
    }
}
