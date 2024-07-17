﻿using Microsoft.Extensions.Options;
using OrderManagementSystem.Settings;
//using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;


namespace OrderManagementSystem.BL.Helper
{
        public interface IEmailService
        {
            bool SendEmail(Email email);
        }
        public class EmailService : IEmailService
        {
            private MailSettings _options;
            public EmailService(IOptions<MailSettings> options)
            {
                _options = options.Value;
            }
            public bool SendEmail(Email email)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

                    message.To.Add(MailboxAddress.Parse(email.To));
                    message.Subject = email.Subject;
                    var builder = new BodyBuilder();
                    builder.TextBody = email.Body;
                    message.Body =new TextPart(TextFormat.Html)
                    {
                        Text =email.Body
                    };
                  // builder.ToMessageBody();

                using (var client = new SmtpClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Connect(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);

                        client.Authenticate(_options.Email, _options.Password);

                        client.Send(message);
                        client.Disconnect(true);
                    }
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }

            }
        }
  
}
