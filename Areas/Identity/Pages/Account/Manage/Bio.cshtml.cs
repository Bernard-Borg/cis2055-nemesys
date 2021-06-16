﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nemesys.Models;

namespace Nemesys.Areas.Identity.Pages.Account.Manage
{
    public class BioModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public BioModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<BioModel> logger)
        {
            _userManager = userManager;
        }

        public string Bio { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [StringLength(255, ErrorMessage = "The {0} must be at max {1} characters long.")]
            [DataType(DataType.Text)]
            [Display(Name = "New bio")]
            public string NewBio { get; set; }
        }

        private async Task LoadAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var bio = user.Bio;
            Bio = bio;
            Input = new InputModel
            {
                NewBio = bio
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            var bio = user.Bio;
            if (Input.NewBio != bio)
            {
                user.Bio = Input.NewBio;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToPage();
        }
    }
}