using Microsoft.AspNetCore.Mvc;
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
        private readonly INemesysRepository starsRepository;

        public ReportController(INemesysRepository starRepository)
        {
            starsRepository = starRepository;
        }

        public IActionResult Index(int id)
        {
            var report = new ReportViewModel(starsRepository.GetReportById(id));
            return View(report);
        }

        public IActionResult Submit()
        {
            return View();
        }
    }
}
