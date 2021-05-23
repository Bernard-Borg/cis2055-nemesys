using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        //Change userId to user's user ID from session
        [ResponseCache(Duration = 2)]
        public IActionResult Index()
        {
            HallOfFameViewModel hofViewModel = new HallOfFameViewModel(_nemesysRepository.GetTopUsers(10));
            ReportListViewModel reportsViewModel = new ReportListViewModel(
                _nemesysRepository.GetAllReports().ToList(),
                _nemesysRepository.GetUserById(_userManager.GetUserId(this.User))
            );

            var model = new HomePageViewModel(hofViewModel, reportsViewModel, _nemesysRepository.GetReportStatuses());
            return View(model);
        }

        [ResponseCache(Duration = 2)]
        [Route("Home/Index/{id}")]
        public IActionResult Index(int id)
        { 
            HallOfFameViewModel hofViewModel = new HallOfFameViewModel(_nemesysRepository.GetTopUsers(10));
            ReportListViewModel reportsViewModel = new ReportListViewModel(
                _nemesysRepository.GetAllReportsWithStatus(id).ToList(),
                _nemesysRepository.GetUserById(_userManager.GetUserId(this.User))
            );

            var model = new HomePageViewModel(hofViewModel, reportsViewModel, _nemesysRepository.GetReportStatuses());
            return View(model);
        }

        public IActionResult Hall()
        {
            var model = new HallOfFameViewModel(_nemesysRepository.GetTopUsers(10));
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Search(string search)
        {
            var reportListViewModel = new ReportListViewModel(
                _nemesysRepository.GetAllReports()
                    .Where(report => report.Description.Contains(search))
                    .ToList(),
                _userManager.GetUserAsync(User).Result
            );

            var model = new SearchResultViewModel(reportListViewModel, search);

            return View("SearchResult", model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
