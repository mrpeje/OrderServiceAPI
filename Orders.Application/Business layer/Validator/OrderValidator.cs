using OrdersService.Interfaces;
using OrdersService.Context;
using OrdersService.Models;
using Orders.Domain;

namespace OrdersService.Business_layer.Validator
{
    public class OrderValidator : IOrderValidator
    {
        public ValidationResult CanDeleteOrder(Order order)
        {
            var result = new ValidationResult();
            if (!(order.Status != OrderStatus.InDelivery.ToString()
                   && order.Status != OrderStatus.Delivered.ToString()
                   && order.Status != OrderStatus.Completed.ToString()))
            {
                result.Validated = ValidationStatus.Invalid;
                result.ErrorMessage = $"Order {order.Id} cannot be deleted due to status";
            }
            return result;
        }

        public ValidationResult CanEditOrderLines(Order orderData)
        {
            var result = new ValidationResult();

            if (!(orderData.Status != OrderStatus.Paid.ToString()
                   && orderData.Status != OrderStatus.InDelivery.ToString()
                   && orderData.Status != OrderStatus.Delivered.ToString()
                   && orderData.Status != OrderStatus.Completed.ToString()))
            {
                result.Validated = ValidationStatus.CannotEditLines;
                result.ErrorMessage = $"Order {orderData.Id} cannot be edited due to status";
            }
            return result;
        }

        public ValidationResult ValidateOrderLines(List<OrderLineModel> lines, Guid orderId)
        {
            var result = new ValidationResult();

            if (lines == null || !lines.Any())
            {
                result.Validated = ValidationStatus.Invalid;
                result.ErrorMessage = $"Order {orderId} must contain lines";
                return result;
            }
            foreach (var line in lines)
            {
                if (line.qty <= 0)
                {
                    result.Validated = ValidationStatus.Invalid;
                    result.ErrorMessage = $"Line {line.Id} quantity should be more then 0";
                    return result;
                }
            }
            result.Validated = ValidationStatus.Valid;
            return result;
        }

    }
    public class ValidationResult
    {
        public ValidationStatus Validated { get; set; }
        public string ErrorMessage { get; set; }
        public ValidationResult()
        {
            Validated = ValidationStatus.Valid;
            ErrorMessage = string.Empty;
        }
    }
    public enum ValidationStatus
    {
        Invalid,
        Valid,
        CannotEditLines
    }

}
