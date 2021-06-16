using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace Nemesys.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            ViewBag.Message = statusCode + ": " + ReasonPhrases.GetReasonPhrase(statusCode);

            switch(statusCode)
            {
                case 404:
                    return View("Error404");
                default:
                    return View("Error");
            }
        }
    }
}
