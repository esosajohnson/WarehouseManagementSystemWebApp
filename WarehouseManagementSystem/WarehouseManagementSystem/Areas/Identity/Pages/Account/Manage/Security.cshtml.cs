using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public class SecurityModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SecurityModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        public bool Is2faEnabled { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Unable to load user.");

            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostDisable2faAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Unable to load user.");

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
                StatusMessage = "Unexpected error when disabling 2FA.";
            else
                StatusMessage = "Two-factor authentication has been disabled.";

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
