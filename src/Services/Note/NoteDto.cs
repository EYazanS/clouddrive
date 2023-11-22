using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Services.Note
{
	public class NoteDto
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Tags { get; set; }

		public DateTime CreateDate { get; set; }
		public string UserId { get; set; }
	}
}