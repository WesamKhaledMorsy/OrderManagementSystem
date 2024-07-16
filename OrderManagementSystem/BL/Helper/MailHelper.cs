using System.Net;
using System.Net.Mail;
using System.Text;
using OrderManagementSystem.Models;
namespace OrderManagementSystem.BL.Helper
{
    public class MailHelper
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public MailHelper(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }
        public string SendMail(string fromEmail, string toEmail, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }

                return "Email sent successfully.";
            }
            catch (Exception ex)
            {
                return $"Failed to send email. Error: {ex.Message}";
            }
        }
      
       



        


    }



}
