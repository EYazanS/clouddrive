using System.Globalization;
using System.Reflection;

using CloudDrive;
using CloudDrive.Api.Middleware;
using CloudDrive.Api.Workers;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using CloudDrive.Services;
using CloudDrive.Services.CreditCards;
using CloudDrive.Services.Emails;
using CloudDrive.Services.Files;
using CloudDrive.Services.Note;
using CloudDrive.Services.Notebooks;
using CloudDrive.Services.UserPasswords;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Quartz;

[assembly: RootNamespace("CloudDrive")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder
	.Services
	.AddScoped<IEmailSender, EmailSender>()
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

builder.Services.AddSingleton<BackgroundWorkService>();

EmailConfig emailConfig = new()
{
	Username = builder.Configuration.GetValue<string>("EmailConfig:Username"),
	Name = builder.Configuration.GetValue<string>("EmailConfig:Name"),
	Email = builder.Configuration.GetValue<string>("EmailConfig:Email"),
	Password = builder.Configuration.GetValue<string>("EmailConfig:Password"),
	Host = builder.Configuration.GetValue<string>("EmailConfig:Host"),
	Port = builder.Configuration.GetValue<int>("EmailConfig:Port"),
};

builder.Services.AddSingleton(emailConfig);

// Add Authentication
builder
	.Services
	.AddIdentity<AppUser, IdentityRole>(options =>
	{
		options.User.RequireUniqueEmail = true;
		options.SignIn.RequireConfirmedAccount = true;
		options.SignIn.RequireConfirmedPhoneNumber = false;
		options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireDigit = false;
		options.Password.RequireLowercase = false;
		options.Password.RequiredUniqueChars = 0;
	})
	.AddDefaultUI()
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<AppDbContext>();

builder.Services
	.AddAuthentication()
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
		googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
	});

builder.Services.AddAuthorization();

// Add Localization

builder
	.Services
	.AddLocalization(options =>
	{
		options.ResourcesPath = "Resources";
	});

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

// Register the worker responsible of seeding the database with the sample clients.
// Note: in a real world application, this step should be part of a setup script.

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddHostedService<TimerWorker>();

builder.Services.AddHostedService<WorkQueueWorker>();

var app = builder.Build();

var supportedCultures = new[] { "ar", "en" };

var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture(supportedCultures[0])
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

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