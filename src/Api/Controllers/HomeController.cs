using CloudDrive.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CloudDrive.Api.Controllers
{
	[Route("/")]
	public class HomeController : Controller
	{
		private readonly IFilesService _service;
		private readonly IEmailSender _emailSender;

		public HomeController(
			IFilesService service,
			IEmailSender emailSender
		)
		{
			_service = service;
			_emailSender = emailSender;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var files = await _service.Get();

			return View(files);
		}

		[HttpGet("/Terms")]
		public IActionResult Terms()
		{
			return View();
		}

		[HttpGet("/TestEmail")]
		public async Task<IActionResult> TestEmailAsync(string email)
		{
			await _emailSender.SendEmailAsync(email, "Hello", "<h1>Test Email</h1>");

			return Ok();
		}
	}
}