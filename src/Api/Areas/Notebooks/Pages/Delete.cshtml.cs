using CloudDrive.Services.Notebooks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.Notebooks.Pages
{
    public class DeletePage : PageModel
    {
        private readonly INotebooksService _service;

        [BindProperty]
        public NotebookDto Notebook { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public DeletePage(INotebooksService service)
        {
            _service = service;
        }

        public async Task OnGet()
        {
            ViewData["Page Title"] = " On Delete";

            if (Id > 0)
            {
                var result = await _service.Get(Id);

                if (result.IsSuccssfull)
                {
                    Notebook = result.Data;
                }
            }
        }

        public async Task<IActionResult> OnPost()
        {
            ViewData["Page Title"] = " On Delete";

            if (Notebook.Id > 0)
            {
                await _service.Delete(Notebook.Id);
            }

            return LocalRedirect("/notebooks");
        }
    }
}
