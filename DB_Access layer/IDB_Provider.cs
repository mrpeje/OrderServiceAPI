using OrdersService.Context;
using OrdersService.Models;

namespace OrdersService.DB_Access
{
    public interface IDB_Provider
    {
        public OperationStatus CreateOrder(Order order);
        public OperationStatus UpdateOrder(Order order);
        public OperationStatus UpdateOrderLine(OrderLine orderLine);
        public OperationStatus DeleteOrder(Guid id);
        public Order GetOrderById(Guid id);
        public OperationStatus CreateOrderLine(OrderLine orderLine);
        public OperationStatus DeleteOrderLine(Guid id);
        public List<OrderLine> GetOrderLinesByOrderId(Guid id);

    }
}
