using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        //public string ?Message { get; set; }
        //public int ?MessageCode { get; set; }
        //public string JWT { get; set; }
    }
}
