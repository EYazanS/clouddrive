using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudDrive.Services.Notebooks
{
	public interface INotebooksService
	{
		Task<List<NotebookDto>> Get();
		Task<Result<NotebookDto>> Insert(NotebookDto notebook);
	}

	public class NotebooksService : INotebooksService
	{
		private readonly AppDbContext _db;
		private readonly ILogger<NotebooksService> _logger;

		public NotebooksService(
			AppDbContext db,
			ILogger<NotebooksService> logger
		)
		{
			_db = db;
			_logger = logger;
		}
		public async Task<List<NotebookDto>> Get()
		{
			var notebook = await _db.Notebooks.ToListAsync();

			List<NotebookDto> results = new List<NotebookDto>();

			foreach (var item in notebook)
			{
				results.Add(new NotebookDto
				{
					Id = item.Id,
					Name = item.Name,
					CreateDate = item.CreateDate,
					Category = item.Category,
					Color = item.Color
				});
			}
			return results;
		}

		public async Task<Result<NotebookDto>> Insert(NotebookDto notebook)
		{
			var transaction = _db.Database.BeginTransaction();
			try
			{

				// LINQ
				var data = new Notebook
				{
					Name = notebook.Name,
					CreateDate = DateTime.Now,
					Category = notebook.Category,
					Color = notebook.Color

				};

				_db.Notebooks.Add(data);

				await _db.SaveChangesAsync();

				_logger.LogInformation(
					"Saved Notebook to databse with  name: '{Name}' with id: {id}",
					data.Id,
					data.Name
				);

				transaction.Commit();

				return new Result<NotebookDto>
				{
					Message = "Inserted",
					IsSuccssfull = true,
					Data = new NotebookDto
					{
						Id = data.Id,
						Name = data.Name,
						CreateDate = data.CreateDate,
						Category = data.Category,
						Color = data.Color
					}
				};

			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to save Notebook because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result<NotebookDto>
				{
					Message = "Error while trying to save Notebook due to technical reason with code: " + ex.HResult,
					IsSuccssfull = false,
				};
			}
		}
	}
}