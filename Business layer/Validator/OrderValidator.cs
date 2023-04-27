using OrdersService.Context;
using OrdersService.Models;

namespace OrdersService.Business_layer.Validator
{
    public class OrderValidator : IOrderValidator
    {
        public bool CanDeleteOrder(OrderWithLines order)
        {
            return order.Status != OrderStatus.InDelivery.ToString()
                   && order.Status != OrderStatus.Delivered.ToString()
                   && order.Status != OrderStatus.Completed.ToString();
        }

        public bool CanEditOrder(OrderWithLines orderData)
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
                //errorMessage = "Невозможно создать заказ без строк.";
                return false;
            }
            foreach (var line in lines)
            {
                if (line.qty <= 0)
                {
                    //errorMessage = $"Количество по строке заказа '{line.Id}' должно быть больше 0.";
                    return false;
                }
            }
            return true;
        }
    }
}
