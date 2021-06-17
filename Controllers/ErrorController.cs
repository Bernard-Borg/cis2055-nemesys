using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Nemesys.Controllers
{
    public class ErrorController : Controller
    {
        //This action handles status codes (401, 403, 404, 405, 500, etc)
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            //Displays the status code and status code message
            ViewBag.Message = statusCode + ": " + ReasonPhrases.GetReasonPhrase(statusCode);

            switch(statusCode)
            {
                case 404:
                    //Returns custom 404 page
                    return View("Error404");
                default:
                    //Returns generic status code page
                    return View("Error");
            }
        }
    }
}
