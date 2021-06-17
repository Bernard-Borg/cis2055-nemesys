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
        
        //Returns investigation details page
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
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Returns the page containing the investigation create form
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
                    //Sets status id of the report (it can only be Open)
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

        //Handles the investigation create form submissions
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
                    
                    //If the report has already been investigated, then it has an InvestigationId assigned
                    if (investigationReport.InvestigationId != null)
                    {
                        //An investigation cannot be created on a report which already has an investigation
                        return Json("Report already has an investigation");
                    }
                    
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

                    var investigation = new Investigation
                    {
                        Description = model.Description,
                        //Converts from Malta time to UTC (assuming that only people from UOM would use the website)
                        DateOfAction = TimeZoneInfo.ConvertTimeToUtc(model.DateOfAction ?? default, timeZone),
                        UserId = _userManager.GetUserId(User),
                        ReportId = id
                    };

                    var createdInvestigation = _nemesysRepository.CreateInvestigation(investigation);

                    if (createdInvestigation != null)
                    {
                        //If investigation creation was successful, update report with new status and invesigation id
                        investigationReport.StatusId = model.StatusId ?? default;
                        investigationReport.InvestigationId = createdInvestigation.InvestigationId;

                        if (_nemesysRepository.UpdateReport(investigationReport))
                        {
                            _logger.LogError("Updating report of investigation failed");
                            return RedirectToAction("Index", new { id = createdInvestigation.InvestigationId });
                        }
                        else
                        {
                            return StatusCode(500);
                        }
                    }
                    else
                    {
                        _logger.LogError("Investigation creation failed");
                        return StatusCode(500);
                    }
                }
                else
                {
                    /*
                     * If model was invalid, re-attach the report statuses (to create the selectlist) 
                     * and re-display create form with appropriate validation error messages
                     */

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

        //Returns the page containing the investigation edit form
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

                    //Only allow the investigator to edit the investigation
                    if (existingInvestigation.UserId == _userManager.GetUserId(User))
                    {
                        //Pre-fill form fields with invesigation properties
                        EditInvestigationViewModel model = new EditInvestigationViewModel
                        {
                            InvestigationId = existingInvestigation.InvestigationId,
                            Description = existingInvestigation.Description,
                            DateOfAction = existingInvestigation.DateOfAction,
                            StatusId = investigationReport.StatusId
                        };

                        //Allow all report statuses except Open (since investigations cannot be deleted)
                        model.ReportStatuses = _nemesysRepository.GetReportStatuses()
                            .Where(s => s.Id != 1)
                            .Select(s => new ReportStatusViewModel(s))
                            .ToList();

                        return View(model);
                    }
                    else
                    {
                        return Forbid();
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

        //Handles the investigation edit form submissions
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
                        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

                        //Converts time from Malta time to UTC
                        existingInvestigation.DateOfAction = TimeZoneInfo.ConvertTimeToUtc(updatedInvestigation.DateOfAction ?? default, timeZone);
                        existingInvestigation.Description = updatedInvestigation.Description;
                        investigationReport.StatusId = updatedInvestigation.StatusId ?? default;

                        if (_nemesysRepository.UpdateInvestigation(existingInvestigation))
                        {
                            //If updating the investigation is successful, then the report is updated as well
                            if (_nemesysRepository.UpdateReport(investigationReport))
                            {
                                return RedirectToAction("Index", new { id });
                            }
                        }

                        _logger.LogError("Updating investigation failed");
                        return StatusCode(500);
                    }
                    else
                    {
                        /*
                         * If model was invalid, re-attach the report statuses (to create the selectlist) 
                         * and re-display edit form with appropriate validation error messages
                         */

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
