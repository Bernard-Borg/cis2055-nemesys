using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace Nemesys.Controllers
{
    public class InvestigationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly INemesysRepository _nemesysRepository;
        private readonly ILogger<InvestigationController> _logger;

        public InvestigationController(INemesysRepository nemesysRepository, UserManager<User> userManager, ILogger<InvestigationController> logger)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
            _logger = logger;
        }
        
        public IActionResult Index(int id)
        {
            try
            {
                var investigation = _nemesysRepository.GetInvestigationById(id);

                if (investigation != null)
                {
                    return View(new InvestigationViewModel(investigation));
                }
                else
                {
                    return Json("No such investigation");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        [Authorize(Roles = "Investigator,Admin")]
        public IActionResult Create(int id)
        {
            try
            {
                var report = _nemesysRepository.GetReportById(id);

                if (report == null)
                {
                    return NotFound();
                }

                var model = new EditInvestigationViewModel
                {
                    StatusId = report.StatusId
                };

                model.ReportStatuses = _nemesysRepository.GetReportStatuses()
                    .Select(h => new ReportStatusViewModel(h))
                    .ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Investigator,Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromRoute] int id, [Bind("Description", "DateOfAction, StatusId")] EditInvestigationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var investigationReport = _nemesysRepository.GetReportById(id);

                    if (investigationReport == null)
                    {
                        return NotFound();
                    }
                
                    if (investigationReport.InvestigationId != null)
                    {
                        return Json("Report already has an investigation");
                    }

                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

                    var investigation = new Investigation
                    {
                        Description = model.Description,
                        DateOfAction = TimeZoneInfo.ConvertTimeToUtc(model.DateOfAction ?? default, timeZone),
                        UserId = _userManager.GetUserId(User),
                        ReportId = id
                    };

                    var createdInvestigation = _nemesysRepository.CreateInvestigation(investigation);

                    if (createdInvestigation != null)
                    {
                        investigationReport.StatusId = model.StatusId ?? default;
                        investigationReport.InvestigationId = createdInvestigation.InvestigationId;

                        if (_nemesysRepository.UpdateReport(investigationReport))
                        {
                            return RedirectToAction("Index", new { id = createdInvestigation.InvestigationId });
                        }
                        else
                        {
                            return StatusCode(500);
                        }
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
                else
                {
                    model.ReportStatuses = _nemesysRepository.GetReportStatuses()
                    .Select(h => new ReportStatusViewModel(h))
                    .ToList();

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        [Authorize(Roles = "Investigator,Admin")]
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var existingInvestigation = _nemesysRepository.GetInvestigationById(id);

                if (existingInvestigation != null)
                {
                    var investigationReport = _nemesysRepository.GetReportById(existingInvestigation.InvestigationId);

                    if (existingInvestigation.UserId == _userManager.GetUserId(User))
                    {
                        EditInvestigationViewModel model = new EditInvestigationViewModel
                        {
                            InvestigationId = existingInvestigation.InvestigationId,
                            Description = existingInvestigation.Description,
                            DateOfAction = existingInvestigation.DateOfAction,
                            StatusId = investigationReport.StatusId
                        };

                        //Allow all report statuses except Open (since investigations cannot be deleted)
                        model.ReportStatuses = _nemesysRepository.GetReportStatuses()
                            .Where(s => s.Id == 2 || s.Id == 3 || s.Id == 4)
                            .Select(s => new ReportStatusViewModel(s))
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

        [Authorize(Roles = "Investigator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, [Bind("Description, DateOfAction, StatusId")] EditInvestigationViewModel updatedInvestigation)
        {
            try
            {
                Investigation existingInvestigation = _nemesysRepository.GetInvestigationById(id);

                //Check if the report being edited exists
                if (existingInvestigation == null)
                {
                    return NotFound();
                }

                Report investigationReport = _nemesysRepository.GetReportById(existingInvestigation.ReportId);

                if (investigationReport == null)
                {
                    return StatusCode(500);
                }

                //Checks whether the user is the owner of the investigation
                if (existingInvestigation.UserId == _userManager.GetUserId(User))
                {
                    if (ModelState.IsValid)
                    {
                        existingInvestigation.DateOfAction = TimeZoneInfo.ConvertTimeToUtc(updatedInvestigation.DateOfAction ?? default, timeZone);
                        existingInvestigation.Description = updatedInvestigation.Description;
                        investigationReport.StatusId = updatedInvestigation.StatusId ?? default;

                        if (_nemesysRepository.UpdateInvestigation(existingInvestigation))
                        {
                            if (_nemesysRepository.UpdateReport(investigationReport))
                            {
                                return RedirectToAction("Index", new { id });
                            }
                        }

                        return StatusCode(500);
                    }
                    else
                    {
                        updatedInvestigation.ReportStatuses = _nemesysRepository.GetReportStatuses()
                            .Select(h => new ReportStatusViewModel(h))
                            .ToList();

                        return View(updatedInvestigation);
                    }
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
