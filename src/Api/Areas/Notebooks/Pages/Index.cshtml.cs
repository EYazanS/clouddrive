using CloudDrive.Services.Notebooks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudDrive.Api.Areas.Notebooks.Pages
{
    public class IndexPage : PageModel
    {
        private readonly INotebooksService _service;

        [BindProperty]
        public List<NotebookDto> Notebooks { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public IndexPage(INotebooksService service)
        {
            _service = service;
        }

        public async Task OnGet()
        {
            ViewData["PageTitle"] = "Index page";

            Notebooks = await _service.Get();
        }
    }
}
