using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.DL.Entities
{
    public class Order
    {
        public Order() { }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate {  get; set; }
        public decimal TotalAmount { get; set; }
        [ForeignKey("OrderId")]
        public ICollection<OrderItem> OrderItems { get; set; }
        public string  PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
