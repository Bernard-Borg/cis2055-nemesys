using Microsoft.AspNetCore.Authorization;
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
        private readonly INemesysRepository mainRepository;

        public HomeController(INemesysRepository starRepository)
        {
            mainRepository = starRepository;
        }

        //Change userId to user's user ID from session
        [ResponseCache(Duration = 2)]
        public IActionResult Index()
        {
            HallOfFameViewModel hofViewModel = new HallOfFameViewModel(mainRepository.GetTopUsers(10));
            ReportListViewModel reportsViewModel = new ReportListViewModel(mainRepository.GetAllReports());

            var model = new HomePageViewModel(hofViewModel, reportsViewModel);
            return View(model);
        }

        [ResponseCache(Duration = 2)]
        [Route("Home/Index/{status}")]
        public IActionResult Index(ReportStatus status)
        {
            HallOfFameViewModel hofViewModel = new HallOfFameViewModel(mainRepository.GetTopUsers(10));
            ReportListViewModel reportsViewModel = new ReportListViewModel(mainRepository.GetAllReportsWithStatus(status));

            var model = new HomePageViewModel(hofViewModel, reportsViewModel);
            return View(model);
        }

        public IActionResult Hall()
        {
            var model = new HallOfFameViewModel(mainRepository.GetTopUsers(10));
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
