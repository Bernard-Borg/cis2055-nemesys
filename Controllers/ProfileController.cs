using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Profiledetails()
        {
            return View();
        }

        public IActionResult Signin()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
    }
}
