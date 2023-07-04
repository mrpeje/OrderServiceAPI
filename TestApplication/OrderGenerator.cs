using Orders.Domain;
using OrdersService.Context;
using OrdersService.Models;
using System;
using System.Collections.Generic;

namespace TestApplication
{
    public class OrderGenerator
    {
        Guid _id;
        public OrderGenerator()
        {
            _id = Guid.NewGuid();
        }
        public Order Order(OrderStatus status = 0)
        {
            var order = new Order();
            order.Id = _id;
            order.Lines =  new List<OrderLine>();
            order.Status = status.ToString();

            return order;
        }

        public OrderModel OrderModel()
        {
            var orderModel = new OrderModel();
            orderModel.Id = _id;
            orderModel.Lines = new List<OrderLineModel>();
            return orderModel;
        }

        public NewOrderModel NewOrderModel()
        {
            var newOrderModel = new NewOrderModel();
            newOrderModel.Id = _id;
            newOrderModel.Lines = new List<OrderLineModel>();
            return newOrderModel;
        }

        public EditOrderModel EditOrderModel()
        {
            var editOrderModel = new EditOrderModel();
            editOrderModel.Lines = new List<OrderLineModel>();
            return editOrderModel;
        }
    }
    public static class LineGenerator
    {
        /// <summary>
        /// Generate lines 
        /// </summary>
        /// <param name="count">lines</param>
        /// <param name="valid">if true qty > 0, else qty = 0 </param>
        /// <returns></returns>
        public static List<OrderLineModel> LinesModel(uint count, bool valid)
        {
            List<OrderLineModel> linesModel = new List<OrderLineModel>();
            uint quantity = 1;
            if(!valid)
            {
                quantity = 0;
            }
            for(int i = 0; i < count; i++)
            {
                var line = new OrderLineModel
                {
                    Id = Guid.NewGuid(),
                    qty = quantity
                };
                linesModel.Add(line);
            }
            return linesModel;
        }
    }
}