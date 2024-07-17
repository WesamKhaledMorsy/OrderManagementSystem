using AutoMapper;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.DL;
using OrderManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.BL.EntityService.InvoiceService;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Settings;
using OrderManagementSystem.BL.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting.Server;

namespace OrderManagementSystem.BL.EntityService.OrderService
{
    public class OrderService: IOrderService
    {
        private readonly IGenericRepository<Order> _IGenericRepository;
        private readonly IUnitOfWork<Order> _IUnitOfWork;

        private readonly AppDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;
        public OrderService(IGenericRepository<Order> iGenericRepository, IUnitOfWork<Order> iUnitOfWork,
        AppDBContext dbContext, IMapper mapper, IConfiguration configuration , EmailService emailService)
        {
            _IGenericRepository = iGenericRepository;
            _IUnitOfWork = iUnitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
        
        }
        public Order GetOrderById(int orderid)
        {
            return _IGenericRepository.GetById(orderid);
        }
        public async Task<Order> CreateNewOrder(OrderModel order)
        {
            var ordermap = _mapper.Map<Order>(order);          
          
                bool validOrder = ValidateOrderAsync(order);
                if (validOrder)
                {
                    ordermap.Status =(int)OrderStatus.Ordered;
                    foreach (var item in order.OrderItems)
                    {
                        item.UnitPrice = GetPriceOfProductByIDAsync(item.ProductId);
                        order.TotalAmount += ((item.UnitPrice * item.Quantity) * (1 - (item.Discount / 100m)));
                    }
                    ordermap.TotalAmount = ApplyDiscount( order.TotalAmount);
                    ordermap.OrderItems = order.OrderItems;
                
                    var ReqResult = _IGenericRepository.Insert(ordermap);
                      _IUnitOfWork.Save();
                    if(ordermap.OrderItems != null)
                    {
                        foreach (var item in ordermap.OrderItems)
                        {
                            item.OrderItemId=0;
                            item.OrderId = ReqResult.OrderId;
                            item.UnitPrice =  GetPriceOfProductByIDAsync(item.ProductId);      
                            
                        }
                         var myOrderItems = ReqResult.OrderItems;
                        _dbContext.OrderItems.AddRange(myOrderItems);
                    foreach (var item in myOrderItems)
                    {
                         UpdateProductAsync(item.ProductId, item.Quantity);
                    }
                }
                             
                //_IUnitOfWork.Save();
                //var invoice = _invoiceService.GenerateInvoiceAsync(newInvoice);


                //// Logic to send invoice (e.g., via email) could be implemented here
                //await SendInvoiceAsync(order.CustomerId, invoice);
                return ReqResult;
                }
                else
                {
                    return null;
                }
           
        }
        public  void UpdateProductAsync(int productId, int quantity)
        {
            var product =  _dbContext.Products.Find(productId);
            if (product != null)
            {
                product.Stock -= quantity;
                _dbContext.Products.Update(product);
                 _dbContext.SaveChanges();
            }
        }
        public decimal GetPriceOfProductByIDAsync(int id)
        {
            var product =  _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
            //if (product == null)
            //{
            //    throw new Exception("Id not found");
            //}
            return product.Price;
        }

        public Order UpdateOrder(OrderModel order)
        {
            var ordermap = GetOrderById((int)order.OrderId);
            if (ordermap is null)
            {
                throw new ArgumentException("Id not found");
            }
            order.OrderId = ordermap.OrderId;
            //order.TotalAmount = _dbContext.OrderItems.Where(x => x.OrderId == order.OrderId)
            //                                                .Select(x => x.UnitPrice*x.Quantity).Sum();
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
        public async void SendApprovelEmail(Customer customer,Order order)
        {
            var Status = (OrderStatus)order.Status;
            var emailAddress = new Email()
            {
                To = customer.Email,
                Subject = "Check Your Invoice",
                //Body = $"Hi {customer.Name} 💥🎉, \n\n  Your Invoice ! \n\n Your Order Amount = {order.TotalAmount} EGP !\n\n  " +
                //$"Your  Order Status {Status} \n\nWelcome aboard!\n\nBest regards,\n Wesam Morsy"
                Body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eaeaea; border-radius: 10px;'>
                    <h2 style='color: #333;'>Hi {customer.Name} 💥🎉,</h2>
                    <p style='color: #333;'>Thank you for your order! We're excited to have you with us.</p>
                    <div style='background-color: #f9f9f9; padding: 10px; border-radius: 5px;'>
                        <h3 style='color: #007bff;'>Your Invoice</h3>
                        <p><strong>Order Amount:</strong> {order.TotalAmount} EGP</p>
                        <p><strong>Order Status:</strong> {Status}</p>
                    </div>
                    <p style='color: #333;'>If you have any questions, feel free to contact our support team.</p>
                    <p style='color: #333;'>Welcome aboard!</p>
                    <p style='color: #333;'>Best regards,</p>
                    <p style='color: #333;'><strong>Wesam Morsy</strong></p>
                </div>
            </body>
            </html>
        "
            };
            //string body = this.createEmailBody(customer.Name, emailAddress.Subject , emailAddress.Body);

            var result = _emailService.SendEmail(emailAddress);           

        }


     

        //private string createEmailBody(string userName, string title, string message)

        //{

        //    string body = string.Empty;
        //    //using streamreader for reading my htmltemplate   

        //    using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlTemplate.html")))

        //    {

        //        body = reader.ReadToEnd();

        //    }

        //    body = body.Replace("{UserName}", userName); //replacing the required things  

        //    body = body.Replace("{Title}", title);

        //    body = body.Replace("{message}", message);

        //    return body;

        //}
    }
}
