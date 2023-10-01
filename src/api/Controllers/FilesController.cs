using Microsoft.AspNetCore.Mvc;

namespace clouddrive.Controllers
{
	[Route("/api/Files")]
	public class FilesController : ControllerBase
	{
		[HttpGet]
		public string Get()
		{
			return "Hello world!";
		}
	}
}