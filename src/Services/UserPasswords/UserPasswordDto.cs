namespace CloudDrive.Services.UserPasswords
{
	public class UserPasswordDto
	{
		public int Id { get; set; }
		public string Site { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public DateTime CreateDate { get; set; }
	}
}