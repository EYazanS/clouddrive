using CloudDrive.Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace CloudDrive.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class ResetPasswordModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IStringLocalizer<SharedResource> _localizer;

		public ResetPasswordModel(
			UserManager<AppUser> userManager,
			IStringLocalizer<SharedResource> localizer
		)
		{
			_userManager = userManager;
			_localizer = localizer;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required(ErrorMessage = "The {0} field is required.")]
			[EmailAddress(ErrorMessage = "Please enter a valid email")]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			[Display(Name = "Confirm Password")]
			public string ConfirmPassword { get; set; }

			public string Code { get; set; }
		}

		public IActionResult OnGet(string code = null)
		{
			if (code == null)
			{
				return BadRequest("A code must be supplied for password reset.");
			}
			else
			{
				Input = new InputModel
				{
					Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
				};

				return Page();
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = await _userManager.FindByEmailAsync(Input.Email);

			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToPage("./ResetPasswordConfirmation");
			}

			if (await _userManager.CheckPasswordAsync(user, Input.Password))
			{
				ModelState.AddModelError(string.Empty, _localizer["Used password"]);
			}
			else
			{
				var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

				if (result.Succeeded)
				{
					return RedirectToPage("./ResetPasswordConfirmation");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return Page();
		}
	}
}
