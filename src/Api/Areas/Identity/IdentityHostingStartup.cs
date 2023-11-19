using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CloudDrive.Areas.Identity.IdentityHostingStartup))]
namespace CloudDrive.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
			});
		}
	}
}