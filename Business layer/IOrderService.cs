using OrdersService.Models;
using System.Net;

namespace OrdersService.Business_layer
{
    public interface IOrderService
    {
        public OrderWithLines GetOrderData(Guid orderId);
        public OrderWithLines CreateOrder(NewOrder order);
        public OrderWithLines UpdateOrderData(OrderWithLines order);
        public HttpStatusCode DeleteOrder(Guid orderId);
    }
}