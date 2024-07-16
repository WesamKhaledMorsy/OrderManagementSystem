namespace OrderManagementSystem.BL.EntityService.EmailService
{
    public class EmailService
    {

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Implementation to send email (e.g., using SMTP or a third-party service)
            await Task.CompletedTask;
        }

        public async Task SendOrderStatusChangeEmailAsync(string toEmail, string status)
        {
            var subject = "Order Status Update";
            var body = $"Your order status has been updated to: {status}";
            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
