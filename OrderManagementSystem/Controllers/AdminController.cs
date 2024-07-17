using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.BL.EntityService.OrderService;
using OrderManagementSystem.BL.EntityService.ProductService;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;
using _Constants = OrderManagementSystem.Constants.Constants;
namespace OrderManagementSystem.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly _Constants _constants;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        public AdminController(AppDBContext context 
            ,ProductService productService
            ,_Constants constants, OrderService orderService)
        {
            _context = context;
            _productService = productService;
            _constants = constants;
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] int status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            var customer = await _context.Customers.FindAsync(order.CustomerId);
            if (customer != null)
            {
                _orderService.SendApprovelEmail(customer, order);
            }

            return Ok(order);
        }

       
        [HttpPost]
        [Route("products")]
        public ActionResult AddProduct(ProductModel product)
        {
           
            try
            {
                var Response = _productService.CreateNewProduct(product);
                string result = _constants.GetResponseGenericSuccess(Response);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }
        }

        [HttpPut("products/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, ProductModel product)
        {
            try
            {
                var Response = _productService.UpdateProduct(product);
                string result = _constants.GetResponseGenericSuccess(Response);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }
        }

        [HttpGet("products/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("orders")]
        public ActionResult GetAllOrders()
        {
            try
            {
                var Response = _orderService.GetAllOrders();
                string result = _constants.GetResponseGenericSuccess(Response);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }


        }

    }
}
