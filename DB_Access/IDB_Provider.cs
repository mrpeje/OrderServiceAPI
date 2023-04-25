using OrdersService.Models;

namespace OrdersService.DB_Access
{
    public interface IDB_Provider
    {
        public Order CreateOrder(Order order);
        public Order UpdateOrder(Order order);
        public OperationStatus DeleteOrder(Guid id);
        public Order GetOrderById(Guid id);

    }
}
