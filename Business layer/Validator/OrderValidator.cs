﻿using OrdersService.Context;
using OrdersService.Models;

namespace OrdersService.Business_layer.Validator
{
    public class OrderValidator : IOrderValidator
    {
        public ValidatorResult CanDeleteOrder(Order order)
        {
            var result = new ValidatorResult();

            result.Validated = order.Status != OrderStatus.InDelivery.ToString()
                   && order.Status != OrderStatus.Delivered.ToString()
                   && order.Status != OrderStatus.Completed.ToString();
            if(!result.Validated)
            {
                result.ErrorMessage = $"Order {order.Id} cannot be deleted due to status";
            }
            return result;
        }

        public ValidatorResult CanEditOrderLines(Order orderData)
        {
            var result = new ValidatorResult();

            result.Validated = orderData.Status != OrderStatus.Paid.ToString()
                   && orderData.Status != OrderStatus.InDelivery.ToString()
                   && orderData.Status != OrderStatus.Delivered.ToString()
                   && orderData.Status != OrderStatus.Completed.ToString();
            
            if (!result.Validated)
            {
                result.ErrorMessage = $"Order {orderData.Id} cannot be edited due to status";
            }
            return result;
        }

        public ValidatorResult isEdited(List<OrderLine> requestLines, List<OrderLine> dbLines)
        {
            var result = new ValidatorResult();
            result.Validated = !requestLines.OrderBy(e=>e.Id).SequenceEqual(dbLines.OrderBy(e => e.Id));
            return result;
        }

        public ValidatorResult ValidateOrderLines(List<OrderLineModel> lines, Guid orderId)
        {
            var result = new ValidatorResult();

            if (lines == null || lines.Count == 0)
            {
                result.Validated = false;
                result.ErrorMessage = $"Order {orderId} must contain lines";
                return result;
            }
            foreach (var line in lines)
            {
                if (line.qty <= 0)
                {
                    result.Validated = false;
                    result.ErrorMessage = $"Line {line.Id} quantity should be more then 0";
                    return result;
                }            
            }
            result.Validated = true;
            return result;
        }

    }
    public class ValidatorResult
    {
        public bool Validated { get; set; }
        public string ErrorMessage { get; set; }
        public ValidatorResult()
        {
            Validated = false;
            ErrorMessage = "";
        }
    }

}
