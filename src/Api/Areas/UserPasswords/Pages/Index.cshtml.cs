using CloudDrive.Services.UserPasswords;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.UserPasswords.Pages
{
	public class IndexPage : PageModel
	{
		private readonly IUserPasswordsService _service;

		public List<UserPasswordDto> UserPasswords { get; set; }

		public IndexPage(
			IUserPasswordsService service
		)
		{
			_service = service;
		}

		public async Task OnGet()
		{	
			ViewData["PageTitle"] = "User Passwords";
			UserPasswords = await _service.GetAsync();
		}
	}
}