using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var model = starsRepository.GetTopUsers(10);
            return View(model);
        }
    }
}
