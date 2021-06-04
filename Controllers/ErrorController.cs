using Microsoft.AspNetCore.Mvc;

namespace Nemesys.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
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
