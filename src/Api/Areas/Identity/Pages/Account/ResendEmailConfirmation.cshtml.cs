using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using CloudDrive.Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;

namespace CloudDrive.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class ResendEmailConfirmationModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IStringLocalizer<SharedResource> _localizer;

		public ResendEmailConfirmationModel(
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
		}

		public void OnGet()
		{
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
				ModelState.AddModelError(string.Empty, _localizer["Verification email sent. Please check your email."]);
				return Page();
			}

			var userId = await _userManager.GetUserIdAsync(user);

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

			var callbackUrl = Url.Page(
				"/Account/ConfirmEmail",
				pageHandler: null,
				values: new { userId = userId, code = code },
				protocol: Request.Scheme);

			string url = HtmlEncoder.Default.Encode(callbackUrl);

			// string mailTxt = EmailTemplateManager.GetEmailTemplate("ConfirmEmail");

			// mailTxt = mailTxt.Replace(":personName", $"{user.FirstName} {user.LastName}");

			// mailTxt = mailTxt.Replace(":url", url.Trim());

			// await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", mailTxt);

			ModelState.AddModelError(string.Empty, _localizer["Verification email sent. Please check your email."]);

			return Page();
		}
	}
}
