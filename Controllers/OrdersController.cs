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
        public ActionResult<OrderModel> GetOrder(Guid id)
        {
            var order = _orderService.GetOrderData(id);
            if(order == null)
                return StatusCode(404, "Not found");
            return order;
        }
        [HttpDelete("orders/{id}")]
        public ActionResult DeleteOrderItem(Guid id)
        {
            var operationResult = _orderService.DeleteOrder(id);
            return StatusCode((int)operationResult.Status, operationResult.ErrorMessage);          
        }
        [HttpPut("orders/{id}")]
        public ActionResult<OrderModel> UpdateOrder([FromRoute] Guid id, EditOrderModel orderData)
        {
            var operationResult = _orderService.UpdateOrderData(id, orderData);
            if(operationResult.Status == OperationStatus.Success)
            {
                return _orderService.GetOrderData(id);
            }
            else
            {
                return StatusCode((int)operationResult.Status, operationResult.ErrorMessage);
            }           
        }
        [HttpPost("orders")]
        public ActionResult<OrderModel> CreateOrder(NewOrder order)
        {
            var operationResult = _orderService.CreateOrder(order);

            if (operationResult.Status == OperationStatus.Success)
            {
                return _orderService.GetOrderData(order.Id);
            }
            else
            {
                return StatusCode((int)operationResult.Status, operationResult.ErrorMessage);
            }
        }
    }
}
