using Microsoft.AspNetCore.Mvc;

namespace clouddrive.Controllers
{
	public class Name
	{
		public string Value { get; set; }
		public string Value2 { get; set; }
	}

	[Route("/api/Files")]
	public class FilesController : ControllerBase
	{
		[HttpGet]
		public string Get()
		{
			return "Hello world!";
		}

		[HttpPost("{id}")]
		public string Post(
			[FromRoute] int id,
			[FromQuery] int query,
			[FromQuery] string question,
			[FromBody] Name name,
			[FromHeader] string header
		)
		{
			return "Id: " + id + " query:" + query + " question:" + question + " value: " + name.Value + " value2: " + name.Value2 + " header: " + header;
		}
	}
}