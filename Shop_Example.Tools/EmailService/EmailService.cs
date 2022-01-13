using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Tools.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string UserEmail, string Body, string Subject);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> SendEmail(string UserEmail, string Body, string Subject)
        {
            //enable less secure apps in account google with link
            //https://myaccount.google.com/lesssecureapps
            //https://mail.google.com/mail/u/0/?tab=km#inbox

            try
            {
                string email = "PazelShop09@gmail.com";
                string password = "---";

                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.UseDefaultCredentials = false;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(email, password);

                MailMessage message = new MailMessage(email, UserEmail, Subject, Body);
                message.IsBodyHtml = true;
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                client.Send(message);

                return true;
            }
            catch (Exception error)
            {
                //Log error
                _logger.LogError(error.ToString());

                return false;
            }
        }
    }
}
