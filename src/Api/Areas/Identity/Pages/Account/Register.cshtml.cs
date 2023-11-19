using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using CloudDrive.Domain.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CloudDrive.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IStringLocalizer<SharedResource> _localizer;

		public RegisterModel(
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			ILogger<RegisterModel> logger,
			IStringLocalizer<SharedResource> localizer)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_localizer = localizer;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel
		{
			[EmailAddress(ErrorMessage = "Please enter a valid email")]
			[DataType(DataType.EmailAddress)]
			[Required(ErrorMessage = "The {0} field is required.")]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Text)]
			[Display(Name = "FirstName")]
			public string FirstName { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Text)]
			[Display(Name = "LastName")]
			public string LastName { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Text)]
			[Display(Name = "JobTitle")]
			public string JobTitle { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.PhoneNumber)]
			[Display(Name = "PhoneNumber")]
			public string PhoneNumber { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				var user = new AppUser
				{
					UserName = Input.Email,
					Email = Input.Email,
					PhoneNumber = Input.PhoneNumber,
				};

				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = user.Id, code, returnUrl },
						protocol: Request.Scheme);

					string url = HtmlEncoder.Default.Encode(callbackUrl);

					// string mailTxt = EmailTemplateManager.GetEmailTemplate("ConfirmEmail");

					// mailTxt = mailTxt.Replace(":personName", $"{user.FirstName} {user.LastName}");

					// mailTxt = mailTxt.Replace(":url", url.Trim());

					// await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", mailTxt);

					if (_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
					}
					else
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						return new LocalRedirectResult("/Profile/Complete");
					}
				}

				foreach (var error in result.Errors)
				{
					switch (error.Code)
					{
						case "DuplicateEmail":
							ModelState.AddModelError(string.Empty, _localizer["Email '{0}' is already taken.", Input.Email]);
							break;

						default:
							ModelState.AddModelError(string.Empty, _localizer[error.Description]);
							break;
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
