using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        //public string JWT { get; set; }
    }
}
