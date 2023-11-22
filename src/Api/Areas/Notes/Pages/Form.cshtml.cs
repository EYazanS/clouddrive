using CloudDrive.Domain.Entities;
using CloudDrive.Services.Note;
using CloudDrive.Services.Notebooks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace CloudDrive.Api.Areas.Notes.Pages
{
	public class FormPage : PageModel
	{
		private readonly INotesService _service;
		private readonly INotebooksService _notebooksService;

		[BindProperty]
		public NoteDto Note { get; set; }

		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }

		public List<NotebookDto> Notebooks { get; set; }

		public FormPage(
			INotesService service,
			INotebooksService notebooksService
		)
		{
			_service = service;
			_notebooksService = notebooksService;
		}

		public async Task OnGetCreate()
		{
			ViewData["PageTitle"] = "On Create";

			await FillNotebooks();
		}

		public async Task OnGetUpdateAsync()
		{
			ViewData["PageTitle"] = "On Update";

			await FillNotebooks();

			if (Id > 0)
			{
				var result = await _service.Get(Id);

				if (result.IsSuccssfull)
				{
					Note = result.Data;
				}
			}
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				await FillNotebooks();

				return Page();
			}

			if (Id > 0)
			{
				await _service.Update(Id, Note);
			}
			else
			{
				await _service.Insert(Note);
			}

			return LocalRedirect("/notes");
		}

		public async Task FillNotebooks()
		{
			Notebooks = await _notebooksService.Get();
		}
	}
}
