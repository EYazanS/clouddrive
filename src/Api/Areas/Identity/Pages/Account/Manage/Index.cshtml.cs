using CloudDrive.Domain.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CloudDrive.Areas.Identity.Pages.Account.Manage
{
	public partial class IndexModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IStringLocalizer<SharedResource> _localizer;

		public IndexModel(
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			IStringLocalizer<SharedResource> localizer
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_localizer = localizer;
		}

		[Display(Name = "Email")]
		public string Username { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		public string UserId { get; set; }

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Phone]
			[Display(Name = "PhoneNumber")]
			public string PhoneNumber { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Text)]
			[Display(Name = "FirstName")]
			public string FirstName { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Text)]
			[Display(Name = "LastName")]
			public string LastName { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Date)]
			[Display(Name = "DateOfBirth")]
			public DateTime? DateOfBirth { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[Display(Name = "Gender")]
			public string Gender { get; set; }


			[Display(Name = "Facebook")]
			[DataType(DataType.Url)]
			public string Facebook { get; set; }


			[Display(Name = "LinkedIn")]
			[DataType(DataType.Url)]
			public string LinkedIn { get; set; }


			public string Logo { get; set; }

			public IFormFile LogoFile { get; set; }
		}

		private async Task LoadAsync(AppUser user)
		{
			var userName = await _userManager.GetUserNameAsync(user);
			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

			Username = userName;
			UserId = user.Id;

			Input = new InputModel
			{
				PhoneNumber = phoneNumber,
			};
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

			user.PhoneNumber = Input.PhoneNumber;

			var updateResult = await _userManager.UpdateAsync(user);

			if (!updateResult.Succeeded)
			{
				StatusMessage = _localizer["Unexpected error when trying to update."];
				return RedirectToPage();
			}

			await _signInManager.RefreshSignInAsync(user);

			StatusMessage = _localizer["Your profile has been updated"];

			return RedirectToPage();
		}
	}
}
