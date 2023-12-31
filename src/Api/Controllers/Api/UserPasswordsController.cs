using CloudDrive.Domain.Entities;
using CloudDrive.Services.UserPasswords;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers.Api
{
	[Route("api/user-passwords")]
	public class ApiUserPasswordsController : ControllerBase
	{
		private readonly IUserPasswordsService _service;
		public ApiUserPasswordsController(IUserPasswordsService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _service.GetAsync());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _service.GetAsync(id);

			if (result.IsSuccssfull)
			{
				return Ok(result.Data);
			}

			return NotFound(result);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UserPasswordFormDto user)
		{
			var result = await _service.InsertAsync(user);

			if (result.IsSuccssfull)
			{
				return Ok(result.Data);
			}

			return BadRequest(result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserPasswordFormDto user)
		{
			var result = await _service.UpdateAsync(id, user);

			if (result.IsSuccssfull)
			{
				return Ok(result.Data);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var result = await _service.DeleteAsync(id);

			if (result.IsSuccssfull)
			{
				return NoContent();
			}
			else
			{
				return BadRequest(result);
			}
		}
	}
}