using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.UserService
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterModel registerDto);
        Task<string> LoginUserAsync(LoginModel loginDto);
   
    }
}
