using Microsoft.AspNetCore.Mvc;
using OrdersService.Business_Layer;
using OrdersService.Models;
using System.Net;

namespace OrdersService.Controllers
{
    [ApiController]
    [Route("api/")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders/{id}")]
        public OrderWithLines GetOrder(Guid id)
        {
            var order = _orderService.GetOrderData(id);
            return order;
        }
        [HttpDelete("orders/{id}")]
        public HttpStatusCode DeleteOrderItem(Guid id)
        {
            return _orderService.DeleteOrder(id);
            
        }
        [HttpPut("orders")]
        public OrderWithLines UpdateOrder(OrderWithLines order)
        {
            var editedOrder = _orderService.UpdateOrderData(order);
            return editedOrder;
        }
        [HttpPost("orders")]
        public OrderWithLines CreateOrder(NewOrder order)
        {
            var newOrder = _orderService.CreateOrder(order);
            return newOrder;
        }
    }
}
