using OrdersService.Business_layer.Validator;
using OrdersService.Context;

namespace OrdersService.Interfaces
{
    public interface IOrderValidator 
    {
        public ValidatorResult CanEditOrderLines(Order orderData);
        public ValidatorResult CanDeleteOrder(Order orderData);
        public ValidatorResult ValidateOrderLines(List<OrderLineModel> lines, Guid orderId);
        public ValidatorResult isEdited(List<OrderLine> userLines, List<OrderLine> dbLines);
    }
}
