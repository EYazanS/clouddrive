
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using CloudDrive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Persistence
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Data> Data { get; set; }
		public DbSet<Notebook> Notebooks { get; set; }
		public DbSet<Notes> Notes { get; set; }
		public DbSet<UserPasswords> UserPasswords { get; set; }
		public DbSet<Audit> Audit { get; set; }
	}
}
