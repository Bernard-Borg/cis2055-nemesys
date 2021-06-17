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
using X.PagedList;

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

        //Home page action
        public IActionResult Index(HomeSortQueryParameter sort, int? page)
        {
            try
            {
                IEnumerable<Report> reports;
                
                if (sort.StatusId != null)
                {
                    //Used when user presses on "Open/Under Investigation/No Action Required/Closed"
                    reports = _nemesysRepository.GetAllReportsWithStatus(sort.StatusId ?? default) ?? _nemesysRepository.GetAllReports();
                }
                else
                {
                    //Returns all reports
                    reports = _nemesysRepository.GetAllReports();
                }

                //Used when user sorts the reports
                switch (sort.SortString)
                {
                    case "Award":
                        reports = reports.OrderByDescending(r => r.NumberOfStars)
                            .ThenBy(r => r.Description);
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

                //Fills hall of fame "summary" with the top 5 users
                HallOfFameViewModel hofViewModel = new HallOfFameViewModel(
                    _nemesysRepository.GetTopUsers(5).Select(u => new ProfileCardViewModel(u))
                );

                int pageNumber = page ?? 1;

                //Paging is used to limit the number of reports displayed to make the site more usable
                PagedReportListViewModel pagedReportListViewModel = new PagedReportListViewModel(
                    reports.Select(r => new ReportViewModel(r, _nemesysRepository.GetUserById(_userManager.GetUserId(User))))
                        .ToPagedList(pageNumber, 7)
                );
                
                //ViewModel is constructed
                var model = new HomePageViewModel(hofViewModel, pagedReportListViewModel, 
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

        //Hall of fame action
        [ResponseCache(Duration = 3)]
        public IActionResult Hall(string sort)
        {
            try
            {
                var users = _nemesysRepository.GetUsers();

                //Used when user sorts the hall of fame users
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

        //Search action
        [HttpGet]
        public IActionResult Search(string search)
        {
            try
            {
                if (search == null)
                {
                    return RedirectToAction("Index");
                }

                //Gets reports who's description contains the search term and constructs view model for list of reports
                var reportListViewModel = new ReportListViewModel(
                    _nemesysRepository.GetAllReports()
                        .Where(report => report.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase))
                        .ToList(),
                    _nemesysRepository.GetUserById(_userManager.GetUserId(User))
                );

                //Gets users who's username contains the search term and creates list of view models
                var listOfUsers = _nemesysRepository.GetUsers()
                    .Where(user => user.Alias.Contains(search, StringComparison.CurrentCultureIgnoreCase))
                    .Select(u => new ProfileCardViewModel(u));

                //Constructs the view model for the search result
                var model = new SearchResultViewModel(reportListViewModel, listOfUsers, search);

                return View("SearchResult", model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }  
        }

        //Starring action (upvotes)
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
                    //If the request was done through AJAX, then return "true/false" to signal whether or not operation succeeded
                    return Json(_nemesysRepository.StarReport(_userManager.GetUserId(User), reportId));
                }
                else
                {
                    /*
                     * If the user has Javascript disabled, the report is starred and the user is returned to the page 
                     * where the star was located (which can either be in the home page, report index, or profile index)
                     */

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
    }
}
