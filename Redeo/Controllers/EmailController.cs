using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redeo.Data;
using Redeo.Models;

namespace Redeo.Controllers
{
    public class EmailController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public EmailController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<Category> GetCategory()
        {
            return _context.categories.ToList();
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            ViewBag.Category = GetCategory();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                TempData["error"] = "An error occurred while processing your request. Please try again later.";

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["success"] = "Email confirmed successfuly";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["error"] = "An error occurred while processing your request. Please try again later.";
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
