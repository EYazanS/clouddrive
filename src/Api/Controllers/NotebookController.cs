using CloudDrive.Services.Files;
using CloudDrive.Services.Notebooks;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
	[Route("/api/Notebooks")]

	public class NotebookController : ControllerBase
	{
		private readonly INotebooksService _service;

		public NotebookController(INotebooksService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _service.Get());
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] NotebookDto notebook)
		{
			var result = await _service.Insert(notebook);

			if (result.IsSuccssfull)
			{
				return Ok(result.Data);
			}

			return BadRequest(result);
		}

	}

}