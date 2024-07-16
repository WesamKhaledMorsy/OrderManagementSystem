using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.CustomerService;
using OrderManagementSystem.Constants;
using OrderManagementSystem.Models;
using _Constants = OrderManagementSystem.Constants.Constants;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly _Constants _constants;
        public CustomerController(ICustomerService customerService, _Constants IConstants)
        {
            _customerService = customerService;
            _constants = IConstants;
        }

        [HttpPost]
        [Route("customers")]
        public ActionResult CreateCustomer(CustomerModel customer)
        {
            try
            {
                var Response = _customerService.CreateNewCustomer(customer);
                string result = _constants.GetResponseGenericSuccess(Response);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);

            }
            catch (Exception ex)
            {
                string result = _constants.GetResponseError(ex.Message);
                return Content(result, _Constants.ContentTypeJson, System.Text.Encoding.UTF8);
            }
        }

        [HttpGet]
        [Route("customers/orders")]
        public ActionResult GetAllOrdersPerCustomer(int customerId)
        {
            try
            {
                var Response = _customerService.GetAllOrdersOfCustomer(customerId);
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
