using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using Microsoft.Extensions.Logging;
using System;

namespace Nemesys.Controllers
{
    public class ProfileController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(INemesysRepository nemesysRepository, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<ProfileController> logger)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index(string id)
        {
            try
            {
                User user = _nemesysRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                UserViewModel model = new UserViewModel(
                    user,
                    _userManager.GetUserAsync(User).Result,
                    _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result
                );
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            } 
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Promote(string id)
        {
            try
            {
                var user = _nemesysRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result).Wait();
                _userManager.AddToRoleAsync(user, "Investigator").Wait();
                return RedirectToAction("Index", "Profile", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Demote(string id)
        {
            try
            {
                var user = _nemesysRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result).Wait();
                _userManager.AddToRoleAsync(user, "Reporter").Wait();
                return RedirectToAction("Index", "Profile", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        public IActionResult SignOut(string returnUrl = null)
        {
            try
            {
                _signInManager.SignOutAsync().Wait();
                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }
                return Redirect("/Home/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }
    }
}
