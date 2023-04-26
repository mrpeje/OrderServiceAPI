using OrdersService.Context;
using OrdersService.DB_Access;
using OrdersService.Models;
using System.Net;

namespace OrdersService.Business_Layer
{
    public class OrderService : IOrderService
    {
        private readonly IDB_Provider _dbProvider;
        public OrderService(IDB_Provider dbProvider)
        {
            _dbProvider = dbProvider;
        }
        public OrderWithLines GetOrderData(Guid orderId)
        {
            var order = _dbProvider.GetOrderById(orderId);
            var daolines = _dbProvider.GetOrderLinesByOrderId(orderId);
            var dtoLines = new List<OrderLineModel>();
            foreach (var line in daolines)
            {
                var dtoLine = new OrderLineModel
                {
                    Id = line.Id,
                    qty = line.qty
                };
                dtoLines.Add(dtoLine);
            }
            return new OrderWithLines
            {
                Id = order.Id,
                Status = order.Status,
                Created = order.Created,
                Lines = dtoLines
            };
        }
        public OrderWithLines CreateOrder(NewOrder orderData)
        {
            // Validate order data
            bool validated = true;

            if (validated)
            {
                var order = new Order
                {
                    Id = orderData.Id,
                    Created = DateTime.UtcNow,
                    Status = OrderStatus.New.ToString()
                };
                var orderCreationResult = _dbProvider.CreateOrder(order);
                if (orderCreationResult == OperationStatus.Success)
                {
                    foreach (var item in orderData.Lines)
                    {
                        var newLine = new OrderLine 
                        {   
                            Id = item.Id, 
                            qty = item.qty, 
                            OrderId = order.Id 
                        };
                        _dbProvider.CreateOrderLine(newLine);
                    }
                }
                return GetOrderData(order.Id);
            }
            else
            {
                return null;
            }
        }
        public OrderWithLines UpdateOrderData(OrderWithLines orderData)
        {
            var order = new Order 
            { 
                Id = orderData.Id,
                Status = orderData.Status,
                Created = orderData.Created
            };
            var userLines = orderData.Lines;
            _dbProvider.UpdateOrder(order);
            // Update lines
            foreach(var usrLine in userLines)
            {
                var line = new OrderLine
                {
                    Id = usrLine.Id,
                    qty = usrLine.qty,
                    OrderId = order.Id
                };
                var status = _dbProvider.UpdateOrderLine(line);
                if(status == OperationStatus.NotFound)
                {
                    _dbProvider.CreateOrderLine(line);
                }
            }
            // Delete lines
            var dbLines = _dbProvider.GetOrderLinesByOrderId(order.Id);
            var deletedLines = dbLines.Where(dbItem => userLines.All(userItem => dbItem.Id != userItem.Id)).ToList();
            foreach (var line in deletedLines)
            {
                _dbProvider.DeleteOrderLine(line.Id);
            }

            return GetOrderData(order.Id);
        }
        public HttpStatusCode DeleteOrder(Guid orderId)
        {
            
            var newOrderLines = _dbProvider.GetOrderLinesByOrderId(orderId);
            foreach (var line in newOrderLines)
            {
                _dbProvider.DeleteOrderLine(line.Id);
            }

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
    }
}