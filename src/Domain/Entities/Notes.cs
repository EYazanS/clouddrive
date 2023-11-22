namespace CloudDrive.Domain.Entities
{
	public class Notes
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Tags { get; set; }
		public DateTime CreateDate { get; set; }

		public string UserId { get; set; }
		public AppUser User { get; set; }

		public int? NotebookId { get; set; }
		public Notebook Notebook { get; set; }
	}
}