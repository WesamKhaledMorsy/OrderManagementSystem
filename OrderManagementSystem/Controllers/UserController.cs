using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.CustomerService;
using OrderManagementSystem.BL.EntityService.UserService;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly UserManager<User>  _userManager;

        public UserController(IUserService userService, ICustomerService customerService, UserManager<User> userManager)
        {
            _userService = userService;
            _customerService = customerService;
            _userManager=userManager;
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
                var someUser = await _userManager.FindByNameAsync(customer.Name);
                if (someUser != null)
                {
                    await _userManager.AddToRoleAsync(someUser, "Customer");
                }
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

       
    }
}
