using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
     
        private readonly _Constants _constants;
       // private readonly AppDBContext _DBContext;
        public InvoiceController(InvoiceService invoiceService,
            //AppDBContext appDBContext ,
            _Constants _Constants)
        {
           // _DBContext = appDBContext;
            _invoiceService = invoiceService;
            _constants = _Constants;
           
            
        }

        [HttpPost("invoice")]
        public ActionResult GenerateInvoiceAsync(InvoiceModel invoice)
        {
            try
            {
                var Response = _invoiceService.GenerateInvoiceAsync(invoice);
                string result = _constants.GetResponseGenericSuccess(Response);                
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }
        }
       
        //[HttpGet("invoices")]
        //public async Task<IActionResult> GetAllInvoices()
        //{
        //    var invoices = await _DBContext.Invoices.ToListAsync();
        //    return Ok(invoices);
        //}

        //[HttpGet("invoices/{invoiceId}")]
        //public async Task<IActionResult> GetInvoiceById(int invoiceId)
        //{
        //    var invoice = await _DBContext.Invoices.FindAsync(invoiceId);
        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(invoice);
        //}



        //[HttpPost("Invoice")]

    }
}
