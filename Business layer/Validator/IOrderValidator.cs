using OrdersService.Context;

namespace OrdersService.Business_layer.Validator
{
    public interface IOrderValidator 
    {
        public ValidatorResult CanEditOrderLines(Order orderData);
        public ValidatorResult CanDeleteOrder(Order orderData);
        public ValidatorResult ValidateOrderLines(List<OrderLineModel> lines, Guid orderId);
        public ValidatorResult isEdited(List<OrderLine> userLines, List<OrderLine> dbLines);
    }
}
