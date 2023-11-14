using CloudDrive.Services.Note;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace CloudDrive.Api.Areas.Notes.Pages
{
	public class FormPage : PageModel
	{
		private readonly INotesService _service;

		[BindProperty]
		public NoteDto Note { get; set; }

		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }
		public IStringLocalizer<Resource> Localizer { get; }

		public FormPage(
			INotesService service,
			IStringLocalizer<Resource> localizer
		)
		{
			_service = service;
			Localizer = localizer;
		}

		public void OnGetCreate()
		{
			ViewData["PageTitle"] = "On Create";
		}

		public async Task OnGetUpdateAsync()
		{
			ViewData["PageTitle"] = "On Update";

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


	}
}
