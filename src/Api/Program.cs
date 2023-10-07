using CloudDrive.Persistence;
using Microsoft.EntityFrameworkCore;

using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

	options.UseOpenIddict();
});

builder
	.Services
	.AddAuthentication();

builder
	.Services
	.AddOpenIddict()
	// Register the OpenIddict core components.
	.AddCore(options =>
	{
		// Configure OpenIddict to use the Entity Framework Core stores and models.
		// Note: call ReplaceDefaultEntities() to replace the default entities.
		options
			.UseEntityFrameworkCore()
			.UseDbContext<AppDbContext>();
	})

	// Register the OpenIddict server components.
	.AddServer(options =>
	{
		// Enable the token endpoint.
		options.SetTokenEndpointUris("connect/token");

		// Enable the client credentials flow.
		options.AllowClientCredentialsFlow();

		// Register the signing and encryption credentials.
		options
			.AddDevelopmentEncryptionCertificate()
			.AddDevelopmentSigningCertificate();

		// Register the ASP.NET Core host and configure the ASP.NET Core options.
		options
			.UseAspNetCore()
			.EnableTokenEndpointPassthrough();
	})

	// Register the OpenIddict validation components.
	.AddValidation(options =>
	{
		// Import the configuration from the local OpenIddict server instance.
		options.UseLocalServer();

		// Register the ASP.NET Core host.
		options.UseAspNetCore();
	});

// Register the worker responsible of seeding the database with the sample clients.
// Note: in a real world application, this step should be part of a setup script.

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
