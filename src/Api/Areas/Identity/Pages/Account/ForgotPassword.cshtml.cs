using CloudDrive.Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CloudDrive.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class ForgotPasswordModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;

		public ForgotPasswordModel(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required(ErrorMessage = "The {0} field is required.")]
			[EmailAddress(ErrorMessage = "Please enter a valid email")]
			[Display(Name = "Email")]
			[DataType(DataType.EmailAddress)]
			public string Email { get; set; }
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(Input.Email);

				if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return RedirectToPage("./ForgotPasswordConfirmation");
				}

				// For more information on how to enable account confirmation and password reset please 
				// visit https://go.microsoft.com/fwlink/?LinkID=532713
				var code = await _userManager.GeneratePasswordResetTokenAsync(user);

				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

				var callbackUrl = Url.Page(
					"/Account/ResetPassword",
					pageHandler: null,
					values: new { area = "Identity", code },
					protocol: Request.Scheme);

				string url = HtmlEncoder.Default.Encode(callbackUrl);

				// string mailTxt = EmailTemplateManager.GetEmailTemplate("ResetPassword");

				// mailTxt = mailTxt.Replace(":personName", $"{user.FirstName} {user.LastName}");

				// mailTxt = mailTxt.Replace(":url", url.Trim());

				// await _emailSender.SendEmailAsync(Input.Email, "Reset Password", mailTxt);

				return RedirectToPage("./ForgotPasswordConfirmation");
			}

			return Page();
		}
	}
}
