using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace MusicStore.Services.Implementation
{
	public class EmailService : IEmailService
	{
		private readonly IOptions<AppSettings> _options;
		private readonly ILogger<EmailService> _logger;

		public EmailService(IOptions<AppSettings> options, ILogger<EmailService> logger)
		{
			_options = options;
			_logger = logger;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			try
			{
				var smtp = _options.Value.SmtpConfiguration;
				var mailMessage = new MailMessage(
					new MailAddress(smtp.UserName, smtp.FromName),
					new MailAddress(email));
				
				mailMessage.Subject = subject;
				mailMessage.Body = message;
				mailMessage.IsBodyHtml = true;

				using var smtpClient = new SmtpClient(smtp.Server, smtp.PortNumber)
				{
					Credentials = new NetworkCredential(smtp.UserName, smtp.Password),
					EnableSsl = smtp.EnableSsl
				};

				await smtpClient.SendMailAsync(mailMessage);

				_logger.LogInformation("Se envió correctamente el correo a {email}", email);
			}
			catch (SmtpException ex)
			{
				_logger.LogWarning(ex, "No se puede enviar el correo {message}", ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error al enviar el correo a {email} {message}", email, ex.Message);
			}
		}
	}
}
