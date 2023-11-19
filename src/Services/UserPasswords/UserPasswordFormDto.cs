using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Services.UserPasswords
{
	public class UserPasswordFormDto
	{	
		public string Title { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string Site { get; set; }
		public string Category { get; set; }
	}
}