using CloudDrive.Api.Middleware;
using CloudDrive.Api.Workers;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using CloudDrive.Services;
using CloudDrive.Services.Files;
using CloudDrive.Services.UserPasswords;
using CloudDrive.Services.Notebooks;
using CloudDrive.Services.Note;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;
using CloudDrive.Services.CreditCards;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Reflection;
using CloudDrive;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

	options.UseOpenIddict();
});

builder
	.Services
	.AddScoped<IFilesService, FilesService>()
	.AddScoped<INotesService, NotesService>()
	.AddScoped<ICreditCardsServices, UserCreditCards>()
	.AddScoped<IUserPasswordsService, UserPasswordsService>()
	.AddScoped<INotesService, NotesService>();

builder
	.Services
	.AddScoped<INotebooksService, NotebooksService>();

builder.Services.AddSingleton(new FileConfigurations()
{
	FileSavePath = builder.Configuration["FileSavePath"]
});

builder
	.Services
	.AddLocalization(options =>
	{
		options.ResourcesPath = "Resources";
	});

builder.Services.AddSingleton<BackgroundWorkService>();

builder
	.Services
	.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

builder
	.Services
	.Configure<RequestLocalizationOptions>(options =>
	{
		CultureInfo[] supportedCultures = new[]
		{
			new CultureInfo("ar"),
			new CultureInfo("en")
		};

		options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "ar");
		options.SupportedCultures = supportedCultures;
		options.RequestCultureProviders = new List<IRequestCultureProvider>
		{
			new CookieRequestCultureProvider()
		};
	});

builder
	.Services
	.AddControllersWithViews()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
	.AddDataAnnotationsLocalization(options =>
	{
		options.DataAnnotationLocalizerProvider = (type, factory) =>
		{
			var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
			return factory.Create("Resource", assemblyName.Name);
		};
	});


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
	.Services
	.AddOpenIddict()

	// Register the OpenIddict core components.
	.AddCore(options =>
	{
		// Configure OpenIddict to use the Entity Framework Core stores and models.
		// Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
		options
			.UseEntityFrameworkCore()
			.UseDbContext<AppDbContext>();

		// Enable Quartz.NET integration.
		options.UseQuartz();
	})

	// Register the OpenIddict server components.
	.AddServer(options =>
	{
		// Enable the authorization, logout, token and userinfo endpoints.
		options
			.SetAuthorizationEndpointUris("/connect/authorize")
			.SetLogoutEndpointUris("/connect/logout")
			.SetTokenEndpointUris("/connect/token")
			.SetUserinfoEndpointUris("/connect/userinfo");

		// Mark the "email", "profile" and "roles" scopes as supported scopes.
		options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

		options
			.AllowPasswordFlow()
			.AllowAuthorizationCodeFlow()
			.AllowRefreshTokenFlow();

		options
			.AddDevelopmentEncryptionCertificate()
			.AddDevelopmentSigningCertificate();

		// Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
		options
			.UseAspNetCore()
			.EnableAuthorizationEndpointPassthrough()
			.EnableLogoutEndpointPassthrough()
			.EnableStatusCodePagesIntegration()
			.EnableTokenEndpointPassthrough();
	})

	// Register the OpenIddict validation components.
	.AddValidation(options =>
	{
		// Import the configuration from the local OpenIddict server instance.
		options.UseLocalServer();

		// Register the ASP.NET Core host.
		options.UseAspNetCore();

		options.UseDataProtection();

		// For applications that need immediate access token or authorization
		// revocation, the database entry of the received tokens and their
		// associated authorizations can be validated for each API call.
		// Enabling these options may have a negative impact on performance.
		//
		// options.EnableAuthorizationEntryValidation();
		// options.EnableTokenEntryValidation();
	});

// Register the worker responsible of seeding the database with the sample clients.
// Note: in a real world application, this step should be part of a setup script.

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddHostedService<InitWorker>();

builder.Services.AddHostedService<TimerWorker>();

builder.Services.AddHostedService<WorkQueueWorker>();

var app = builder.Build();

var supportedCultures = new[] { "ar", "en" };

var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture(supportedCultures[0])
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UsePerformance();

app.UseErrorHandeling();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.MapGet("/api/notes", async (INotesService service) =>
{
	return await service.Get();
});

app.Run();
