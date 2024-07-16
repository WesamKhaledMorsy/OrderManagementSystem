using System.ComponentModel.DataAnnotations.Schema;
using OrderManagementSystem.DL.Entities;

namespace OrderManagementSystem.Models
{
    public class CustomerModel
    {

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Order>? Orders { get; set; }

    }
}
