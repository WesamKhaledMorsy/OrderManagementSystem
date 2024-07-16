using OrderManagementSystem.BL.EntityService.InvoiceService;
using OrderManagementSystem.DL.Entities;

namespace OrderManagementSystem.BL.EntityService.OrderService
{
    //public class OrderServiceTests
    //{
    //    private readonly OrderService _orderService;
    //    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    //    private readonly Mock<IProductRepository> _productRepositoryMock;
    //    private readonly Mock<IInvoiceService> _invoiceServiceMock;

    //    public OrderServiceTests()
    //    {
    //        _orderRepositoryMock = new Mock<IOrderRepository>();
    //        _productRepositoryMock = new Mock<IProductRepository>();
    //        _invoiceServiceMock = new Mock<IInvoiceService>();

    //        _orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object, _invoiceServiceMock.Object);
    //    }

    //    [Fact]
    //    public async Task PlaceOrderAsync_InsufficientStock_ThrowsException()
    //    {
    //        // Arrange
    //        var order = new Order
    //        {
    //            OrderItems = new List<OrderItem> {
    //            new OrderItem { ProductId = 1, Quantity = 10, UnitPrice = 100 }
    //        }
    //        };
    //        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, Stock = 5 });

    //        // Act & Assert
    //        await Assert.ThrowsAsync<Exception>(() => _orderService.PlaceOrderAsync(order));
    //    }

    //    // Other unit tests
    //}

}
