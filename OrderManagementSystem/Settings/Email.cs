using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Settings
{
    public class Email
    {
        [EmailAddress]
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
