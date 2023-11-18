using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Services.Notebooks
{
    public class NotebookDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        [Required]
        public string Category { get; set; }
        public string Color { get; set; }
    }
}
