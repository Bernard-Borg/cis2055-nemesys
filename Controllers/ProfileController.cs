using Microsoft.AspNetCore.Mvc;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System.Linq;

namespace Nemesys.Controllers
{
    public class ProfileController : Controller
    {
        private readonly INemesysRepository starsRepository;

        public ProfileController(INemesysRepository starRepository)
        {
            starsRepository = starRepository;
        }

        public IActionResult Index(string id)
        {
            UserViewModel model = new UserViewModel(starsRepository.GetUserById(id));
            return View(model);
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
