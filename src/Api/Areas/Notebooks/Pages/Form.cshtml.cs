using CloudDrive.Services.Notebooks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.Notebooks.Pages
{
    public class FormPage : PageModel
    {
        private readonly INotebooksService _service;

        [BindProperty]
        public NotebookDto Notebook { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public FormPage(INotebooksService service)
        {
            _service = service;
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
                    Notebook = result.Data;
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
                await _service.Update(Id, Notebook);
            }
            else
            {
                await _service.Insert(Notebook);
            }

            return LocalRedirect("/notebooks");
        }

    
    }
}
