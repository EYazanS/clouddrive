using Microsoft.AspNetCore.Mvc;
using CloudDrive.Services.UserPasswords;

namespace CloudDrive.Controllers
{	
	[Route("/user-passwords")]
	public class UserPasswordsController: Controller
	{	
		private readonly IUserPasswordsService _service;

		public UserPasswordsController(IUserPasswordsService service)
		{
			_service=service;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await _service.GetAsync();
			return View(result);
		}
	}
}