using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.BL.EntityService.OrderItemService;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
 

            private readonly IOrderItemService _orderItemService;

            public OrderItemController(IOrderItemService orderItemService)
            {
                _orderItemService = orderItemService;
            }

            [HttpGet("{id}")]
            public IActionResult GetOrderItemById(int id)
            {
                var orderItem = _orderItemService.GetOrderItemById(id);
                if (orderItem == null)
                {
                    return NotFound();
                }
                return Ok(orderItem);
            }
            [HttpDelete("{id}")]
            public IActionResult DeleteOrderItem(int orderId)
            {
                 _orderItemService.DeleteOrderItem(orderId);
           
                return Ok("Order Item is Deleted");
            }
        [HttpPost]
            public IActionResult CreateOrderItem([FromBody] OrderItemModel orderItemModel)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdOrderItem = _orderItemService.CreateNewOrderItem(orderItemModel);
                if (createdOrderItem == null)
                {
                    return BadRequest("OrderItem creation failed due to insufficient stock or other validation errors.");
                }

                return CreatedAtAction(nameof(GetOrderItemById), new { id = createdOrderItem.OrderId }, createdOrderItem);
            }

            [HttpPut("{id}")]
            public IActionResult UpdateOrderItem(int id, [FromBody] OrderItemModel orderItemModel)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var updatedOrderItem = _orderItemService.UpdateOrderItem(orderItemModel);
                    return Ok(updatedOrderItem);
                }
                catch (ArgumentException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            [HttpGet]
            public IActionResult GetAllOrderItems()
            {
                var orderItems = _orderItemService.GetAllOrderItems();
                return Ok(orderItems);
            }
        
    }
}
