using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using CloudDrive.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace CloudDrive.Areas.Identity.Pages.Account.Manage
{
	public partial class EmailModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public EmailModel(
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public string Username { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		public bool IsEmailConfirmed { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required(ErrorMessage = "The {0} field is required.")]
			[EmailAddress(ErrorMessage = "Please enter a valid email")]
			[Display(Name = "New email")]
			public string NewEmail { get; set; }
		}

		private async Task LoadAsync(AppUser user)
		{
			var email = await _userManager.GetEmailAsync(user);
			Email = email;

			Input = new InputModel
			{
				NewEmail = email,
			};

			IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await LoadAsync(user);

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
				await LoadAsync(user);
				return Page();
			}

			var email = await _userManager.GetEmailAsync(user);

			if (Input.NewEmail != email)
			{
				var userId = await _userManager.GetUserIdAsync(user);

				var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);

				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

				var callbackUrl = Url.Page(
					"/Account/ConfirmEmailChange",
					pageHandler: null,
					values: new { userId = userId, email = Input.NewEmail, code = code },
					protocol: Request.Scheme);

				string url = HtmlEncoder.Default.Encode(callbackUrl);

				// string mailTxt = EmailTemplateManager.GetEmailTemplate("ConfirmEmail");

				// mailTxt = mailTxt.Replace(":personName", $"{user.FirstName} {user.LastName}");

				// mailTxt = mailTxt.Replace(":url", url.Trim());

				// await _emailSender.SendEmailAsync(Input.NewEmail, "Confirm your email", mailTxt);

				StatusMessage = "Confirmation link to change email sent. Please check your email.";
				return RedirectToPage();
			}

			StatusMessage = "Your email is unchanged.";

			await LoadAsync(user);

			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostSendVerificationEmailAsync()
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!ModelState.IsValid)
			{
				await LoadAsync(user);
				return Page();
			}

			var userId = await _userManager.GetUserIdAsync(user);

			var email = await _userManager.GetEmailAsync(user);

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

			var callbackUrl = Url.Page(
				"/Account/ConfirmEmail",
				pageHandler: null,
				values: new { area = "Identity", userId = userId, code = code },
				protocol: Request.Scheme);


			// string mailTxt = EmailTemplateManager.GetEmailTemplate("ConfirmEmail");

			// string url = HtmlEncoder.Default.Encode(callbackUrl);

			// mailTxt = mailTxt.Replace(":personName", $"{user.FirstName} {user.LastName}");

			// mailTxt = mailTxt.Replace(":url", url.Trim());

			// await _emailSender.SendEmailAsync(email, "Confirm your email", mailTxt);

			StatusMessage = "Verification email sent. Please check your email.";

			return RedirectToPage();
		}
	}
}
