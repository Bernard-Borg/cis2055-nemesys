using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System;
using System.IO;
using System.Linq;

namespace Nemesys.Controllers
{
    public class ReportController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ReportController(INemesysRepository nemesysRepository, UserManager<User> userManager, IWebHostEnvironment environment)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
            _environment = environment;
        }

        public IActionResult Index(int id)
        {
            var report = _nemesysRepository.GetReportById(id);

            if (report != null)
            {
                var model = new ReportViewModel(
                   report,
                   _nemesysRepository.GetUserById(_userManager.GetUserId(User))
                );

                return View(model);
            }
            else
            {
                return Json("No such report");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var hazardTypes = _nemesysRepository.GetHazardTypes()
                .Select(h => new HazardTypeViewModel(h))
                .ToList();

            var model = new EditReportViewModel()
            {
                HazardTypes = hazardTypes
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(EditReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var report = new Report
                {
                    DateOfReport = DateTime.UtcNow,
                    DateTimeOfHazard = model.DateTimeOfHazard ?? default(DateTime),
                    DateOfUpdate = DateTime.UtcNow,
                    Latitude = model.Latitude ?? default(double),
                    Longitude = model.Longitude ?? default(double),
                    HazardTypeId = model.HazardTypeId,
                    StatusId = 1,
                    InvestigationId = null,
                    Description = model.Description,
                    NumberOfStars = 0,
                    UserId = _userManager.GetUserId(User)
                };

                if (model.Photo != null)
                {
                    report.Photo = "/images/reportimages/" + Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                }

                var createdReport = _nemesysRepository.CreateReport(report);

                if (createdReport != null)
                {
                    if (model.Photo != null)
                    {
                        model.Photo.CopyTo(new FileStream(_environment.WebRootPath + report.Photo, FileMode.Create));
                    }

                    return RedirectToAction("Index", new { id = createdReport.Id });
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                var hazardTypes = _nemesysRepository.GetHazardTypes()
                    .Select(h => new HazardTypeViewModel(h))
                    .ToList();

                model.HazardTypes = hazardTypes;
                return View(model);
            }
        }
    }
}
