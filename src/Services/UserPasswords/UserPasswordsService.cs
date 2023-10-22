using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudDrive.Services.Users
{
	public interface IUsersService
	{
		Task<List<UserPasswordDto>> Get();
		Task<Result<UserPasswordDto>> Get(int id);
		Task<Result<UserPasswordDto>> Insert(UserPasswordFormDto user);
		Task<Result<UserPasswordDto>> Update(int id, UserPasswordFormDto user);
		Task<Result> Delete(int id);
	}
	public class UserPasswordsService : IUsersService
	{
		private readonly AppDbContext _db;
		private readonly ILogger<UserPasswordsService> _logger;
		public UserPasswordsService(
			AppDbContext db,
			ILogger<UserPasswordsService> logger
		)
		{
			_db = db;
			_logger = logger;
		}

		public async Task<List<UserPasswordDto>> Get()
		{
			var entityList = await _db.UserPassword.ToListAsync();

			List<UserPasswordDto> usersDtoList = new List<UserPasswordDto>();

			foreach (var entity in entityList)
			{
				usersDtoList.Add(new UserPasswordDto
				{
					Id = entity.Id,
					Title = entity.Title,
					Username = entity.Username,
					CreateDate = entity.CreateDate
				});
			}

			return usersDtoList;
		}

		public async Task<Result<UserPasswordDto>> Get(int id)
		{
			var entity = await _db.UserPassword.FindAsync(id);

			if (entity == null)
			{
				return new Result<UserPasswordDto>
				{
					IsSuccssfull = false,
					Message = "User password not found",
				};
			}

			return new Result<UserPasswordDto>
			{
				IsSuccssfull = true,
				Data = new UserPasswordDto
				{
					Id = entity.Id,
					Title = entity.Title,
					Username = entity.Username,
					CreateDate = entity.CreateDate
				}
			};
		}

		public async Task<Result<UserPasswordDto>> Insert(UserPasswordFormDto user)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{
				UserPassword entity = new UserPassword();

				entity.Title = user.Title;
				entity.Username = user.Username;
				entity.Password = user.Password;
				entity.Site = user.Site;
				entity.Category = user.Category;
				entity.CreateDate = DateTime.Now;

				_db.UserPassword.Add(entity);

				await _db.SaveChangesAsync();

				_logger.LogInformation(
					"Saved a new user password to database with title '{Title}' and id: {Id}",
					entity.Title,
					entity.Id
				);

				transaction.Commit();

				return new Result<UserPasswordDto>
				{
					IsSuccssfull = true,
					Message = "Inserted",
					Data = new UserPasswordDto
					{
						Id = entity.Id,
						Username = entity.Username,
						Title = entity.Title,
						CreateDate = entity.CreateDate
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to save user password because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result<UserPasswordDto>
				{
					IsSuccssfull = false,
					Message = "Error while trying to save user password due to technical reason with code: " + ex.HResult,
				};
			}
		}

		public async Task<Result<UserPasswordDto>> Update(int id, UserPasswordFormDto user)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{
				var entity = await _db.UserPassword.FindAsync(id);

				if (entity == null)
				{
					return new Result<UserPasswordDto>
					{
						IsSuccssfull = false,
						Message = "User password not found",
						Data = new UserPasswordDto
						{
							Id = entity.Id,
							Username = entity.Username,
							Title = entity.Title,
							CreateDate = entity.CreateDate
						}
					};
				}

				_logger.LogInformation(
					"User password found with title '{Title}' and id: {Id}",
					entity.Title,
					entity.Id
				);

				entity.Title = user.Title;
				entity.Username = user.Username;
				entity.Password = user.Password;
				entity.Site = user.Site;
				entity.Category = user.Category;

				_db.UserPassword.Update(entity);

				await _db.SaveChangesAsync();

				transaction.Commit();

				return new Result<UserPasswordDto>
				{
					IsSuccssfull = true,
					Message = "Updated",
					Data = new UserPasswordDto
					{
						Id = entity.Id,
						Username = entity.Username,
						Title = entity.Title,
						CreateDate = entity.CreateDate
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to update user password because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result<UserPasswordDto>
				{
					IsSuccssfull = false,
					Message = "Error while trying to update the user password due to technical reason with code: " + ex.HResult,
				};
			}
		}
		public async Task<Result> Delete(int id)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{
				var entity = await _db.UserPassword.FindAsync(id);

				if (entity == null)
				{
					return new Result
					{
						IsSuccssfull = false,
						Message = "User password not found",
					};
				}

				_db.UserPassword.Remove(entity);

				await _db.SaveChangesAsync();

				_logger.LogInformation(
					"Deleted user from database with title '{Title}' and id: {Id}",
					entity.Title,
					entity.Id
				);

				transaction.Commit();

				return new Result
				{
					IsSuccssfull = true,
					Message = "Deleted from db",
				};
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to delete user password because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result
				{
					IsSuccssfull = false,
					Message = "Error while trying to delete the user password due to technical reason with code: " + ex.HResult,
				};
			}
		}
	}
}