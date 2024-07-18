using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using OrderManagementSystem.BL.EntityService.OrderService;
using OrderManagementSystem.BL.EntityService.ProductService;
using OrderManagementSystem.Constants;
using OrderManagementSystem.Controllers;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using _Constants = OrderManagementSystem.Constants.Constants;

namespace UnitTest.Tests.Controller
{
    public class AdminControllerTests
    {
        private readonly AppDBContext _context;
        private readonly Mock<_Constants> _constantsMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly AdminController _adminController;
        private readonly Mock<IMapper >_mapper;
        public AdminControllerTests()
        {
            // Create DbContextOptions for AppDBContext
            var dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "OrderManagementSystem1")
                .Options;

            var dbContextMock = new AppDBContext(dbContextOptions);

            _constantsMock = new Mock<_Constants>();
            _productServiceMock = new Mock<IProductService>();
            _orderServiceMock = new Mock<IOrderService>();
            _mapper =new Mock<IMapper>();
            _adminController = new AdminController(
                _context,
                _productServiceMock.Object,
                _constantsMock.Object,
                _orderServiceMock.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task AdminControllerTests_UpdateOrderStatus_ReturnsOkResult()
        {
            // Arrange
            var orderItems1 = A.Fake<ICollection<OrderItemModel>>();
            {
                new OrderItemModel { OrderItemId = 1, ProductId = 1, Quantity = 1, UnitPrice = 50, Discount = 5 };
            };
            var ORItems = _mapper.Object.Map<List<OrderItem>>(orderItems1);
            int orderId = 1;
            int status = 2;
            var order = new Order { OrderId = orderId, Status = 1, CustomerId = 1, OrderDate=DateTime.UtcNow , PaymentMethod="Cash on deliver" , TotalAmount = 250 , OrderItems = ORItems};
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adminController.UpdateOrderStatus(orderId, status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrder = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(status, returnedOrder.Status);
        }
     

        [Fact]
        public async Task UpdateOrderStatus_NonExistingOrder_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            var newStatus = 2;

            // Act
            var result = await _adminController.UpdateOrderStatus(orderId, newStatus);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateOrderStatus_SendApprovalEmail()
        {
            // Arrange
            var orderId = 1;
            var initialStatus = 1;
            var newStatus = 2;
            var orderItems1 = A.Fake<ICollection<OrderItemModel>>();
            {
                new OrderItemModel { OrderItemId = 1, ProductId = 1, Quantity = 1, UnitPrice = 50, Discount = 5 };
            };
            var ORItems = _mapper.Object.Map<List<OrderItem>>(orderItems1);
            var order = new Order { OrderId = orderId, Status = 1, CustomerId = 1, OrderDate=DateTime.UtcNow, PaymentMethod="Cash on deliver", TotalAmount = 250, OrderItems = ORItems };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var customer = new Customer { CustomerId = 1, /* Add other necessary properties */ };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adminController.UpdateOrderStatus(orderId, newStatus);

            // Assert
            _orderServiceMock.Verify(o => o.SendApprovelEmail(customer, order));
        }

        [Fact]
        public void AdminControllerTests_AddProduct_ReturnsContentResultWithSuccess()
        {
            // Arrange
            var productModel = new ProductModel { ProductId = 1, Name = "Product 1", Price = 100, Stock = 10 };
            var product = _mapper.Object.Map<Product>(productModel);
            _productServiceMock.Setup(s => s.CreateNewProduct(productModel)).Returns(product);           

            // Act
            var result = _adminController.AddProduct(productModel);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
        }

        [Fact]
        public async Task AdminControllerTests_GetProductById_ReturnsOkResult()
        {
            // Arrange
            var productModel = new ProductModel { ProductId = 1, Name = "Product 1", Price = 100, Stock = 10 };
            var product = _mapper.Object.Map<Product>(productModel);
            _productServiceMock.Setup(s => s.CreateNewProduct(productModel)).Returns(product);

            // Act
            var result = await _adminController.GetProductById(product.ProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.ProductId, returnedProduct.ProductId);
        }

        [Fact]
        public void AdminControllerTests_GetAllOrders_ReturnsContentResultWithSuccess()
        {
            // Arrange
            var orderItems1 = A.Fake<ICollection<OrderItemModel>>();
            {
                new OrderItemModel { OrderItemId = 1, ProductId = 1, Quantity = 1, UnitPrice = 50, Discount = 5 };
            };
            var ORItems = _mapper.Object.Map<List<OrderItem>>(orderItems1);
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    TotalAmount = 100,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow,
                    PaymentMethod = "Cash on Delivery",
                    Status = 1,
                    OrderItems =ORItems
                }
            };
            var _orders = _mapper.Object.Map<IQueryable<Order>>(orders);
            _orderServiceMock.Setup(s => s.GetAllOrders()).Returns(_orders);
            _constantsMock.Setup(c => c.GetResponseGenericSuccess(orders)).Returns("Success");

            // Act
            var result = _adminController.GetAllOrders();

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("application/json", contentResult.ContentType);
            Assert.Equal("Success", contentResult.Content);
        }

    }
}
