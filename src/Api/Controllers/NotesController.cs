using CloudDrive.Domain.Entities;
using CloudDrive.Services.Files;
using CloudDrive.Services.Note;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
	[Route("/api/Notes")]
	public class NotesController : ControllerBase
	{
		private readonly INotesService _ControllerService;

		public NotesController(INotesService ControllerService)
		{
			_ControllerService = ControllerService;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _ControllerService.Get());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _ControllerService.Get(id);

			if (result.IsSuccssfull)
			{
				return Ok(result.Data);
			}

			return NotFound(result);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromForm] Notes note)
		{
			var result = await _ControllerService.Insert(note);

			if (result.IsSuccssfull)
			{
				return Ok(result);
			}

			return BadRequest(result);
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var result = await _ControllerService.Delete(id);

			if (result.IsSuccssfull)
			{
				return NoContent();
			}
			else
			{
				return BadRequest(result);
			}
		}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromForm] Notes note)
        {
            var result = await _ControllerService.Update(note);

            if (result.IsSuccssfull)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
	}
}