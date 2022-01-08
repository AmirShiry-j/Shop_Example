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
        public async Task SendEmail(string UserEmail, string Body, string Subject)
        {
            //enable less secure apps in account google with link
            //https://myaccount.google.com/lesssecureapps
            //https://mail.google.com/mail/u/0/?tab=km#inbox

            //Get Informations from configurations
            string email = _configuration["Email"].ToString();
            string password = _configuration["Password"].ToString();
            int port = Convert.ToInt32(_configuration["Port"]);
            string host = _configuration["Host"].ToString();
            bool enableSsl = Convert.ToBoolean(_configuration["EnableSsl"]);
            int timeout = Convert.ToInt32(_configuration["Timeout"]);


            SmtpClient client = new SmtpClient();
            client.Port = port;
            client.Host = host;
            client.EnableSsl = enableSsl;
            client.Timeout = timeout;
            
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(email, password);


            MailMessage message = new MailMessage(email, UserEmail, Subject, Body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            client.Send(message);
        }
    }
}
