using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudDrive.Services.Users
{
	public interface IUsersService
	{
		Task<List<UserPasswordsDto>> Get();
		Task<Result<UserPasswordsDto>> Get(int id);
		Task<Result<UserPasswordsDto>> Insert(UserPasswordFormDto user);
		Task<Result<UserPasswordsDto>> Update(int id, UserPasswordFormDto user);
		Task<Result> Delete(int id);
	}
	public class UsersService : IUsersService
	{
		private readonly AppDbContext _db;
		private readonly ILogger<UsersService> _logger;
		public UsersService(
			AppDbContext db,
			ILogger<UsersService> logger
		)
		{
			_db = db;
			_logger = logger;
		}

		public async Task<List<UserPasswordsDto>> Get()
		{
			var usersList = await _db.UserPasswords.ToListAsync();

			List<UserPasswordsDto> usersDtoList = new List<UserPasswordsDto>();

			foreach (var user in usersList)
			{
				usersDtoList.Add(new UserPasswordsDto
				{
					Id = user.Id,
					Title = user.Title,
					Username = user.Username,
					CreateDate = user.CreateDate
				});
			}

			return usersDtoList;
		}

		public async Task<Result<UserPasswordsDto>> Get(int id)
		{
			var user = await _db.UserPasswords.FindAsync(id);

			if (user == null)
			{
				return new Result<UserPasswordsDto>
				{
					IsSuccssfull = false,
					Message = "User not found",
				};
			}

			return new Result<UserPasswordsDto>
			{
				IsSuccssfull = true,
				Data = new UserPasswordsDto
				{
					Id = user.Id,
					Title = user.Title,
					Username = user.Username,
					CreateDate = user.CreateDate
				}
			};
		}

		public async Task<Result<UserPasswordsDto>> Insert(UserPasswordFormDto user)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{
				UserPasswords entity = new UserPasswords();

				entity.Title = user.Title;
				entity.Username = user.Username;
				entity.Password = user.Password;
				entity.Site = user.Site;
				entity.Category = user.Category;
				entity.CreateDate = DateTime.Now;

				_db.UserPasswords.Add(entity);

				await _db.SaveChangesAsync();

				_logger.LogInformation(
					"Saved a new user to database with username '{Username}' and id: {Id}",
					entity.Username,
					entity.Id
				);

				transaction.Commit();

				return new Result<UserPasswordsDto>
				{
					IsSuccssfull = true,
					Message = "Inserted",
					Data = new UserPasswordsDto
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
				_logger.LogError("Failed to save user because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result<UserPasswordsDto>
				{
					IsSuccssfull = false,
					Message = "Error while trying to save user due to technical reason with code: " + ex.HResult,
				};
			}
		}

		public async Task<Result<UserPasswordsDto>> Update(int id, UserPasswordFormDto user)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{

				var entity = await _db.UserPasswords.FindAsync(id);

				if (entity == null)
				{
					return new Result<UserPasswordsDto>
					{
						IsSuccssfull = false,
						Message = "User not found",
					};
				}

				entity.Title = user.Title;
				entity.Username = user.Username;
				entity.Password = user.Password;
				entity.Site = user.Site;
				entity.Category = user.Category;

				_logger.LogInformation(
					"User found"
				);

				_db.UserPasswords.Update(entity);

				await _db.SaveChangesAsync();

				transaction.Commit();

				return new Result<UserPasswordsDto>
				{
					IsSuccssfull = true,
					Message = "Updated",
					Data = new UserPasswordsDto
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
				_logger.LogError("Failed to update user because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result<UserPasswordsDto>
				{
					IsSuccssfull = false,
					Message = "Error while trying to update the user due to technical reason with code: " + ex.HResult,
				};
			}
		}
		public async Task<Result> Delete(int id)
		{
			var transaction = _db.Database.BeginTransaction();

			try
			{

				var user = await _db.UserPasswords.FindAsync(id);
				if (user == null)
				{
					return new Result
					{
						IsSuccssfull = false,
						Message = "User not found",
					};
				}
				_db.UserPasswords.Remove(user);

				await _db.SaveChangesAsync();

				_logger.LogInformation(
					"Deleted user from database with username '{Username}' and id: {Id}",
					user.Username,
					user.Id
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
				_logger.LogError("Failed to delete user because of exception: '{Message}'", ex.Message);

				transaction.Rollback();

				return new Result
				{
					IsSuccssfull = false,
					Message = "Error while trying to delete the user due to technical reason with code: " + ex.HResult,
				};
			}
		}

	}
}