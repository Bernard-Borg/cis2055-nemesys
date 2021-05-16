using Microsoft.AspNetCore.Mvc;
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
        private readonly INemesysRepository _nemesysRepository;

        public InvestigationController(INemesysRepository nemesysRepository)
        {
            _nemesysRepository = nemesysRepository;
        }

        [Route("Investigation/Index/{id}")]
        public IActionResult Index(int id)
        {
            var investigation = new InvestigationViewModel(
                _nemesysRepository.GetInvestigationById(id)
            );

            return View(investigation);
        }
    }
}
