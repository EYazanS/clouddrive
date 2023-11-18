using CloudDrive.Services.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CloudDrive.Api.Controllers
{
	[Route("/")]
	public class HomeController : Controller
	{
		private readonly IFilesService _service;

		public HomeController(IFilesService service)
		{
			_service = service;
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
	}
}