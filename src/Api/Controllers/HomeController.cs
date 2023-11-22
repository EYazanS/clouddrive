using CloudDrive.Domain.Entities;
using CloudDrive.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

		private RoleManager<IdentityRole> _roleManager;
		private UserManager<AppUser> _userManager;

		public HomeController(
			IFilesService service,
			IEmailSender emailSender,
			UserManager<AppUser> userManager,
			RoleManager<IdentityRole> roleManager
		)
		{
			_service = service;
			_emailSender = emailSender;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			if (!await _roleManager.RoleExistsAsync("Admin"))
			{
				await _roleManager.CreateAsync(new IdentityRole("Admin"));
			}

			await _userManager.AddToRoleAsync(await _userManager.GetUserAsync(User), "Admin");

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