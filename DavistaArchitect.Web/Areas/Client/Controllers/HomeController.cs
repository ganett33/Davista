using DavistaArchitect.DataAccess.Data;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.Models;
using DavistaArchitect.Models.ViewModels;
using DavistaArchitect.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Common;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using System.Collections.Generic;
using System.Diagnostics;

namespace DavistaArchitect.Web.Areas.Client.Controllers
{

    [Area("Client")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [Breadcrumb("Index")]
        public IActionResult Index()
        {
            //IEnumerable<House> objHouseList = _unitOfWork.House.GetAll();
            //return View(objHouseList);
            ViewBag.Title = "Residential & Commerical Architecural Design service";
            IEnumerable<House> objHouseList = _unitOfWork.House.GetAll(includeProperties: "ProjectType");

            return View(objHouseList);
        }


        [Breadcrumb("Projects", FromAction = "Index")]
        public IActionResult Projects()
        {
            ViewBag.Breadcrumbs = new[]
            {
                new { DisplayName = "Home".ToUpper(), Url = Url.Action("Index", "Home") },
                new { DisplayName = "Projects".ToUpper(), Url = Url.Action("Projects", "Home") }
            };


            var projectTypes = _unitOfWork.ProjectType.GetAll();

            var projectHouse = projectTypes.Select(p => new ProjectHouseVM
            {
                ProjectTypeId = p.Id,
                ProjectTypeName = p.ProjectTypeName,
                ProjectTypeUrl = p.ProjectTypeUrl,
                ProjectTypeImage = p.ProjectTypeImage,
                Houses = _unitOfWork.House.GetAll().Where(h => h.ProjectTypeId == p.Id).ToList()
            }).ToList();

            return View(projectHouse);
        }


        // Home/ProjectHouse/{projectTypeUrl}/{id}
        [Breadcrumb("Project", FromAction = "Projects")]
        public IActionResult Project(string projectTypeUrl)
        {
            var projectTypeObj = _unitOfWork.ProjectType.GetFirstOrDefault(p => p.ProjectTypeUrl == projectTypeUrl);

            if (projectTypeObj == null)
            {
                return NotFound();
            }

            var viewModelList = _unitOfWork.ProjectType.GetAll()
                .Where(p => p.Id == projectTypeObj.Id)
                .Select(p => new ProjectHouseVM
                {
                    ProjectTypeId = p.Id,
                    ProjectTypeName = p.ProjectTypeName,
                    ProjectTypeUrl = p.ProjectTypeUrl,
                    Houses = _unitOfWork.House.GetAll().Where(h => h.ProjectTypeId == p.Id).ToList()
                })
                .ToList();


            ViewBag.Breadcrumbs = new[]
            {
                new { DisplayName = "Home".ToUpper(), Url = Url.Action("Index", "Home") },
                new { DisplayName = "Projects".ToUpper(), Url = Url.Action("Projects", "Home") },
                new { DisplayName = $"{projectTypeObj.ProjectTypeName.ToUpper()}", Url = Url.Action("Project", "Home", new { projectTypeUrl }) }
            };

            return View(viewModelList);
        }

        [Breadcrumb("HouseDetail", FromAction = "Project")]
        public IActionResult HouseDetail(string projectTypeUrl, int id)
        {
            var house = _unitOfWork.House.GetFirstOrDefault(h => h.Id == id && h.ProjectType.ProjectTypeUrl == projectTypeUrl);

            if (house == null)
            {
                return NotFound();
            }

            ViewBag.Breadcrumbs = new[]
            {
                new { DisplayName = "Home".ToUpper(), Url = Url.Action("Index", "Home") },
                new { DisplayName = "Projects".ToUpper(), Url = Url.Action("Projects", "Home") },
                new { DisplayName = $"{projectTypeUrl.ToUpper()}", Url = Url.Action("Project", "Home", new { projectTypeUrl }) },
                new { DisplayName = $"{house.HouseName.ToUpper()}", Url = Url.Action("HouseDetail", "Home", new { projectTypeUrl, id }) }
            };


            return View(house);
        }


        public IActionResult OurTeam()
        {
            var objStaffList = _unitOfWork.StaffProfile.GetAll().OrderBy(s => s.StartedDate);
            return View(objStaffList);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}