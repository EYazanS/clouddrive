using Microsoft.AspNetCore.Http;

namespace CloudDrive.Services.Files
{
	public interface IFilesService
	{
		void Insert(IFormFile file);
	}

	public class FilesService : IFilesService
	{
		private readonly FileConfigurations _fileConfigurations;

		public FilesService(
			FileConfigurations fileConfigurations
		)
		{
			_fileConfigurations = fileConfigurations;
		}

		// TODO: save file name, new file name and path to database
		public async void Insert(IFormFile file)
		{
			// Path
			var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

			string path = Path.Combine(_fileConfigurations.FileSavePath, newFileName);

			// Contet type
			var originalName = file.FileName;

			// file name
			using (Stream stream = File.Create(path))
			{
				await file.CopyToAsync(stream);
			}
		}
	}
}