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

        [HttpGet("{id}")]
        public async Task<IActionResult> CreateNote(NoteDto newNote)
        {
            if (ModelState.IsValid)
            {
                newNote.CreateDate = DateTime.Now;

                await _service.Insert(newNote); 

                return RedirectToAction(nameof(Get)); 
            }

          
            return View("Error"); 
        }

    }
     
    
}
