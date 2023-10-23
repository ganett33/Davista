using DavistaArchitect.Models;
using DavistaArchitect.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{

    [Area("Webmanager")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();

            return View(allUsersExceptCurrentUser);
        }


        public async Task<IActionResult> ManagePassword(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user doesn't exist
            }

            var model = new ChangePasswordVM { UserId = userId, UserName = user.UserName };
            return View(model);
        }

        // Handle the password change request
        [HttpPost]
        public async Task<IActionResult> ManagePassword(ChangePasswordVM obj)
        {
            
            var user = await _userManager.FindByIdAsync(obj.UserId);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user doesn't exist
            }

            // Verify the old password
            var result = await _userManager.ChangePasswordAsync(user, obj.OldPassword, obj.NewPassword);
            if (result.Succeeded)
            {
                // Optionally, you can sign the user out or take other actions
                TempData["success"] = "Password changed successfully";
                return RedirectToAction("Index", "Users"); // Redirect to a success page
            }
            else
            {
                // Handle password change failure (e.g., validation errors)
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["error"] = "Password doesn't matched";
                return View(obj); // Return to the change password form with errors
            }
        }


    }
}
