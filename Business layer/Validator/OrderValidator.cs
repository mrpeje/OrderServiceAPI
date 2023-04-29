using OrdersService.Context;
using OrdersService.Models;

namespace OrdersService.Business_layer.Validator
{
    public class OrderValidator : IOrderValidator
    {
        public bool CanDeleteOrder(Order order)
        {
            return order.Status != OrderStatus.InDelivery.ToString()
                   && order.Status != OrderStatus.Delivered.ToString()
                   && order.Status != OrderStatus.Completed.ToString();
        }

        public bool CanEditOrder(Order orderData)
        {

            return orderData.Status != OrderStatus.Paid.ToString()
                   && orderData.Status != OrderStatus.InDelivery.ToString()
                   && orderData.Status != OrderStatus.Delivered.ToString()
                   && orderData.Status != OrderStatus.Completed.ToString();
        }


        public bool ValidateOrderLines(List<OrderLineModel> lines)
        {
            if (lines == null || lines.Count == 0)
            {
                return false;
            }
            foreach (var line in lines)
            {
                if (line.qty <= 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
