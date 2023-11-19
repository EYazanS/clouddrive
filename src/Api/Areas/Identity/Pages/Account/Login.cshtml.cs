using CloudDrive.Domain.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class LoginModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ILogger<LoginModel> _logger;
		private readonly IStringLocalizer<SharedResource> _localizer;

		public LoginModel(SignInManager<AppUser> signInManager,
			ILogger<LoginModel> logger,
			UserManager<AppUser> userManager,
			IStringLocalizer<SharedResource> localizer
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_localizer = localizer;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public string ReturnUrl { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		[TempData]
		public bool NeedConfirmation { get; set; }

		public class InputModel
		{
			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.EmailAddress)]
			[Display(Name = "Email")]
			public string Username { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			returnUrl ??= Url.Content("~/");

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);

				var user = await _userManager.FindByEmailAsync(Input.Username);

				if (user == null)
				{
					ModelState.AddModelError(string.Empty, _localizer["Incorrect email or password"]);
					return Page();
				}

				if (result.Succeeded)
				{
					_logger.LogInformation("User logged in.");

					return LocalRedirect(returnUrl);
				}

				if (result.RequiresTwoFactor)
				{
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
				}

				if (result.IsLockedOut)
				{
					_logger.LogWarning("User account locked out.");
					return RedirectToPage("./Lockout");
				}
				else
				{
					if (!await _userManager.IsEmailConfirmedAsync(user))
					{
						NeedConfirmation = true;
					}
					else
					{
						ModelState.AddModelError(string.Empty, _localizer["Incorrect email or password"]);
					}

					return Page();
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
