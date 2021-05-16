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
    public class ReportController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;

        public ReportController(INemesysRepository nemesysRepository, UserManager<User> userManager)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var report = new ReportViewModel(
                _nemesysRepository.GetReportById(id), 
                _nemesysRepository.GetUserById(_userManager.GetUserId(this.User))
            );

            return View(report);
        }

        public IActionResult Submit()
        {
            return View();
        }
    }
}
