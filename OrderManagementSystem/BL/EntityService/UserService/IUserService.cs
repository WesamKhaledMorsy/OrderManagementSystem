using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.UserService
{
    public interface IUserService
    {
         Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<string> LoginAsync(LoginModel model);

        Task<IdentityResult> RegisterUserAsync(RegisterModel registerDto);
        Task<string> LoginUserAsync(LoginModel loginDto);
        Task<bool> ForgotPasswordAsync(string email);
        //Task<string> GeneratePasswordResetTokenAsync(string email);
        //Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
    }
}
