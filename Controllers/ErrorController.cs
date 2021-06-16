using Microsoft.AspNetCore.Mvc;
using System;

namespace Nemesys.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            Console.WriteLine("Hello: " + statusCode);

            switch(statusCode)
            {
                case 404:
                    //ViewBag.ErrorMessage("Resource could not be found.");
                    break;
            }
            return View("Error");
        }
    }
}
