using Microsoft.AspNetCore.Mvc;
using OrdersService.Business_layer;
using OrdersService.Context;
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
        public OrderModel GetOrder(Guid id)
        {
            return _orderService.GetOrderData(id);
        }
        [HttpDelete("orders/{id}")]
        public HttpStatusCode DeleteOrderItem(Guid id)
        {
            return _orderService.DeleteOrder(id);
            
        }
        [HttpPut("orders/{id}")]
        public OrderModel UpdateOrder([FromRoute] Guid id, EditOrderModel orderData)
        {
            var editedOrder = _orderService.UpdateOrderData(id, orderData);
            return editedOrder;
        }
        [HttpPost("orders")]
        public OrderModel CreateOrder(NewOrder order)
        {
            var newOrder = _orderService.CreateOrder(order);
            return newOrder;
        }
    }
}
