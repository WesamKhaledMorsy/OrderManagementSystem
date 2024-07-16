using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.CustomerService;
using OrderManagementSystem.BL.EntityService.UserService;
using OrderManagementSystem.DL;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;

        public UserController(IUserService userService, ICustomerService customerService)
        {
            _userService = userService;
            _customerService = customerService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUserAsync(registerDto);

            if (result.Succeeded)
            {
                var customerMap = new CustomerModel { Email=registerDto.Email, Name = registerDto.UserName };
                var customer = _customerService.CreateNewCustomer(customerMap);
                return Ok(new { Message = "User registered successfully" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginDto)
        {
            var token = await _userService.LoginUserAsync(loginDto);

            if (token == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            return Ok(new { Token = token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordDto)
        {
            var result = await _userService.ForgotPasswordAsync(forgotPasswordDto.Email);

            if (!result)
            {
                return BadRequest(new { Message = "Email not found" });
            }

            return Ok(new { Message = "Password reset email sent" });
        }
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    if (ModelState.IsValid) {
        //        //var user = new ApplicationUser
        //        //{
        //        //    Name = model.UserName,
        //        //    UserName = model.UserName,
        //        //    Email = model.Email
        //        //};
        //        var result = await _userService.RegisterAsync(model);
        //        if (!result.Succeeded)
        //        {
        //            return  BadRequest(result.Errors); //Unauthorized()
        //        }
        //        return Ok("User registered successfully.");
        //    }
        //    return BadRequest();

        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var token = await _userService.LoginAsync(model);
        //    if (token == null)
        //    {
        //        return Unauthorized("Invalid credentials.");
        //    }

        //    return Ok(new { Token = token });
        //}

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        //{
        //    var token = await _userService.GeneratePasswordResetTokenAsync(model.Email);
        //    if (token == null)
        //    {
        //        return BadRequest("Invalid Email.");
        //    }

        //    // Logic to send the token to user's email
        //    // e.g., await _emailService.SendPasswordResetEmailAsync(model.Email, token);

        //    return Ok("Password reset token sent to email.");
        //}

        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        //{
        //    var result = await _userService.ResetPasswordAsync(model);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(result.Errors);
        //    }

        //    return Ok("Password reset successfully.");
        //}
    }
}
