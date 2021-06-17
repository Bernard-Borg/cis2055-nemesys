using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nemesys.Models;
using Microsoft.Extensions.Logging;

namespace Nemesys.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            UserManager<User> userManager,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _logger = logger ;
        }

        public string Alias { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(20, ErrorMessage = "Cannot have a username with more than 20 characters")]
            [MinLength(3, ErrorMessage = "Cannot have a username with less than 3 characters")]
            [Display(Name = "New username")]
            public string NewAlias { get; set; }
        }

        private async Task LoadAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var alias = user.Alias;
                Alias = alias;
                Input = new InputModel
                {
                    NewAlias = alias
                };
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
                    await LoadAsync();
                    return Page();
                }

                var alias = user.Alias;
                if (Input.NewAlias != alias)
                {
                    user.Alias = Input.NewAlias;
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
