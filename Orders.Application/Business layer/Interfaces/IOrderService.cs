using OrdersService.Business_layer;
using OrdersService.Models;


namespace OrdersService.Interfaces
{
    public interface IOrderService
    {
        public OrderModel GetOrderModel(Guid orderId);
        public OperationResult CreateOrder(NewOrderModel order);
        public OperationResult UpdateOrder(Guid orderID, EditOrderModel order);
        public OperationResult DeleteOrder(Guid orderId);
    }
}