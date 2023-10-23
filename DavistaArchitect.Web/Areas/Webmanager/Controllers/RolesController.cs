using DavistaArchitect.Models;
using DavistaArchitect.Models.Enums;
using DavistaArchitect.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{
    [Area("Webmanager")]
    [Authorize(Roles = "Administrator, SubAdministrator")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }


        //GET

        public async Task<IActionResult> Edit(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return NotFound(); // Handle the case where roleId is empty or null
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound(); // Handle the case where the role with the given ID is not found
            }

            return View(role);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            // Retrieve the role you want to update
            var existingRole = await _roleManager.FindByIdAsync(role.Id);


            if (existingRole == null)
            {
                // Handle the case where the role does not exist
                TempData["error"] = "Role not found";
                return RedirectToAction("Index"); // You can choose to handle this differently.
            }

            // Update the properties of the existing role
            existingRole.Name = role.Name; // Modify other properties as needed

            // Save the changes
            var result = await _roleManager.UpdateAsync(existingRole);

            if (result.Succeeded)
            {
                TempData["success"] = "Role updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                // Handle errors if the update fails, such as displaying an error message or logging the errors.
                // You can access the errors using result.Errors.
                TempData["error"] = "Role update failed";
                return RedirectToAction("Index"); // You can choose to handle this differently.
            }

        }





        //GET
        // controller= Roles, action= Edit  
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return NotFound();
            }

            var rolesViewModel = new RolesVM
            {
                RoleId = role.Id,
                RoleName = role.Name
            };



            return View(rolesViewModel);
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, SubAdministrator")]
        public async Task<IActionResult> DeletePost(string roleId)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return NotFound();
            }

            // Delete the role
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                TempData["success"] = "Role deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                // Handle errors if the deletion fails, such as displaying an error message or logging the errors.
                // You can access the errors using result.Errors.
                TempData["error"] = "Role deletion failed";
                return RedirectToAction("Index"); // You can choose to handle this differently.
            }

        }



    }
}
