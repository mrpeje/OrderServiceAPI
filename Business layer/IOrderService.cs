using OrdersService.Context;
using OrdersService.Models;
using System.Net;

namespace OrdersService.Business_layer
{
    public interface IOrderService
    {
        public OrderModel GetOrderData(Guid orderId);
        public OrderModel CreateOrder(NewOrder order);
        public OrderModel UpdateOrderData(OrderModel order);
        public HttpStatusCode DeleteOrder(Guid orderId);
    }
}