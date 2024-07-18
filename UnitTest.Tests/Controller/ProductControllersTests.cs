using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderManagementSystem.BL.EntityService.ProductService;
using OrderManagementSystem.Controllers;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Constants = OrderManagementSystem.Constants.Constants;


namespace UnitTest.Tests.Controller
{
    public class ProductControllersTests
    {      
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<_Constants> _constantsMock;
        private readonly ProductController _productController;

        public ProductControllersTests()
        {
            // Create DbContextOptions for AppDBContext
            var dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "OrderManagementSystem1")
                .Options;

            // Create an instance of AppDBContext using the options
            var dbContextMock = new AppDBContext(dbContextOptions);

            _productServiceMock = new Mock<IProductService>();
            _constantsMock = new Mock<_Constants>();

            _productController = new ProductController(
                dbContextMock,
                _productServiceMock.Object,
                _constantsMock.Object
            );
        }
        [Fact]
        public async Task GetAllProducts_ReturnsContentResultWithSuccess()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new Product { ProductId = 2, Name = "Product 2", Price = 200, Stock = 20 }
            };

            _productServiceMock.Setup(s => s.GetAllProducts()).Returns(products.AsQueryable());

            // Act
            var result = await _productController.GetAllProducts();

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);            
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResultWithProduct()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Product 1", Price = 100, Stock = 10 };
            var dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "OrderManagementSystem1")
                .Options;

            using (var context = new AppDBContext(dbContextOptions))
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            // Act
            IActionResult result;
            using (var context = new AppDBContext(dbContextOptions))
            {
                var controller = new ProductController(context,_productServiceMock.Object, _constantsMock.Object);
                result = await controller.GetProductById(product.ProductId);
            }

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.ProductId, returnedProduct.ProductId);
            Assert.Equal(product.Name, returnedProduct.Name);
            Assert.Equal(product.Price, returnedProduct.Price);
            Assert.Equal(product.Stock, returnedProduct.Stock);
        }

        [Fact]
        public async Task GetProductById_ReturnsNotFoundResult()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Product 1", Price = 100, Stock = 10 };
            var dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "OrderManagementSystem1")
                .Options;

            using (var context = new AppDBContext(dbContextOptions))
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
            // Act
            IActionResult result;
            using (var context = new AppDBContext(dbContextOptions))
            {
                var controller = new ProductController(context,_productServiceMock.Object, _constantsMock.Object);
                result = await controller.GetProductById(2); // Non-existing product ID
            }

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
