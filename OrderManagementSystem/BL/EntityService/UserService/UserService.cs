using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.BL.EntityService.CustomerService;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderManagementSystem.BL.EntityService.UserService
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        
        
        public UserService(UserManager<User> userManager, 
                        SignInManager<User> signInManager,
                        IConfiguration configuration
                       )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
           
        }


    
        public async Task<IdentityResult> RegisterUserAsync(RegisterModel registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,                
            };

            return await _userManager.CreateAsync(user, registerDto.Password);
           
        }

        public async Task<string> LoginUserAsync(LoginModel loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:jti"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:iss"],
                    audience: _configuration["Jwt:aud"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,                    
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return null;
        }


    }
}
