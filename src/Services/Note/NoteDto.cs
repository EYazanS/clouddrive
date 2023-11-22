using System.ComponentModel.DataAnnotations;
using CloudDrive.Services.Notebooks;

namespace CloudDrive.Services.Note
{
	public class NoteDto
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Tags { get; set; }

		public int? NotebookId { get; set; }

		public DateTime CreateDate { get; set; }
		public string UserId { get; set; }

		public NotebookDto Notebook { get; set; }
	}
}