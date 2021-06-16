using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Nemesys.Controllers
{
    public class ErrorController : Controller
    {
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
