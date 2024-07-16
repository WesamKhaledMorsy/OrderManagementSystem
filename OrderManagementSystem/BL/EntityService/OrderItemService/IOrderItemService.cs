using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.OrderItemService
{
    public interface IOrderItemService
    {
        OrderItem CreateNewOrderItem(OrderItemModel orderitem);
        OrderItem UpdateOrderItem(OrderItemModel orderItem);
        IQueryable<OrderItem> GetAllOrderItems();
        OrderItem GetOrderItemById(int orderId);
        void DeleteOrderItem(int orderId);
        bool ValidateOrderItemAsync(OrderItem orderItem);
    }
}
