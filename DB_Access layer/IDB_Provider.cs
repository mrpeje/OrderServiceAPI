using OrdersService.Context;
using OrdersService.Models;

namespace OrdersService.DB_Access
{
    public interface IDB_Provider
    {
        public OperationStatus CreateOrder(Order order);
        public OperationStatus UpdateOrder(Order order);
        public OperationStatus DeleteOrder(Guid id);
        public Order GetOrderById(Guid id);
    }
}
