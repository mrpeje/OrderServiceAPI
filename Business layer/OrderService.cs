using OrdersService.Business_layer.Validator;
using OrdersService.Context;
using OrdersService.DB_Access;
using OrdersService.Models;
using System.Net;

namespace OrdersService.Business_layer
{
    public class OrderService : IOrderService
    {
        private readonly IDB_Provider _dbProvider;
        private readonly IOrderValidator _orderValidator;
        public OrderService(IDB_Provider dbProvider, IOrderValidator orderValidator)
        {
            _dbProvider = dbProvider;
            _orderValidator = orderValidator;
        }

        public OrderModel GetOrderData(Guid orderId)
        {
            var order = _dbProvider.GetOrderById(orderId);
            var dtoLines = new List<OrderLineModel>();
            foreach (var line in order.Lines)
            {
                var dtoLine = new OrderLineModel
                {
                    Id = line.Id,
                    qty = line.qty
                };
                dtoLines.Add(dtoLine);
            }
            return new OrderModel
            {
                Id = order.Id,
                Status = order.Status,
                Created = order.Created,
                Lines = dtoLines
            };
        }
        public OrderModel CreateOrder(NewOrder orderData)
        {
            // Validate order data
            bool validated = _orderValidator.ValidateOrderLines(orderData.Lines);
            var lines = new List<OrderLine>();
            if (validated)
            {
               
                // Create OrderLines
                foreach (var item in orderData.Lines)
                {
                    var newLine = new OrderLine 
                    {   
                        Id = item.Id, 
                        qty = item.qty, 
                        OrderId = orderData.Id 
                    };
                    lines.Add(newLine);
                }
                // Create Order
                var order = new Order
                {
                    Id = orderData.Id,
                    Created = DateTime.UtcNow,
                    Status = OrderStatus.New.ToString(),
                    Lines = lines
                };

                var orderCreationResult = _dbProvider.CreateOrder(order);
                if(orderCreationResult == OperationStatus.Success)
                {
                    return GetOrderData(order.Id);
                }                
            }
            return null;           
        }
        public OrderModel UpdateOrderData(Guid orderId, EditOrderModel orderData)
        {
            // Validate order data
            var dbOrder = _dbProvider.GetOrderById(orderId);
            if (dbOrder == null)
            {
                return null;
            }
            bool canEdit = _orderValidator.CanEditOrder(dbOrder);
            bool validated = _orderValidator.ValidateOrderLines(orderData.Lines);

            if (validated && canEdit)
            {
                // Update Order
                var userLines = new List<OrderLine>();
                foreach(var line in orderData.Lines)
                {
                    userLines.Add(new OrderLine
                    {
                        Id = line.Id,
                        qty = line.qty,
                        OrderId = orderId
                    });
                }
                var order = new Order
                { 
                    Id = orderId,
                    Status = orderData.Status.ToString(),
                    Created = dbOrder.Created,
                    Lines = userLines 
                };
                var orderUpdateResult =_dbProvider.UpdateOrder(order);
                if (orderUpdateResult == OperationStatus.Success)
                {
                    return GetOrderData(order.Id);
                }
            }
            return null;           
        }
        public HttpStatusCode DeleteOrder(Guid orderId)
        {
            // Validate order
            var order = _dbProvider.GetOrderById(orderId);
            if(order == null)
            {
                return HttpStatusCode.BadRequest;
            }
            bool canDeleted = _orderValidator.CanDeleteOrder(order);
            if (canDeleted)
            {
                // Delete Order
                var result = _dbProvider.DeleteOrder(orderId);
                HttpStatusCode returnValue = HttpStatusCode.NoContent;
                switch (result)
                {
                    case OperationStatus.Success:
                        returnValue = HttpStatusCode.OK;
                        break;
                    case OperationStatus.NotFound:
                        returnValue = HttpStatusCode.NotFound;
                        break;
                    case OperationStatus.Error:
                        returnValue = HttpStatusCode.BadRequest;
                        break;
                }
                return returnValue;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }
}