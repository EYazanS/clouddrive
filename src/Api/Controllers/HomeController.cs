using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CloudDrive.Api.Controllers
{
	[Route("/")]
	public class HomeController : Controller
	{
		[HttpGet]
		public IActionResult Get()
		{
			return View();
		}
	}
}