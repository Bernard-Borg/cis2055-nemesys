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
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.Controllers
{
    public class ReportController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ReportController> _logger;

        public ReportController(INemesysRepository nemesysRepository, UserManager<User> userManager, IWebHostEnvironment environment, ILogger<ReportController> logger)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
            _environment = environment;
            _logger = logger;
        }

        //Returns report details page
        public IActionResult Index(int id)
        {
            try
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
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Returns page containing report create form
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Handles report create form submissions
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DateTimeOfHazard", "Latitude", "Longitude", "HazardTypeId", "Description", "Photo")] EditReportViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

                    var report = new Report
                    {
                        DateOfReport = DateTime.UtcNow,
                        DateTimeOfHazard = TimeZoneInfo.ConvertTimeToUtc(model.DateTimeOfHazard ?? default, timeZone),
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
                            if (model.Photo.Length > 0)
                            {
                                using (FileStream stream = new FileStream(_environment.WebRootPath + report.Photo, FileMode.Create))
                                {
                                    model.Photo.CopyTo(stream);
                                    stream.Flush();
                                }
                            }
                            else
                            {
                                _logger.LogDebug("User tried to upload file of length 0");
                            }
                        }

                        return RedirectToAction("Index", new { id = createdReport.Id });
                    }
                    else
                    {
                        _logger.LogError("Report creation failed");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Returns page containing report edit form
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Handles report edit form submissions
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, [Bind("DateTimeOfHazard", "Latitude", "Longitude", "HazardTypeId", "Description", "Photo")] EditReportViewModel updatedReport)
        {
            try
            {
                Report existingReport = _nemesysRepository.GetReportById(id);

                //Check if the report being edited exists
                if (existingReport == null)
                {
                    return NotFound();
                }

                //Checks whether the user is the owner of the report
                if (existingReport.UserId == _userManager.GetUserId(User))
                {
                    if (ModelState.IsValid)
                    {
                        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

                        existingReport.DateTimeOfHazard = TimeZoneInfo.ConvertTimeToUtc(updatedReport.DateTimeOfHazard ?? default, timeZone);
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
                                if (updatedReport.Photo.Length > 0)
                                {
                                    using (FileStream stream = new FileStream(_environment.WebRootPath + existingReport.Photo, FileMode.Create))
                                    {
                                        updatedReport.Photo.CopyTo(stream);
                                        stream.Flush();
                                    }
                                }
                                else
                                {
                                    _logger.LogWarning("User tried to upload file of length 0");
                                }   
                            }

                            return RedirectToAction("Index", new { id });
                        }
                        else
                        {
                            _logger.LogError("Updating report failed");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Handles request for report deletion (returns ConfirmDelete view as part of "read-first" approach)
        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var report = _nemesysRepository.GetReportById(id);

                if (report == null)
                {
                    return NotFound();
                }

                if (report.UserId == _userManager.GetUserId(User))
                {
                    return View("ConfirmDelete", new ReportViewModel(report,
                        _nemesysRepository.GetUserById(_userManager.GetUserId(User)))
                    );
                }
                else
                {
                    return Forbid();
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult ConfirmDelete([Required] int reportid)
        {
            try
            {
                var reportToDelete = _nemesysRepository.GetReportById(reportid);

                if (reportToDelete == null)
                {
                    return NotFound();
                }

                //Only reporter his own report
                if (reportToDelete.UserId == _userManager.GetUserId(User))
                {
                    if (_nemesysRepository.DeleteReport(reportid))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    _logger.LogError("Report deletion failed");
                    return StatusCode(500);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }
    }
}
