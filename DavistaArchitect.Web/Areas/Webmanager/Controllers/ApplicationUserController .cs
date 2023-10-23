using Microsoft.AspNetCore.Mvc;
using DavistaArchitect.Models;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{
    [Area("Webmanager")]
    public class ApplicationUser : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;


        public ApplicationUser(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //var users = _userManager.Users
            //            .Select(u => new UserViewModel
            //            {
            //                Id = u.Id,
            //                Name = u.UserName,
            //                Email = u.Email
            //            })
            //            .ToList();

            //return View(users);

            var users = _unitOfWork.ApplicationUser.GetAll();
            return View(users);
        }


        //GET 
        public IActionResult Upsert(string? id)
        {
            UserViewModel userVM = new()
            {
                RoleList = _userManager.Users.Select(x => x.Id).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),

            };

            if (id == null || id.Count() == 0)
            {
                //create
                return View(userVM);
            }
            else
            {
                //update
                userVM.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
                return View(userVM);
            }

        }

        //Post
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpsertAsync(ApplicationUser obj, string? id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        obj = _userManager.Users.FirstOrDefault(u => u.Id == id);

        //        if (!string.IsNullOrEmpty(id) || id.Count() == 0)
        //        {
        //            user = await _userManager.FindByIdAsync(obj.Id);

        //            if (user == null)
        //            {
        //                // User not found, create a new one
        //                user = new IdentityUser
        //                {
        //                    UserName = obj.Name,
        //                    Email = obj.Email,
        //                };

        //                var result = await _userManager.CreateAsync(user);

        //                if (result.Succeeded)
        //                {
        //                    TempData["success"] = "User added successfully";
        //                }
        //                else
        //                {
        //                    TempData["error"] = "Data was not saved";
        //                }
        //                return RedirectToAction("Index");

        //            }
        //            else
        //            {
        //                user.UserName = obj.Name;
        //                user.Email = obj.Email;

        //                var result = await _userManager.UpdateAsync(user);

        //                if (result.Succeeded)
        //                {
        //                    TempData["success"] = "User updated successfully";
        //                }
        //                else
        //                {
        //                    TempData["error"] = "Data was not saved";
        //                }
        //                return RedirectToAction("Index");

        //            }
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);
        //}


    }

}
