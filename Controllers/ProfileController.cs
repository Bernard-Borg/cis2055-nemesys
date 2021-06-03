using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nemesys.Areas.Identity.Pages.Account;

namespace Nemesys.Controllers
{
    public class ProfileController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ProfileController(INemesysRepository nemesysRepository, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
            _signInManager = signInManager;
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

        [HttpPost]
        public IActionResult Promote(string id)
        {
            var user = _nemesysRepository.GetUserById(id);
            _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result).Wait();
            _userManager.AddToRoleAsync(user, "Investigator").Wait();
            return RedirectToAction("Index", "Profile", new { id });
        }

        [HttpPost]
        public IActionResult Demote(string id)
        {
            var user = _nemesysRepository.GetUserById(id);
            _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result).Wait();
            _userManager.AddToRoleAsync(user, "Reporter").Wait();
            return RedirectToAction("Index", "Profile", new { id });
        }

        public IActionResult SignOut(string returnUrl = null)
        {
            _signInManager.SignOutAsync().Wait();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return Redirect("/Home/Index");
        }
    }
}
