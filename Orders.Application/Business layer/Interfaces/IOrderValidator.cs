using OrdersService.Business_layer.Validator;
using OrdersService.Context;

namespace OrdersService.Interfaces
{
    public interface IOrderValidator 
    {
        public ValidationResult CanEditOrderLines(Order orderData);
        public ValidationResult CanDeleteOrder(Order orderData);
        public ValidationResult ValidateOrderLines(List<OrderLineModel> lines, Guid orderId);
    }
}
