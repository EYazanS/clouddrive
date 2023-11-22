namespace CloudDrive.Domain.Entities
{
	public class UserPassword
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Site { get; set; }
		public string Category { get; set; }
		public DateTime CreateDate { get; set; }

		public string UserId { get; set; }
		public AppUser User { get; set; }
	}
}