using CloudDrive.Services.Files;
using CloudDrive.Services.Note;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.Notes.Pages
{
	public class IndexPage : PageModel
	{
		private readonly INotesService _service;

		public List<NoteDto> Notes { get; set; }

		public IndexPage(
			INotesService service
		)
		{
			_service = service;
		}

		public async Task OnGet()
		{
			Notes = await _service.Get();
		}
	}
}