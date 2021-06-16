using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Nemesys.Controllers
{
    public class HomeController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(INemesysRepository nemesysRepository, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [ResponseCache(Duration = 3)]
        public IActionResult Index(HomeSortQueryParameter sort)
        {
            try
            {
                IEnumerable<Report> reports;

                if (sort.StatusId != null)
                {
                    reports = _nemesysRepository.GetAllReportsWithStatus(sort.StatusId ?? default) ?? _nemesysRepository.GetAllReports();
                }
                else
                {
                    reports = _nemesysRepository.GetAllReports();
                }

                switch (sort.SortString)
                {
                    case "Award":
                        reports = reports.OrderByDescending(r => r.NumberOfStars).ThenBy(r => r.Description);
                        break;
                    case "Update":
                        reports = reports.OrderByDescending(r => r.DateOfUpdate);
                        break;
                    case "Newest":
                        reports = reports.OrderByDescending(r => r.DateOfReport);
                        break;
                    case "Oldest":
                        reports = reports.OrderBy(r => r.DateOfReport);
                        break;
                    default:
                        reports = reports.OrderBy(r => r.DateOfUpdate);
                        break;
                }

                HallOfFameViewModel hofViewModel = new HallOfFameViewModel(
                    _nemesysRepository.GetTopUsers(5).Select(u => new ProfileCardViewModel(u))
                );

                ReportListViewModel reportsViewModel = new ReportListViewModel(
                    reports.ToList(),
                    _nemesysRepository.GetUserById(_userManager.GetUserId(User))
                );
                
                var model = new HomePageViewModel(hofViewModel, reportsViewModel, 
                    _nemesysRepository.GetReportStatuses()
                        .Select(s => new ReportStatusViewModel(s))
                );

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        [ResponseCache(Duration = 3)]
        public IActionResult Hall(string sort)
        {
            try
            {
                var users = _nemesysRepository.GetUsers();

                switch (sort)
                {
                    case "Award":
                        users = users.OrderByDescending(u => u.NumberOfStars).ThenBy(u => u.Alias);
                        break;
                    case "Report":
                        users = users.OrderByDescending(u => u.NumberOfReports).ThenBy(u => u.Alias);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.NumberOfStars).ThenBy(u => u.Alias);
                        break;
                }

                var model = new HallOfFameViewModel(users.Select(u => new ProfileCardViewModel(u)));
                return View(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            try
            {
                if (search == null)
                {
                    return RedirectToAction("Index");
                }

                var reportListViewModel = new ReportListViewModel(
                    _nemesysRepository.GetAllReports()
                        .Where(report => report.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase))
                        .ToList(),
                    _userManager.GetUserAsync(User).Result
                );

                var model = new SearchResultViewModel(reportListViewModel, search);

                return View("SearchResult", model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }  
        }

        [HttpPost]
        [Authorize]
        public IActionResult Star(int reportId, string returnUrl = null)
        {
            try
            {
                var report = _nemesysRepository.GetReportById(reportId);

                if (report == null)
                {
                    return NotFound();
                }
                
                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(_nemesysRepository.StarReport(_userManager.GetUserId(User), reportId));
                }
                else
                {
                    if (_nemesysRepository.StarReport(_userManager.GetUserId(User), reportId))
                    {
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
