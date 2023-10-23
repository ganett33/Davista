using DavistaArchitect.Models;
using DavistaArchitect.Models.Constants;
using DavistaArchitect.Models.ViewModels;
using DavistaArchitect.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{
    [Area("Webmanager")]
    [Authorize(Roles = "Administrator, SubAdministrator")]

    public class PermissionController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public PermissionController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<ActionResult> Index(string roleId)
        {
            var model = new PermissionVM();
            var allPermissions = new List<RoleClaimsVM>();
            allPermissions.GetPermissions(typeof(Permissions.Category), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ProjectType), roleId);
            allPermissions.GetPermissions(typeof(Permissions.StaffProfile), roleId);
            allPermissions.GetPermissions(typeof(Permissions.House), roleId);
            allPermissions.GetPermissions(typeof(Permissions.UserRoles), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Roles), roleId);
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            model.RoleName = role.Name;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(PermissionVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var user = await _userManager.FindByIdAsync(model.UserId);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value); //ClaimHelper
            }
            TempData["success"] = "Permission updated successfully";
            return RedirectToAction("Index", "Roles");
        }
    }
}
