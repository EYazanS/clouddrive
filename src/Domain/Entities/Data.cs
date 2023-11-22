namespace CloudDrive.Domain.Entities
{
	public class Data
	{
		public int Id { get; set; }
		public string OriginalFileName { get; set; }
		public string NewFileName { get; set; }
		public string ContentType { get; set; }
		public string Path { get; set; }
		public string UserId { get; set; }
		public AppUser User { get; set; }
	}
}