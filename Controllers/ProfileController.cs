using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System.Linq;

namespace Nemesys.Controllers
{
    public class ProfileController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;

        public ProfileController(INemesysRepository nemesysRepository, UserManager<User> userManager)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
        }

        public IActionResult Index(string id)
        {
            UserViewModel model = new UserViewModel(
                _nemesysRepository.GetUserById(id),
                _userManager.GetUserAsync(User).Result,
                _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result
            );

            return View(model);
        }
    }
}
