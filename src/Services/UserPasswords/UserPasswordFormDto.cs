using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Services.UserPasswords
{
	public class UserPasswordFormDto
	{	
		[Required]
		public string Title { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		public string Site { get; set; }
		public string Category { get; set; }
	}
}