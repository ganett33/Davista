using Microsoft.AspNetCore.Mvc;
using DavistaArchitect.Models;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DavistaArchitect.Web.Areas.Webmanager.Controllers
{
    [Area("Webmanager")]
    public class UserLevelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserLevelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
 
        }

        public IActionResult Index()
        {
            return View();
        }


        //GET 
        public IActionResult Upsert(int? id)
        {
            UserLevel userLevel = new();
            
            if (id == null || id == 0)
            {
                //create
                return View(userLevel);
            }
            else
            {
                //update
                userLevel = _unitOfWork.UserLevel.GetFirstOrDefault(u => u.Id == id);
                return View(userLevel);
            }


        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(UserLevel obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.UserLevel.Add(obj);
                    TempData["success"] = "UserLevel added successfully";
                }
                else
                {
                    _unitOfWork.UserLevel.Update(obj);
                    TempData["success"] = "UserLevel updated successfully";

                }


                _unitOfWork.Save();


                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var userLevelList = _unitOfWork.UserLevel.GetAll();
            return Json(new { data = userLevelList });
        }


        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.UserLevel.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.UserLevel.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });


            #endregion

        }
    }

}
