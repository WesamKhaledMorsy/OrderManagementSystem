
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.InvoiceService;
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
        public InvoiceController(InvoiceService invoiceService,
            _Constants _Constants)
        {
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
        [HttpGet("invoices/{invoiceId}")]
        public async Task<IActionResult> GetInvoiceById(int invoiceId)
        {
            try
            {
                var Response = _invoiceService.GetInvoiceByID(invoiceId);
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
