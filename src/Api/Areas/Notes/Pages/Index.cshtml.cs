using CloudDrive.Services.Files;
using CloudDrive.Services.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.Notes.Pages
{
	[Authorize("Admin")]
	public class IndexPage : PageModel
	{
		private readonly INotesService _service;

		public List<NoteDto> Notes { get; set; }

		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }

		public IndexPage(INotesService service)
		{
			_service = service;
		}

		public async Task OnGet()
		{
			ViewData["PageTitle"] = "Index page";

			Notes = await _service.Get();
		}

		public async Task<IActionResult> OnPostDeleteAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (Id > 0)
			{
				await _service.Delete(Id);
			}

			return LocalRedirect("/notes");
		}
	}
}
