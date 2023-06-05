using OrdersService.Business_layer.Validator;
using OrdersService.Context;
using OrdersService.DB_Access;
using OrdersService.Interfaces;
using Orders.Domain;
using Orders.Domain.Models;

namespace OrdersService.Business_layer
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _dbProvider;
        private readonly IOrderValidator _orderValidator;
        public OrderService(IOrderRepository dbProvider, IOrderValidator orderValidator)
        {
            _dbProvider = dbProvider;
            _orderValidator = orderValidator;
        }

        public OrderModel GetOrderData(Guid orderId)
        {
            var order = _dbProvider.GetOrderById(orderId);
            if(order == null)
                return null;
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

        public OperationResult CreateOrder(NewOrder orderData)
        {
            // Validate order data
            var result = new OperationResult();
            var orderLinesValidator = _orderValidator.ValidateOrderLines(orderData.Lines, orderData.Id);

            if (orderLinesValidator.Validated == ValidationStatus.Invalid)
            {
                result.Status = OperationStatus.Error;
                result.ErrorMessage = orderLinesValidator.ErrorMessage;
                return result;
            }

            var lines = new List<OrderLine>();
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

            result.Status = _dbProvider.CreateOrder(order);
            if (result.Status != OperationStatus.Success)
            {
                result.ErrorMessage = $"DB Error while creating new order {order.Id}";
            }
            return result;                        
        }
        public OperationResult UpdateOrderData(Guid orderId, EditOrderModel orderData)
        {
            // Validate order 
            var result = new OperationResult();
            List<OrderLine>? linesToWrite;
            var dbOrder = _dbProvider.GetOrderById(orderId);
            if (dbOrder == null)
            {
                result.Status = OperationStatus.NotFound;
                result.ErrorMessage = $"Order {orderId} not found";
                return result;
            }

            // Prepare dao    
            var userLines = new List<OrderLine>();
            foreach (var line in orderData.Lines)
            {
                userLines.Add(new OrderLine
                {
                    Id = line.Id,
                    qty = line.qty,
                    OrderId = orderId
                });
            }

            // Validate order lines
            var isOrderLinesEdited = _orderValidator.isEdited(userLines, dbOrder.Lines.ToList());
            if (isOrderLinesEdited.Validated == ValidationStatus.UnequalLines)
            {
                var canEdit = _orderValidator.CanEditOrderLines(dbOrder);
                if (canEdit.Validated != ValidationStatus.Valid)
                {
                    result.Status = OperationStatus.Error;
                    result.ErrorMessage = canEdit.ErrorMessage;
                    return result;
                }

                var orderLinesValidator = _orderValidator.ValidateOrderLines(orderData.Lines, orderId);
                if (orderLinesValidator.Validated != ValidationStatus.Valid)
                {
                    result.Status = OperationStatus.Error;
                    result.ErrorMessage = orderLinesValidator.ErrorMessage;
                    return result;
                }
                linesToWrite = userLines;
            }
            else
            {
                // If changed only status use DB lines to avoid update
                linesToWrite = dbOrder.Lines.ToList();
            }

            var order = new Order
            {
                Id = orderId,
                Status = orderData.Status.ToString(),
                Created = dbOrder.Created,
                Lines = linesToWrite
            };
            // Update Order
            result.Status = _dbProvider.UpdateOrder(order);
            if (result.Status != OperationStatus.Success)
            {
                result.ErrorMessage = $"DB Error while updating order {orderId}";
            }
            return result;
        }
        public OperationResult DeleteOrder(Guid orderId)
        {
            // Validate order
            var result = new OperationResult();

            var order = _dbProvider.GetOrderById(orderId);
            if(order == null)
            {
                result.Status=OperationStatus.NotFound;
                result.ErrorMessage = $"Order {orderId} not found";
                return result;
            }

            var canDeleted = _orderValidator.CanDeleteOrder(order);
            if (canDeleted.Validated == ValidationStatus.Invalid)
            {
                result.Status = OperationStatus.Error;
                result.ErrorMessage = canDeleted.ErrorMessage;
                return result;
            }

            // Delete Order
            result.Status = _dbProvider.DeleteOrder(orderId);
            if (result.Status != OperationStatus.Success)
            {
                result.ErrorMessage = $"DB Error while deleting order {orderId}";
            }
            return result;         
        }
    }
    public class OperationResult
    {
        public OperationStatus Status { get; set; }
        public string ErrorMessage { get; set; }
        public OperationResult()
        {
            ErrorMessage = string.Empty;
        }
    }
}