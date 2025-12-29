using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace KaufmannPro.Web.Pages.Account
{
    [AllowAnonymous] // Wichtig! Damit die Login-Seite nicht durch Authentifizierung geschützt wird
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public LoginModel(SignInManager<IdentityUser<int>> signInManager,
                          UserManager<IdentityUser<int>> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= "/Dashboard"; // einfacher string reicht jetzt

            if (!ModelState.IsValid)
                return Page();

            var result = await _signInManager.PasswordSignInAsync(
                Input.Username,
                Input.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl); // funktioniert jetzt zuverlässig
            }

            ModelState.AddModelError(string.Empty, "Ungültiger Benutzername oder Passwort.");
            return Page();
        }
    }
}
