using Microsoft.AspNetCore.Identity;

namespace CloudDrive.Domain.Entities
{
	public class AppUser : IdentityUser
	{
		public List<Notes> Notes { get; set; }
		public List<UserCreditCard> CreditCards { get; set; }
		public List<Data> Data { get; set; }
		public List<Notebook> Notebooks { get; set; }
		public List<UserPassword> Passwords { get; set; }
	}
}