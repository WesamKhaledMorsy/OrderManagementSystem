using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.OrderItemService
{
    public class OrderItemService : IOrderItemService
    {

        private readonly IGenericRepository<OrderItem> _IGenericRepository;
        private readonly IUnitOfWork<OrderItem> _IUnitOfWork;
        private readonly AppDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OrderItemService(IGenericRepository<OrderItem> iGenericRepository, IUnitOfWork<OrderItem> iUnitOfWork,
            AppDBContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _IGenericRepository = iGenericRepository;
            _IUnitOfWork = iUnitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;

        }
        public OrderItem GetOrderItemById(int orderitemId)
        {
            return _IGenericRepository.GetById(orderitemId);
        }
        public OrderItem CreateNewOrderItem(OrderItemModel orderitem)
        {
            var orderItemmap = _mapper.Map<OrderItem>(orderitem);
            bool validOrder = ValidateOrderItemAsync(orderItemmap);
            if (validOrder)
            {
                var product = _dbContext.Products.Find(orderitem.ProductId);
                orderitem.UnitPrice = product.Price;
                //orderItemmap.Discount = (orderitem.UnitPrice * orderitem.Quantity) - ApplyDiscount(orderitem.UnitPrice * orderitem.Quantity);
                var ReqResult = _IGenericRepository.Insert(orderItemmap);
                _IUnitOfWork.Save();
                UpdateProduct(orderitem.ProductId, orderitem.Quantity);
                return ReqResult;
            }
            else
            {
                return null;
            }

        }
        public void UpdateProduct(int productId, int quantity )
        {
            var product = _dbContext.Products.Find(productId);                       
            product.Stock-= quantity;
            _dbContext.Update(product);
            _dbContext.SaveChanges();
        }
        public OrderItem UpdateOrderItem(OrderItemModel orderItem)
        {
            var ordermap = GetOrderItemById((int)orderItem.OrderId);
            if (ordermap is null)
            {
                throw new ArgumentException("Id not found");
            }
            orderItem.OrderId = ordermap.OrderId;
            ordermap = _mapper.Map<OrderItem>(orderItem);
            var ReqResult = _IGenericRepository.Update(ordermap);
            _IUnitOfWork.Save();
            UpdateProduct(orderItem.ProductId, orderItem.Quantity);
            return ReqResult;
        }

        public IQueryable<OrderItem> GetAllOrderItems()
        {
            var ordersList = _IGenericRepository.GetAll();
            return ordersList;
        }
        public void DeleteOrderItem(int orderId)
        {
            var orderItem = _dbContext.OrderItems.Find(orderId);
            _dbContext.OrderItems.Remove(orderItem);
            _dbContext.SaveChanges();
        }
        public bool ValidateOrderItemAsync(OrderItem orderItem)
        {
            
                var product = _dbContext.Products?.FindAsync(orderItem.ProductId).Result;

                if (product == null || product.Stock < orderItem.Quantity)
                {
                    return false; // Not enough stock
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
