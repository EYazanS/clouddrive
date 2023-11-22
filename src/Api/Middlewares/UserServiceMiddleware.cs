using System.Diagnostics;
using System.Security.Claims;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using CloudDrive.Services;
using Microsoft.AspNetCore.Identity;

namespace CloudDrive.Api.Middleware
{
	public class UserServiceMiddleware
	{
		private readonly ILogger<PerformanceMiddleware> _logger;
		private readonly RequestDelegate _next;

		public UserServiceMiddleware(
			ILogger<PerformanceMiddleware> logger,
			RequestDelegate next
		)
		{
			_logger = logger;

			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, UserManager<AppUser> userManager, UserService userService)
		{
			userService.UserId = userManager.GetUserId(context.User);

			_logger.LogInformation("Request time for endpoint: '{endpoint}', with user: {userId}", context.Request.Path, userService.UserId);

			await _next(context);
		}
	}
}