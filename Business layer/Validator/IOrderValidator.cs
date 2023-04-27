using OrdersService.Context;
using OrdersService.Models;

namespace OrdersService.Business_layer.Validator
{
    public interface IOrderValidator 
    {
        public bool CanEditOrder(OrderWithLines orderData);
        public bool CanDeleteOrder(OrderWithLines orderData);
        public bool ValidateOrderLines(List<OrderLineModel> lines);
    }
}
