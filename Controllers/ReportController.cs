using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DateTimeOfHazard", "Latitude", "Longitude", "HazardTypeId", "Description", "Photo")] EditReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var report = new Report
                {
                    DateOfReport = DateTime.UtcNow,
                    DateTimeOfHazard = model.DateTimeOfHazard ?? default,
                    DateOfUpdate = DateTime.UtcNow,
                    Latitude = model.Latitude ?? default,
                    Longitude = model.Longitude ?? default,
                    HazardTypeId = model.HazardTypeId ?? default,
                    StatusId = 1,
                    InvestigationId = null,
                    Description = model.Description,
                    NumberOfStars = 0,
                    UserId = _userManager.GetUserId(User)
                };

                if (model.Photo != null)
                    report.Photo = "/images/reportimages/" + Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

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

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var existingReport = _nemesysRepository.GetReportById(id);

            if (existingReport != null)
            {
                if (existingReport.UserId == _userManager.GetUserId(User))
                {
                    EditReportViewModel model = new EditReportViewModel
                    {
                        Id = existingReport.Id,
                        DateTimeOfHazard = existingReport.DateTimeOfHazard,
                        Latitude = existingReport.Latitude,
                        Longitude = existingReport.Longitude,
                        HazardTypeId = existingReport.HazardTypeId,
                        Description = existingReport.Description,
                        ImageUrl = existingReport.Photo,
                    };

                    model.HazardTypes = _nemesysRepository.GetHazardTypes()
                        .Select(h => new HazardTypeViewModel(h))
                        .ToList();

                    return View(model);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, [Bind("DateTimeOfHazard", "Latitude", "Longitude", "HazardTypeId", "Description", "Photo")] EditReportViewModel updatedReport)
        {
            Report existingReport = _nemesysRepository.GetReportById(id);

            //Check if the report being edited exists
            if (existingReport == null) {
                return NotFound();
            }

            //Checks whether the user is the owner of the report
            if (existingReport.UserId == _userManager.GetUserId(User))
            {
                if (ModelState.IsValid)
                {
                    existingReport.DateTimeOfHazard = updatedReport.DateTimeOfHazard ?? default;
                    existingReport.Latitude = updatedReport.Latitude ?? default;
                    existingReport.Longitude = updatedReport.Longitude ?? default;
                    existingReport.HazardTypeId = updatedReport.HazardTypeId ?? default;
                    existingReport.Description = updatedReport.Description;

                    if (updatedReport.Photo != null)
                    {
                        existingReport.Photo = "/images/reportimages/" + Guid.NewGuid().ToString() + "_" + updatedReport.Photo.FileName;
                    }

                    if (_nemesysRepository.UpdateReport(existingReport))
                    {
                        //After the report has successfully been updated, the image file is created
                        if (updatedReport.Photo != null)
                        {
                            updatedReport.Photo.CopyTo(new FileStream(_environment.WebRootPath + existingReport.Photo, FileMode.Create));
                        }

                        return RedirectToAction("Index", new { id });
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
                else
                {
                    updatedReport.HazardTypes = _nemesysRepository.GetHazardTypes()
                        .Select(h => new HazardTypeViewModel(h))
                        .ToList();

                    return View(updatedReport);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var reportToDelete = _nemesysRepository.GetReportById(id);

            if (reportToDelete == null)
            {
                return NotFound();
            }

            if (reportToDelete.UserId == _userManager.GetUserId(User))
            {
                if (_nemesysRepository.DeleteReport(id))
                {
                    return RedirectToAction("Index", "Home");
                }

                return StatusCode(500);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
