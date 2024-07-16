using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.BL.EntityService.UserService;
using OrderManagementSystem.DL;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace OrderManagementSystem.BL.EntityService.UserService
{
    public class CreateToken: ICreateToken
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private  readonly Jwt _jwt;
        
        //public CreateToken(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        //{
        //    _userManager = userManager;
        //    _jwt = jwt.Value;
        //}

        //public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        //{
        //    var usercalims = await _userManager.GetClaimsAsync(user);
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var roleClaims = new List<Claim>();
        //    foreach (var role in roles)
        //        roleClaims.Add(new Claim("roles", role));

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        //        new Claim("uid",user.Id)
        //    }
        //    .Union(usercalims)
        //    .Union(roleClaims);

        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        //    var jwtSecurityToken = new JwtSecurityToken(
        //        issuer: _jwt.Issuer,
        //        audience: _jwt.Audience,
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(_jwt.DurationInDays),
        //        signingCredentials: signingCredentials);
        //    return jwtSecurityToken;
        //}
    }
}
