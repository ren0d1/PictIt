namespace PictIt.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSender : IEmailSender
    {
        private readonly SendGridClientOptions _options;

        public EmailSender(IOptions<SendGridClientOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(_options.ApiKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
                          {
                              From = new EmailAddress("noreply@pictit.be", "PictIt automatic mailer"),
                              Subject = subject,
                              PlainTextContent = message,
                              HtmlContent = message
                          };
            msg.AddTo(new EmailAddress(email));

            msg.TrackingSettings = new TrackingSettings
                                       {
                                           ClickTracking = new ClickTracking { Enable = true }
                                       };

            return client.SendEmailAsync(msg);
        }
    }
}
