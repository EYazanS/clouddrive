using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace CloudDrive.Services.Emails
{
	public class EmailSender : IEmailSender
	{
		private readonly EmailConfig _config;

		public EmailSender(EmailConfig config)
		{
			_config = config;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var message = new MimeMessage();

			message.From.Add(new MailboxAddress(_config.Name, _config.Email));

			message.To.Add(new MailboxAddress(email, email));

			message.Subject = subject;

			message.Body = new TextPart("html")
			{
				Text = htmlMessage
			};

			using var client = new SmtpClient();

			client.Connect(_config.Host, _config.Port, true);

			// Note: only needed if the SMTP server requires authentication
			client.Authenticate(_config.Username, _config.Password);

			await client.SendAsync(message);

			client.Disconnect(true);
		}
	}
}