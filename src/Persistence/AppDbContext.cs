
using clouddrive.Domain.Entities;
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
		public DbSet<Notebook> Notebooks { get; set; }
	}
}
