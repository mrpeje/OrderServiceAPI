using OrdersService.Business_layer.Validator;
using OrdersService.Models;
using System.Net;

namespace OrdersService.Business_layer
{
    public interface IOrderService
    {
        public OrderModel GetOrderData(Guid orderId);
        public OperationResult CreateOrder(NewOrder order);
        public OperationResult UpdateOrderData(Guid orderID, EditOrderModel order);
        public OperationResult DeleteOrder(Guid orderId);
    }
}