using CloudDrive.Api.Workers;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

	options.UseOpenIddict();
});

builder
	.Services
	.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

// Register the Identity services.
builder
	.Services
	.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
builder
	.Services
	.AddQuartz(options =>
	{
		options.UseMicrosoftDependencyInjectionJobFactory();
		options.UseSimpleTypeLoader();
		options.UseInMemoryStore();
	});

builder
	.Services.AddOpenIddict()

			// Register the OpenIddict core components.
			.AddCore(options =>
			{
				// Configure OpenIddict to use the Entity Framework Core stores and models.
				// Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
				options.UseEntityFrameworkCore()
					   .UseDbContext<AppDbContext>();

				// Enable Quartz.NET integration.
				options.UseQuartz();
			})

			// Register the OpenIddict server components.
			.AddServer(options =>
			{
				// Enable the token endpoint.
				options.SetTokenEndpointUris("connect/token");

				// Enable the client credentials flow.
				options.AllowClientCredentialsFlow();

				// Register the signing and encryption credentials.
				options.AddDevelopmentEncryptionCertificate()
					   .AddDevelopmentSigningCertificate();

				// Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
				options.UseAspNetCore()
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

builder.Services.AddHostedService<InitWorker>();

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
