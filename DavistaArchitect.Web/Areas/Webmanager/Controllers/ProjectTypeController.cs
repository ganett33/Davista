using Microsoft.AspNetCore.Mvc;
using DavistaArchitect.Models;
using DavistaArchitect.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{
    [Area("Webmanager")]
    [Authorize(Roles = "Administrator, CMSUser")]
    public class ProjectTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProjectTypeController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<ProjectType> objProjectTypeList = _unitOfWork.ProjectType.GetAll();
            return View(objProjectTypeList);
        }

        // GET: ProjectType/Create
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectType obj, IFormFile projectTypeFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string path = Path.Combine(wwwRootPath, @"images\project-types\");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (projectTypeFile != null)
                {
                    string fileName = obj.ProjectTypeName;
                    var extension = Path.GetExtension(projectTypeFile.FileName);

                    if (obj.ProjectTypeImage != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ProjectTypeImage.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                    {
                        projectTypeFile.CopyTo(fileStreams);
                    }
                    obj.ProjectTypeImage = @"images\project-types\" +  fileName + extension;
                }

                _unitOfWork.ProjectType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "ProjectType created successfully";

                return RedirectToAction("Index");

            }

            return View(obj);

        }


        //GET
        // controller= ProjectType, action= Edit  
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var ProjectTypeFromDbFirst = _unitOfWork.ProjectType.GetFirstOrDefault(u => u.Id == id);


            // EF 
            if (ProjectTypeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(ProjectTypeFromDbFirst);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectType obj)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.ProjectType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "ProjectType updated successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        ////GET
        //// controller= ProjectType, action= Edit  
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    var ProjectTypeFromDbFirst = _unitOfWork.ProjectType.GetFirstOrDefault(u => u.Id == id);


        //    // EF 
        //    if (ProjectTypeFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ProjectTypeFromDbFirst);
        //}

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, CMSUser")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.ProjectType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.ProjectType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "ProjectType deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
