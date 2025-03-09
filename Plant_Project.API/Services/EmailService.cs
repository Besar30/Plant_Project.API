using MailKit.Security;
using MimeKit;
using Plant_Project.API.Settings;
using MailKit.Net.Smtp;

namespace Plant_Project.API.Services;

public class EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger) : IEmailSender
{
    private readonly MailSettings _mailSettings = mailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
		try
		{
			var message = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_mailSettings.Mail),
				Subject = subject
			};

			message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
			message.To.Add(MailboxAddress.Parse(email));

			var builder = new BodyBuilder { HtmlBody = htmlMessage };
			message.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();
			smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

			await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
			await smtp.SendAsync(message);
			await smtp.DisconnectAsync(true);

			Console.WriteLine("✅ Success" + email);
		}
		catch (Exception ex)
		{
			Console.WriteLine("❌ Failuer " + ex.Message);
			throw;
		}
	}
}