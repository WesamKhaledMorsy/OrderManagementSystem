using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.EmailService;
using OrderManagementSystem.BL.EntityService.InvoiceService;
using OrderManagementSystem.BL.EntityService.OrderService;
using OrderManagementSystem.BL.Helper;
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
       // private readonly InvoiceService _invoiceService;
        private readonly AppDBContext _context;
        private readonly MailHelper _mailHelper;
        private readonly IMapper _mapper;
        private readonly _Constants _constants;
        public OrderController(OrderService orderService//InvoiceService invoiceService
            , MailHelper mailHelper
            ,IMapper mapper
            , _Constants _Constants)
        {
            _orderService = orderService;
           // _invoiceService = invoiceService;
            _mailHelper = mailHelper;
            _mapper = mapper;
            _constants = _Constants;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel order)
        {
            try
            {
                var Response = _orderService.CreateNewOrder(order);
                string result = _constants.GetResponseGenericSuccess(Response);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }


        }

        [HttpPut]
        public ActionResult Update_Order( OrderModel order)
        {
            try
            {
                var Response = _orderService.UpdateOrder(order);
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
