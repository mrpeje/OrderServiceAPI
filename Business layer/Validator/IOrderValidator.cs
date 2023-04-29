using OrdersService.Context;
using OrdersService.Models;

namespace OrdersService.Business_layer.Validator
{
    public interface IOrderValidator 
    {
        public bool CanEditOrder(Order orderData);
        public bool CanDeleteOrder(Order orderData);
        public bool ValidateOrderLines(List<OrderLineModel> lines);
    }
}
