using CloudDrive.Services.UserPasswords;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.UserPasswords.Pages
{
	public class DeletePage : PageModel
	{
		private readonly IUserPasswordsService _service;

		[BindProperty]
		public UserPasswordDto UserPassword { get; set; }

		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }

		public DeletePage(
			IUserPasswordsService service
		)
		{
			_service = service;
		}

		public async Task OnGet()
		{
			ViewData["PageTitle"] = "On Delete";

			if (Id > 0)
			{
				var result = await _service.GetAsync(Id);

				if (result.IsSuccssfull)
				{
					UserPassword = result.Data;
				}
			}
		}

		public async Task<IActionResult> OnPost()
		{
			await _service.DeleteAsync(UserPassword.Id);

			return LocalRedirect("/user-passwords");
		}
	}
}