
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using CloudDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Persistence
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Data> Data { get; set; }
		public DbSet<UserPasswords> UserPasswords { get; set; }
	}
}
