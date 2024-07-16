using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.CustomerService
{
    public interface ICustomerService
    {
       Customer CreateNewCustomer(CustomerModel customer);
        IQueryable<Order> GetAllOrdersOfCustomer (int customerId);
    }
}
