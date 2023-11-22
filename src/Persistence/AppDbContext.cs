using CloudDrive.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Persistence
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder
				.Entity<AppUser>()
				.HasMany(user => user.CreditCards)
				.WithOne(creditCard => creditCard.User)
				.HasForeignKey(creditCard => creditCard.UserId);

			builder
				.Entity<AppUser>()
				.HasMany(user => user.Passwords)
				.WithOne(password => password.User)
				.HasForeignKey(password => password.UserId);

			builder
				.Entity<AppUser>()
				.HasMany(user => user.Notebooks)
				.WithOne(notebooks => notebooks.User)
				.HasForeignKey(notebooks => notebooks.UserId);

			builder
				.Entity<AppUser>()
				.HasMany(user => user.Data)
				.WithOne(data => data.User)
				.HasForeignKey(data => data.UserId);
		}

		public DbSet<Data> Data { get; set; }
		public DbSet<Notebook> Notebooks { get; set; }
		public DbSet<Notes> Notes { get; set; }
		public DbSet<UserPassword> UserPasswords { get; set; }
		public DbSet<Audit> Audit { get; set; }
		public DbSet<UserCreditCard> UserCreditCards { get; set; }
	}
}
