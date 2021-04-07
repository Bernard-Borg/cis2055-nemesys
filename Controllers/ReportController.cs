using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cis2205_nemesys.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Report()
        {
            return View();
        }
        public IActionResult Submit()
        {
            return View();
        }
    }
}
