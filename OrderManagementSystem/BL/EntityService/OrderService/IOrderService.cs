using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.OrderService
{
    public interface IOrderService
    {
         Task<Order> CreateNewOrder(OrderModel order);
        Order UpdateOrder(OrderModel order);
        Order UpdateOrderStatus (int orderId, int status);
        IQueryable<Order> GetAllOrders();
        Order GetOrderById(int orderId);
        bool ValidateOrderAsync(OrderModel order);

        void SendApprovelEmail(Customer customer, Order order);

    }
}
