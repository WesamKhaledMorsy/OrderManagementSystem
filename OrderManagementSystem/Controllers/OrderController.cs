using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.InvoiceService;
using OrderManagementSystem.BL.EntityService.OrderService;
using OrderManagementSystem.BL.Helper;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;
using _Constants = OrderManagementSystem.Constants.Constants;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly _Constants _constants;
        private readonly IUnitOfWork<Invoice> _IUnitOfWorkInvoice;
        private readonly IGenericRepository<Invoice> _IGenericRepositoryInvoice;
        public OrderController(OrderService orderService,IUnitOfWork<Invoice> IUnitOfWorkInvoice,
             IGenericRepository<Invoice> iGenericRepositoryInvoice
            ,IMapper mapper,
             AppDBContext context
            , _Constants _Constants)
        {
            _context = context;
            _orderService = orderService;
            _mapper = mapper;
            _constants = _Constants;
            _IUnitOfWorkInvoice = IUnitOfWorkInvoice;
            _IGenericRepositoryInvoice = iGenericRepositoryInvoice;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel order)
        {
            try
            {
                var Response = _orderService.CreateNewOrder(order);
                string result = _constants.GetResponseGenericSuccess(Response);
                var newInvoice = new InvoiceModel()
                {
                    TotalAmount = Response.Result.TotalAmount,
                    OrderId = Response.Result.OrderId,    
                    InvoiceDate = DateTime.Now
                };
                var invoice = _mapper.Map<Invoice>(newInvoice);
                _context.Invoices.Add(invoice);
                _context.SaveChanges();
                var customer = _context.Customers.Where(x => x.CustomerId == order.CustomerId).FirstOrDefault();
                _orderService.SendApprovelEmail(customer , Response.Result);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }


        }


        [HttpGet]
        [Route("orders/{orderId}")]
        public ActionResult GetOrderById(int orderId)
        {
            try
            {
                var Response = _orderService.GetOrderById(orderId);
                string result = _constants.GetResponseGenericSuccess(Response);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }
        }
    }
}
