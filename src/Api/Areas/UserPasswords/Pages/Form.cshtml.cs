using CloudDrive.Services.UserPasswords;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.UserPasswords.Pages
{
	public class FormPage : PageModel
	{
		private readonly IUserPasswordsService _service;

		[BindProperty]
		public UserPasswordFormDto UserPassword { get; set; }

		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }

		public FormPage(
			IUserPasswordsService service
		)
		{
			_service = service;
			UserPassword = new UserPasswordFormDto();
		}

		public void OnGetCreate()
		{
			ViewData["PageTitle"] = "On Create";
		}

		public async Task OnGetUpdateAsync()
		{
			ViewData["PageTitle"] = "On Update";

			if (Id > 0)
			{
				var result = await _service.GetAsync(Id);

				if (result.IsSuccssfull)
				{
					UserPassword.Title = result.Data.Title;
					UserPassword.Username = result.Data.Username;
				}
			}
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (Id > 0)
			{
				await _service.UpdateAsync(Id, UserPassword);
			}
			else
			{
				await _service.InsertAsync(UserPassword);
			}

			return LocalRedirect("/user-passwords");
		}
	}
}