using Microsoft.AspNetCore.Mvc;
using OrdersService.DB_Access;
using OrdersService.Models;
using System.Net;

namespace OrdersService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IDB_Provider _dbProvider;
        public OrdersController(IDB_Provider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [HttpGet("orders/{id}")]
        public Order GetOrder(Guid id)
        {
            var order = _dbProvider.GetOrderById(id);
            return order;
        }
        [HttpDelete("orders/{id}")]
        public HttpStatusCode DeleteOrderItem(Guid id)
        {
            var result = _dbProvider.DeleteOrder(id);
            HttpStatusCode returnValue = HttpStatusCode.NoContent;
            switch (result)
            {
                case OperationStatus.Success:
                    returnValue = HttpStatusCode.OK;
                    break;
                case OperationStatus.NotFound:
                    returnValue = HttpStatusCode.NotFound;
                    break;
                case OperationStatus.Error:
                    returnValue = HttpStatusCode.BadRequest;
                    break;
            }
            return returnValue;
        }
        [HttpPut("order")]
        public Order UpdateOrder(Order order)
        {
            try
            {
                var editedOrder = _dbProvider.UpdateOrder(order);
                return editedOrder;
            }
            catch (Exception e)
            {                
                return null;
            }
        }
        [HttpPost("order")]
        public Order CreateOrder(Order order)
        {
            try
            {
                var newOrder = _dbProvider.CreateOrder(order);
                return newOrder;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
