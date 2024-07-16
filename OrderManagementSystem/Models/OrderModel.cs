using System.ComponentModel.DataAnnotations.Schema;
using OrderManagementSystem.DL.Entities;

namespace OrderManagementSystem.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public string PaymentMethod { get; set; }
        public string? Status { get; set; }
    }
}
