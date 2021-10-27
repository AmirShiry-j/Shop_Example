using Microsoft.Extensions.Configuration;
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
        Task SendEmail(string UserEmail, string Body, string Subject);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmail(string UserEmail, string Body, string Subject)
        {
            //enable less secure apps in account google with link
            //https://myaccount.google.com/lesssecureapps


            //https://mail.google.com/mail/u/0/?tab=km#inbox


            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 1000000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            //Use Secrets Manager for Values
            string emailOrigin = _configuration["Email"].ToString();
            string password = _configuration["Password"].ToString();

            client.Credentials = new NetworkCredential(emailOrigin, password);
            MailMessage message = new MailMessage(emailOrigin, UserEmail, Subject, Body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            client.Send(message);

            return Task.CompletedTask;
        }
    }
}
