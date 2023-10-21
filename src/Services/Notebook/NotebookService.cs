using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudDrive.Services.Notebook
{
	public interface INotebookService
	{
		Task<List<NotebookDto>> Get();

	}

	public class NotebookService : INotebookService
	{
		private readonly AppDbContext _db;
		private readonly ILogger<NotebookService> _logger;
		//private readonly NotebookConfigurations _NotebookConfigurations;


		public NotebookService(
			AppDbContext db,
			ILogger<NotebookService> logger
			//NotebookConfigurations NotebookConfigurations
		)
		{
			_db = db;
			_logger = logger;
			//_NotebookConfigurations = NotebookConfigurations;
		}
			public async Task<List<NotebookDto>> Get()
		{
			var notebook = await _db.Notebook.ToListAsync();

			List<NotebookDto> results = new List<NotebookDto>();

			foreach (var item in notebook)
			{
				results.Add(new NotebookDto
				{
					Id = item.Id,
					Name = item.Name,
					CreateDate = item.CreateDate,
					Category= item.Category,
					Color=item.Color
				});
			}
			return results;
		}
	}
}