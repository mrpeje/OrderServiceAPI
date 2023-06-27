using OrdersService.Context;
using OrdersService.Models;
using System;
using System.Collections.Generic;

namespace TestApplication
{
    internal class OrderGenerator
    {
        Guid _id;
        public OrderGenerator()
        {
            _id = Guid.NewGuid();
        }
        public Order Order()
        {
            var order = new Order();
            order.Id = _id;
            order.Lines =  new List<OrderLine>();
            return order;
        }

        public OrderModel OrderModel()
        {
            var orderModel = new OrderModel();
            orderModel.Id = _id;
            orderModel.Lines = new List<OrderLineModel>();
            return orderModel;
        }

        internal NewOrderModel NewOrderModel()
        {
            var newOrderModel = new NewOrderModel();
            newOrderModel.Id = _id;
            newOrderModel.Lines = new List<OrderLineModel>();
            return newOrderModel;
        }
    }
}