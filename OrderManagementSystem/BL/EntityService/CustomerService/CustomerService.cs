using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.CustomerService
{
    public class CustomerService : ICustomerService
    {

        private readonly IGenericRepository<Customer> _IGenericRepository;
        private readonly IUnitOfWork<Customer> _IUnitOfWork;
        private readonly IGenericRepository<Order> _IGenericRepositoryOrder;
        private readonly IUnitOfWork<Order> _IUnitOfWorkOrder;
        private readonly AppDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CustomerService(IGenericRepository<Customer> iGenericRepository, IUnitOfWork<Customer> iUnitOfWork,
            IGenericRepository<Order> genericRepositoryOrder ,IUnitOfWork<Order> unitOfWorkOrder,
            AppDBContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _IGenericRepository = iGenericRepository;
            _IUnitOfWork = iUnitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _IGenericRepositoryOrder = genericRepositoryOrder;
            _IUnitOfWorkOrder = unitOfWorkOrder;    
        }
        public Customer CreateNewCustomer(CustomerModel customer)
        {            
            var customermap = _mapper.Map<Customer>(customer);
            var ReqResult = _IGenericRepository.Insert(customermap);
            _IUnitOfWork.Save();
            return ReqResult;            
        }

        public IQueryable<Order> GetAllOrdersOfCustomer(int customerId)
        {
            var orderList = _dbContext.Orders.Where(x => x.CustomerId == customerId);   
            orderList .Select(x=>x.OrderItems).ToList();    
            return orderList;
        }
    }
}
