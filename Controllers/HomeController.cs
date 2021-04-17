using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStarsRepository starsRepository;

        public HomeController(IStarsRepository starRepository)
        {
            starsRepository = starRepository;
        }

        //Change userId to user's user ID from session
        public IActionResult Index()
        {
            ViewBag.HallOfFameUsers = starsRepository.GetTopUsers(10);
            ViewBag.Reports = starsRepository.GetAllReports()
                .Select(report => new ReportWithUpvote(report, starsRepository.IsStarred(report.ReportId, 1)));
            
            return View();
        }

        /*public IActionResult Index(ReportStatus status)
        {
            ViewBag.HallOfFameUsers = starsRepository.GetTopUsers(10);
            ViewBag.Reports = starsRepository.GetAllReportsWithStatus(status);
            return View();
        }*/

        public IActionResult Hall()
        {
            return View();
        }        
    }
}
