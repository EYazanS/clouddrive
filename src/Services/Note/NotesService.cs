using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudDrive.Services.Note
{
	public interface INotesService
	{
		Task<List<NoteDto>> Get();
		Task<Result<NoteDto>> Get(int id);
		Task<Result<NoteDto>> Insert(NoteDto notes);
		Task<Result<NoteDto>> Update(int id, NoteDto notes);
		Task<Result> Delete(int id);
	}

	public class NotesService : INotesService
	{
		private readonly AppDbContext _db;
		private readonly ILogger<NotesService> _logger;
		private readonly UserService _currentUser;

		public NotesService(
			AppDbContext db,
			ILogger<NotesService> logger,
			UserService currentUser
			)
		{
			_db = db;
			_logger = logger;
			_currentUser = currentUser;
		}

		public async Task<List<NoteDto>> Get()
		{
			var data = await _db.Notes.Where(x => x.UserId == _currentUser.UserId).Include(x => x.Notebook).ToListAsync();

			List<NoteDto> results = new List<NoteDto>();

			foreach (var item in data)
			{
				results.Add(new NoteDto
				{
					Id = item.Id,
					Title = item.Title,
					Tags = item.Tags,
					CreateDate = item.CreateDate,
					UserId = item.UserId,
					NotebookId = item.NotebookId,
					Notebook = new()
					{
						Name = item.Notebook?.Name,
						Color = item.Notebook?.Color,
						Category = item.Notebook?.Category,
					}
				});
			}

			return results;
		}

		public async Task<Result<NoteDto>> Get(int id)
		{
			var data = await _db.Notes.FindAsync(id);

			if (data == null)
			{
				return new Result<NoteDto>
				{
					Message = "Note not found",
					IsSuccssfull = false,
				};
			}

			return new Result<NoteDto>
			{
				IsSuccssfull = true,
				Data = new NoteDto
				{
					Id = data.Id,
					Title = data.Title,
					Tags = data.Tags,
					CreateDate = data.CreateDate,
					UserId = data.UserId,
				}
			};
		}

		public async Task<Result<NoteDto>> Insert(NoteDto note)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{
				var data = new Notes
				{
					Title = note.Title,
					Tags = note.Tags,
					CreateDate = note.CreateDate,
					UserId = _currentUser.UserId,
					NotebookId = note.NotebookId
				};

				_db.Notes.Add(data);

				await _db.SaveChangesAsync();

				_logger.LogInformation(
					"Inserted '{title}' with tags '{tags}' created on '{createDate}' by user '{userId}' to database with id: {id}",
					data.Title,
					data.Tags,
					data.CreateDate,
					data.UserId,
					data.Id
				);

				transaction.Commit();

				return new Result<NoteDto>
				{
					Message = "Inserted",
					IsSuccssfull = true,
					Data = new NoteDto
					{
						Id = data.Id,
						Title = data.Title,
						Tags = data.Tags,
						CreateDate = data.CreateDate,
						UserId = data.UserId,
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to save note because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result<NoteDto>
				{
					Message = "Error while trying to save note due to technical reason with code: " + ex.HResult,
					IsSuccssfull = false,
				};
			}
		}

		public async Task<Result> Delete(int id)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{
				var data = await _db.Notes.FindAsync(id);

				if (data == null)
				{
					return new Result
					{
						Message = "Note not found",
						IsSuccssfull = false,
					};
				}

				_db.Notes.Remove(data);

				_logger.LogInformation(
					"Deleted note with id: {id}",
					data.Id
				);

				await _db.SaveChangesAsync();

				transaction.Commit();

				return new Result
				{
					Message = "Deleted",
					IsSuccssfull = true,
				};
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to delete note because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result
				{
					Message = "Error while trying to delete note due to technical reason with code: " + ex.HResult,
					IsSuccssfull = false,
				};
			}
		}

		public async Task<Result<NoteDto>> Update(int id, NoteDto notes)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{
				var data = await _db.Notes.FindAsync(id);

				if (data == null)
				{
					return new Result<NoteDto>
					{
						Message = "Note not found",
						IsSuccssfull = false,
					};
				}

				data.Title = notes.Title;
				data.Tags = notes.Tags;
				data.UserId = notes.UserId;

				_db.Notes.Update(data);

				await _db.SaveChangesAsync();

				_logger.LogInformation(
					"Updated '{title}' with tags '{tags}' created on '{createDate}' by user '{userId}' to database with id: {id}",
					data.Title,
					data.Tags,
					data.CreateDate,
					data.UserId,
					data.Id
				);

				transaction.Commit();

				return new Result<NoteDto>
				{
					Message = "Updated",
					IsSuccssfull = true,
					Data = new NoteDto
					{
						Id = data.Id,
						Title = data.Title,
						Tags = data.Tags,
						CreateDate = data.CreateDate,
						UserId = data.UserId,
					}
				};

			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to update note because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result<NoteDto>
				{
					Message = "Error while trying to update note due to technical reason with code: " + ex.HResult,
					IsSuccssfull = false,
				};
			}
		}
	}
}