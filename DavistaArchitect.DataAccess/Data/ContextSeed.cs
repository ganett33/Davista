using DavistaArchitect.Models;
using DavistaArchitect.Models.Constants;
using DavistaArchitect.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DavistaArchitect.DataAccess.Data
{
    public static class ContextSeed
    {       
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.SubAdministrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.CMSUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.WebDeveloper.ToString()));
        }

        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "basic_davista",
                Email = "basic_davista@davista.biz",
                AboutMe = null,
                JobTitle = null,
                PhoneNumber = null,
                Role = "Basic",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Davista!2!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }

            }
        }


        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "admin_davista",
                Email = "superadmin@davista.biz",
                AboutMe = null,
                JobTitle = null,
                PhoneNumber = null,
                Role = "Administrator",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Davista!2");
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.SubAdministrator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.CMSUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.WebDeveloper.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Administrator");
            await roleManager.AddPermissionClaim(adminRole, "Category");
            await roleManager.AddPermissionClaim(adminRole, "StaffProfile");
            await roleManager.AddPermissionClaim(adminRole, "ProjectType");
            await roleManager.AddPermissionClaim(adminRole, "House");
            await roleManager.AddPermissionClaim(adminRole, "UserRoles");
            await roleManager.AddPermissionClaim(adminRole, "Roles");

        }

        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }



    }
}
