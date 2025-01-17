﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.ProductService;
using OrderManagementSystem.DL;
using _Constants = OrderManagementSystem.Constants.Constants;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly _Constants _constants;
        private readonly IProductService _productService;

        public ProductController(AppDBContext context, IProductService productService
            , _Constants constants)
        {
            _context = context;
            _productService = productService;
            _constants = constants;
        }

        [AllowAnonymous]
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            string result = _constants.GetResponseGenericSuccess(products);
            return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
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

    }
}
