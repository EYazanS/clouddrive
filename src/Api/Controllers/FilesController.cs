using CloudDrive.Services.Files;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
	[Route("/api/Files")]
	public class FilesController : ControllerBase
	{
		private readonly IFilesService _filesService;

		public FilesController(IFilesService filesService)
		{
			_filesService = filesService;
		}

		[HttpGet]
		public string Get()
		{
			return "Hello world!";
		}


		[HttpGet("Anon")]
		public string Get0()
		{
			return "Hello world!";
		}

		[HttpPost]
		public string Post([FromForm] IFormFile file)
		{
			_filesService.Insert(file);

			return "Inserted";
		}
	}
}