using CloudDrive.Services.Note;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CloudDrive.Api.Controllers
{
	[Route("/notes")]
	public class NotesController : Controller
	{
		private readonly INotesService _service;

		public NotesController(INotesService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var notes = await _service.Get();

			return View(notes);
		}

		[HttpGet("Create")]
		public IActionResult Create()
		{
			return View("Form");
		}

		[HttpPost("Create")]
		public async Task<IActionResult> PostCreate([FromForm] NoteDto note)
		{
			if (!ModelState.IsValid)
			{
				return View("Form");
			}

			await _service.Insert(note);

			return LocalRedirect("/notes");
		}
	}
}