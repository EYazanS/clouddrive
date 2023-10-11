using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
	[Route("/api/Files")]
	[Authorize]
	public class FilesController : ControllerBase
	{
		[HttpGet]
		public string Get()
		{
			return "Hello world!";
		}
	}
}