using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CloudDrive.Services.Notebooks;

namespace CloudDrive.Api.Controllers
{
    [Route("/Notebooks")]
    public class NotebooksController : Controller
    {
        private readonly INotebooksService _notebookservice;

        public NotebooksController(INotebooksService service)
        {
            _notebookservice = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notebooks = await _notebookservice.Get();

            return View(notebooks);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("Form");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] NotebookDto notebook)
        {
            if (!ModelState.IsValid)
            {
                return View("Form");
            }

            await _notebookservice.Insert(notebook);
            return LocalRedirect("/Notebooks");
        }
    }
}
