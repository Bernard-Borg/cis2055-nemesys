using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.Controllers
{
    public class HomeController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;

        public HomeController(INemesysRepository nemesysRepository, UserManager<User> userManager)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
        }

        [ResponseCache(Duration = 2)]
        public IActionResult Index(HomeSortQueryParameter sort)
        {
            IEnumerable<Report> reports;
            if (sort.StatusId != null)
            {
                reports = _nemesysRepository.GetAllReportsWithStatus(sort.StatusId) ?? _nemesysRepository.GetAllReports();
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
                    reports = reports.OrderBy(r => r.DateOfUpdate);
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

            HallOfFameViewModel hofViewModel = new HallOfFameViewModel(_nemesysRepository.GetTopUsers(5));
            ReportListViewModel reportsViewModel = new ReportListViewModel(
                reports.ToList(),
                _nemesysRepository.GetUserById(_userManager.GetUserId(User))
            );

            var model = new HomePageViewModel(hofViewModel, reportsViewModel, _nemesysRepository.GetReportStatuses());
            return View(model);
        }

        public IActionResult Hall(string sort)
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

            var model = new HallOfFameViewModel(users);
            return View(model);
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            if (search == null)
            {
                return RedirectToAction("Index");
            }

            var reportListViewModel = new ReportListViewModel(
                _nemesysRepository.GetAllReports()
                    .Where(report => report.Description.Contains(search, System.StringComparison.CurrentCultureIgnoreCase))
                    .ToList(),
                _userManager.GetUserAsync(User).Result
            );

            var model = new SearchResultViewModel(reportListViewModel, search);

            return View("SearchResult", model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Star(int reportId)
        {
            return Json(_nemesysRepository.StarReport(_userManager.GetUserId(User), reportId));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
