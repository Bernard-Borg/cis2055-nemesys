using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public IActionResult Index()
        {
            return NotFound();
        }

        [Authorize(Roles = "Investigator,Admin")]
        [Route("/Investigation/Index/{id}")]
        public IActionResult Index(int id)
        {
            var investigation = _nemesysRepository.GetInvestigationById(id);

            if (investigation != null)
            {
                return View(new InvestigationViewModel(investigation));
            } else
            {
                return Json("No such investigation");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Investigator,Admin")]
        public IActionResult Create(int id)
        {
            var model = new EditInvestigationViewModel
            {
                ReportId = id
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Investigator,Admin")]
        public IActionResult Create([FromRoute] int id, EditInvestigationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var investigation = new Investigation
                {
                    Description = model.Description,
                    DateOfAction = model.DateOfAction ?? default(DateTime),
                    UserId = _userManager.GetUserId(User),
                    ReportId = id
                };

                var createdInvestigation = _nemesysRepository.CreateInvestigation(investigation);

                if (createdInvestigation != null)
                {
                    return RedirectToAction("Index", new { id = createdInvestigation.InvestigationId });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
