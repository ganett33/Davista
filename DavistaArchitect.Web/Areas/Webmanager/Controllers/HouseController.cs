using Microsoft.AspNetCore.Mvc;
using DavistaArchitect.Models;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{
    [Area("Webmanager")]
    [Authorize(Roles = "Administrator, CMSUser")]
    public class HouseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HouseController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        //GET
        // controller= House, action= Edit  
        public IActionResult Upsert(int? id)
        {
            HouseVM houseVM = new()
            {
                House = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ProjectTypeList = _unitOfWork.ProjectType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.ProjectTypeName,
                    Value = i.Id.ToString()
                })

            };

            if (id == null || id == 0)
            {
                //create
                return View(houseVM);
            }
            else
            {
                //update
                houseVM.House = _unitOfWork.House.GetFirstOrDefault(u => u.Id == id);
                return View(houseVM);
            }


        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(HouseVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = obj.House.HouseName;
                    string path = Path.Combine(wwwRootPath, @$"images\house\{fileName}\");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }



                    var extension = Path.GetExtension(file.FileName);

                    if (obj.House.HouseImage != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.House.HouseImage.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.House.HouseImage = @"\images\house\" + fileName + "\\" + fileName + extension;
                }

                if (obj.House.Id == 0)
                {
                    _unitOfWork.House.Add(obj.House);
                }
                else
                {
                    _unitOfWork.House.Update(obj.House);

                }


                _unitOfWork.Save();
                TempData["success"] = "House added successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var houseList = _unitOfWork.House.GetAll(includeProperties: "Category,ProjectType");
            return Json(new { data = houseList });
        }


        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, CMSUser")]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.House.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.HouseImage.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.House.Remove(obj);
            _unitOfWork.Save();

            TempData["success"] = "House deleted successfully";
            return RedirectToAction("Index");

        }


        #endregion
    }

}
