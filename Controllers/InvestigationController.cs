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
    public class InvestigationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly INemesysRepository _nemesysRepository;

        public InvestigationController(INemesysRepository nemesysRepository, UserManager<User> userManager)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
        }

        [Authorize(Roles = "Investigator,Admin")]
        [Route("Investigation/Index/{id}")]
        public IActionResult Index(int id)
        {
            var investigation = new InvestigationViewModel(
                _nemesysRepository.GetInvestigationById(id)
            );

            return View(investigation);
        }

        [Authorize(Roles = "Investigator,Admin")]
        [Route("Investigation/Create/{id}")]
        public IActionResult Create(int id)
        {
            var investigation = new EditInvestigationViewModel
            {
                UserId = _userManager.GetUserId(this.User),
                ReportId = id
            };

            return View(investigation);
        }
    }
}
