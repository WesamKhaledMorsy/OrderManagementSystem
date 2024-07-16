using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
