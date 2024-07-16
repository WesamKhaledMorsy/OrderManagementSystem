using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.DL.Entities
{
    public class Customer
    {
        public Customer() { }
        public int CustomerId {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [ForeignKey("CustomerId")]
        public ICollection<Order>? Orders { get; set; }
        
    }
}
