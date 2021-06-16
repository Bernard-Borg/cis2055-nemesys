using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nemesys.CustomAttributes;
using Nemesys.Models;

namespace Nemesys.Areas.Identity.Pages.Account.Manage
{
    public class PhotoModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PhotoModel> _logger;

        public PhotoModel(
            UserManager<User> userManager,
            IWebHostEnvironment environment,
            ILogger<PhotoModel> logger)
        {
            _userManager = userManager;
            _environment = environment;
            _logger = logger;
        }
        public string ImageUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".webp" })]
            [MaxFileSize(10 * 1024 * 1024)]
            [Display(Name = "New photo")]
            public IFormFile NewPhoto { get; set; }
        }

        private async Task LoadAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var photo = user.Photo;
                ImageUrl = photo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                await LoadAsync();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return Redirect("/Error/500");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (Input.NewPhoto != null)
                {
                    user.Photo = "/images/reportimages/" + Guid.NewGuid().ToString() + "_" + Input.NewPhoto.FileName;
                    Input.NewPhoto.CopyTo(new FileStream(_environment.WebRootPath + user.Photo, FileMode.Create));
                    await _userManager.UpdateAsync(user);
                }
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Data);
                return Redirect("/Error/500");
            }   
        }
    }
}
