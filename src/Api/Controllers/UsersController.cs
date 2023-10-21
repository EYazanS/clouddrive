using CloudDrive.Domain.Entities;
using CloudDrive.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
	[Route("api/Users")]

	public class UsersController : ControllerBase
	{
		private readonly IUsersService _usersService;
		public UsersController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _usersService.Get());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _usersService.Get(id);

			if (result.IsSuccssfull)
			{
				return Ok(result.Data);
			}

			return NotFound(result);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UserPasswordFormDto user)
		{
			var result = await _usersService.Insert(user);

			if (result.IsSuccssfull)
			{
				return Ok(result.Data);
			}

			return BadRequest(result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserPasswordFormDto user)
		{
			var result = await _usersService.Update(id, user);

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
			var result = await _usersService.Delete(id);

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