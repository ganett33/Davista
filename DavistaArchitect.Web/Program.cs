using Microsoft.EntityFrameworkCore;
using DavistaArchitect.DataAccess.Data;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.DataAccess.Repository;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using SmartBreadcrumbs.Extensions;
using Microsoft.AspNetCore.Identity.UI.Services;
using DavistaArchitect.Utility;
using DavistaArchitect.Models;
using Microsoft.AspNetCore.Authorization;
using DavistaArchitect.Utility.PermissionRequirement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection")
    ));



builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Add authorization policy provider and handler
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddRazorPages();


// Seeding roles
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<Program>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        //This will invoke the Method that seeds the default user to the database.
        await ContextSeed.SeedRolesAsync(userManager, roleManager); // Roles
        await ContextSeed.SeedBasicUserAsync(userManager, roleManager);  // Basic
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);  // SuperAdmin
        logger.LogInformation("Finished Seeding Default Data");
        logger.LogInformation("Application Starting");

    }
    catch (Exception ex)
    {

        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
        name: "projectshouse",
        pattern: "{area=Client}/{controller=Home}/Projects/{ProjectTypeUrl}/{HouseDetail}/{id?}",
        defaults: new { controller = "Home", action = "Project" }
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");

app.Run();
