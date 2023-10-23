using Microsoft.AspNetCore.Mvc;
using DavistaArchitect.Models;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{
    [Area("Webmanager")]
    [Authorize(Roles = "Administrator, SubAdministrator")]
    public class StaffProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StaffProfileController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }


        //GET
        // controller= StaffProfile, action= Edit  
        public IActionResult Upsert(int? id)
        {
            StaffProfileVM staffProfileVM = new()
            {
                StaffProfile = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };

            if (id == null || id == 0)
            {
                //create
                return View(staffProfileVM);
            }
            else
            {
                //update
                staffProfileVM.StaffProfile = _unitOfWork.StaffProfile.GetFirstOrDefault(u => u.Id == id);
                return View(staffProfileVM);
            }


        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StaffProfileVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string path = Path.Combine(wwwRootPath, @"images\staff");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (file != null)
                {
                    string fileName = obj.StaffProfile.FullName;
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.StaffProfile.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.StaffProfile.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.StaffProfile.ImageUrl = @"\images\staff\" + fileName + extension;
                }

                if (obj.StaffProfile.Id == 0)
                {
                    _unitOfWork.StaffProfile.Add(obj.StaffProfile);
                }
                else
                {
                    _unitOfWork.StaffProfile.Update(obj.StaffProfile);

                }


                _unitOfWork.Save();
                TempData["success"] = "StaffProfile added successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var staffList = _unitOfWork.StaffProfile.GetAll(includeProperties: "Category");
            return Json(new { data = staffList });
        }


        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.StaffProfile.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.StaffProfile.Remove(obj);
            _unitOfWork.Save();


            TempData["success"] = "Staff profile deleted successfully";
            return RedirectToAction("Index");

        }

        #endregion
    }

}
