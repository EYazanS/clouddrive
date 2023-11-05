using Microsoft.AspNetCore.Mvc;
using CloudDrive.Services.UserPasswords;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

		[HttpGet("create")]
		public IActionResult Create()
		{
			return View("Form");
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreatePost(UserPasswordFormDto userPassword)
		{	
			if(!ModelState.IsValid)
			{
				return View("Form");
			}

			await _service.InsertAsync(userPassword);

			return LocalRedirect("/user-passwords");
		}
	}
}