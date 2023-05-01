using OrdersService.Business_layer;
using OrdersService.Models;


namespace OrdersService.Interfaces
{
    public interface IOrderService
    {
        public OrderModel GetOrderData(Guid orderId);
        public OperationResult CreateOrder(NewOrder order);
        public OperationResult UpdateOrderData(Guid orderID, EditOrderModel order);
        public OperationResult DeleteOrder(Guid orderId);
    }
}