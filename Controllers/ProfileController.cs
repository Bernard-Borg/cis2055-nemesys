using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

        //Returns the profile details view
        public IActionResult Index(string id)
        {
            try
            {
                User user = _nemesysRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                //Construction of user view model
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

        //Handles promotion of Reporters to Investigators
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Promote(string id)
        {
            try
            {
                var user = _nemesysRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                //Removes current roles and adds investigator
                await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result);
                await _userManager.AddToRoleAsync(user, "Investigator");

                _logger.LogInformation("User with id " + id + " has been demoted to Investigator");

                return RedirectToAction("Index", "Profile", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Handles demotion of Investigators to Reporters
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Demote(string id)
        {
            try
            {
                var user = _nemesysRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                //Removes current roles and adds reporter
                await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(_nemesysRepository.GetUserById(id)).Result);
                await _userManager.AddToRoleAsync(user, "Reporter");

                _logger.LogInformation("User with id " + id + " has been demoted to Reporter");

                return RedirectToAction("Index", "Profile", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return View("Error");
            }
        }

        //Action to sign out user (instead of default identity Identity/Account/Logout)
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
