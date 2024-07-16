using AutoMapper;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.DL;
using OrderManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementSystem.BL.EntityService.OrderService
{
    public class OrderService: IOrderService
    {
        private readonly IGenericRepository<Order> _IGenericRepository;
        private readonly IUnitOfWork<Order> _IUnitOfWork;      
        private readonly AppDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Order> iGenericRepository, IUnitOfWork<Order> iUnitOfWork,
            AppDBContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _IGenericRepository = iGenericRepository;
            _IUnitOfWork = iUnitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        
        }
        public Order GetOrderById(int orderid)
        {
            return _IGenericRepository.GetById(orderid);
        }
        public  Order CreateNewOrder(OrderModel order)
        {
            var ordermap = _mapper.Map<Order>(order);
            //order.TotalAmount = ApplyDiscount(order.TotalAmount);
           // ordermap.TotalAmount = ApplyDiscount(order.TotalAmount);
           if(order.OrderItems != null)
            {
                bool validOrder = ValidateOrderAsync(order);
                if (validOrder)
                {
                    order.TotalAmount = _dbContext.OrderItems.Where(x=>x.OrderId == order.OrderId)
                                                             .Select(x=>x.UnitPrice*x.Quantity).Sum();
                    var ReqResult = _IGenericRepository.Insert(ordermap);
                    _IUnitOfWork.Save();

                    //var invoice = await _invoiceService.GenerateInvoiceAsync(order);

                    //// Logic to send invoice (e.g., via email) could be implemented here
                    //await SendInvoiceAsync(order.CustomerId, invoice);
                    return ReqResult;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var ReqResult = _IGenericRepository.Insert(ordermap);
                _IUnitOfWork.Save();
                return ReqResult;
            }
        }
        public Order UpdateOrder(OrderModel order)
        {
            var ordermap = GetOrderById((int)order.OrderId);
            if (ordermap is null)
            {
                throw new ArgumentException("Id not found");
            }
            order.OrderId = ordermap.OrderId;
            order.TotalAmount = _dbContext.OrderItems.Where(x => x.OrderId == order.OrderId)
                                                            .Select(x => x.UnitPrice*x.Quantity).Sum();
            ordermap = _mapper.Map<Order>(order);
            var ReqResult = _IGenericRepository.Update(ordermap);
            _IUnitOfWork.Save();
            return ReqResult;
        }

        public IQueryable<Order> GetAllOrders()
        {
            var orders = _dbContext.Orders.Include(o => o.OrderItems);
            return orders;
        }

        public bool ValidateOrderAsync(OrderModel order)
        {
            
            foreach (var item in order.OrderItems)
            {
                var product = _dbContext.Products?.FindAsync(item.ProductId).Result;

                if (product == null || product.Stock < item.Quantity)
                {
                    return false; // Not enough stock
                }
            }
            return true;
        }

        public decimal ApplyDiscount(decimal totalAmount)
        {
            if (totalAmount > 200)
            {
                return totalAmount * 0.90m; // 10% discount
            }
            if (totalAmount > 100)
            {
                return totalAmount * 0.95m; // 5% discount
            }
            return totalAmount;
        }
    }
}
