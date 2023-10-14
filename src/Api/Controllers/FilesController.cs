using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace CloudDrive.Controllers
{
	[Route("/api/Files")]
	[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
	public class FilesController : ControllerBase
	{
		[HttpGet]
		public string Get()
		{
			return "Hello world!";
		}
	}
}