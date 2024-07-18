using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.BL.EntityService.CustomerService;
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
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly _Constants _constants;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        public AdminController(AppDBContext context 
            ,IProductService productService
            ,_Constants constants, IOrderService orderService 
            ,IMapper mapper
            , ICustomerService customerService)
        {
            _context = context;
            _productService = productService;
            _constants = constants;
            _orderService = orderService;
            _mapper = mapper;
            _customerService = customerService;
        }

        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] int status)
        {
            try
            {
                var Response = _orderService.UpdateOrderStatus(orderId, status);
                string result = _constants.GetResponseGenericSuccess(Response);
                //var order = _orderService.GetOrderById(orderId);
               var customer =_customerService.GetCustomer(Response.CustomerId);
                if (customer != null)
                {
                    _orderService.SendApprovelEmail(customer, Response);
                }
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }          
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
            //var product = await _context.Products.FindAsync(productId);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            //return Ok(product);
            try
            {
                var Response = _orderService.GetOrderById(productId);
                string result = _constants.GetResponseGenericSuccess(Response);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }
        }

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
