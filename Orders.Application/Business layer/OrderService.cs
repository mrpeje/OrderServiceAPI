using Orders.Domain;
using OrdersService.Business_layer.Validator;
using OrdersService.Context;
using OrdersService.DB_Access;
using OrdersService.Interfaces;
using OrdersService.Models;

namespace OrdersService.Business_layer
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderValidator _orderValidator;

        Order _order { get; set; }
        public OrderService(IOrderRepository dbProvider, IOrderValidator orderValidator)
        {
            _orderRepository = dbProvider;
            _orderValidator = orderValidator;
            _order = new Order();
        }

        public OrderModel GetOrderModel(Guid orderId)
        {
            FindAndInitOrder(orderId);

            var dtoOrderLines = MapLinesModelWithDb(); 
            
            return new OrderModel
            {
                Id = _order.Id,
                Status = _order.Status,
                Created = _order.Created,
                Lines = dtoOrderLines
            };
        }
        private List<OrderLineModel> MapLinesModelWithDb()
        {
            var dtoOrderLines = new List<OrderLineModel>();
            foreach (var line in _order.Lines)
            {
                var dtoLine = new OrderLineModel
                {
                    Id = line.Id,
                    qty = line.qty
                };
                dtoOrderLines.Add(dtoLine);
            }
            return dtoOrderLines;
        }

        public OperationResult CreateOrder(NewOrderModel orderModel)
        {
            var result = new OperationResult();
    
            var orderLinesValidator = _orderValidator.ValidateOrderLines(orderModel.Lines, orderModel.Id);
            if (orderLinesValidator.Validated == ValidationStatus.Invalid)
            {
                result.Status = OperationStatus.Error;
                result.ErrorMessage = orderLinesValidator.ErrorMessage;
                return result;
            }

            NewOrderByModel(orderModel);
            
            result.Status = _orderRepository.CreateOrder(_order);
            if (result.Status != OperationStatus.Success)
            {
                result.ErrorMessage = $"DB Error while creating new order {_order.Id}";
            }
            return result;
        }
        private void NewOrderByModel(NewOrderModel orderModel)
        {
            var orderLines = MapDbLinesWithModel(orderModel.Lines, orderModel.Id);

            _order.Id = orderModel.Id;
            _order.Created = DateTime.UtcNow;
            _order.Status = OrderStatus.New.ToString();
            _order.Lines = orderLines;
        }
        private List<OrderLine> MapDbLinesWithModel(List<OrderLineModel> listModel, Guid orderId)
        {
            var orderLines = new List<OrderLine>();

            foreach (var item in listModel)
            {
                var newLine = new OrderLine
                {
                    Id = item.Id,
                    qty = item.qty,
                    OrderId = orderId
                };
                orderLines.Add(newLine);
            }
            return orderLines;
        }

        public OperationResult UpdateOrder(Guid orderId, EditOrderModel orderModel)
        {
            var result = new OperationResult();

            FindAndInitOrder(orderId);
            if (_order.Id == Guid.Empty)
            {
                result.Status = OperationStatus.NotFound;
                result.ErrorMessage = $"Order {orderId} not found";
                return result;
            }

            var canEdit = _orderValidator.CanEditOrderLines(_order);
            if (canEdit.Validated != ValidationStatus.Valid)
            {
                result.Status = OperationStatus.Error;
                result.ErrorMessage = canEdit.ErrorMessage;
                return result;
            }
            var orderLinesValidator = _orderValidator.ValidateOrderLines(orderModel.Lines, orderId);
            if (orderLinesValidator.Validated != ValidationStatus.Valid)
            {
                result.Status = OperationStatus.Error;
                result.ErrorMessage = orderLinesValidator.ErrorMessage;
                return result;
            }

            var userLines = CreateLinesDbRecord(orderModel.Lines, orderId);
            var isOrderLinesEdited = AnyDiffInLines(userLines, _order.Lines.ToList());
            if (isOrderLinesEdited)
            {
                _order.Lines = userLines;
            }

            result.Status = _orderRepository.UpdateOrder(_order);
            if (result.Status != OperationStatus.Success)
            {
                result.ErrorMessage = $"DB Error while updating order {orderId}";
            }
            return result;
        }

        private List<OrderLine> CreateLinesDbRecord(List<OrderLineModel> linesModel, Guid orderId)
        {  
            var userLines = new List<OrderLine>();
            foreach (var line in linesModel)
            {
                userLines.Add(new OrderLine
                {
                    Id = line.Id,
                    qty = line.qty,
                    OrderId = orderId
                });
            }
            return userLines;
        }
        private bool AnyDiffInLines(List<OrderLine> requestLines, List<OrderLine> dbLines)
        {
            return !requestLines.OrderBy(e => e.Id).SequenceEqual(dbLines.OrderBy(e => e.Id));
        }

        public OperationResult DeleteOrder(Guid orderId)
        {
            var result = new OperationResult();

            FindAndInitOrder(orderId);
            if (_order.Id == Guid.Empty)
            {
                result.Status = OperationStatus.NotFound;
                result.ErrorMessage = $"Order {orderId} not found";
                return result;
            }
            var canDeleted = _orderValidator.CanDeleteOrder(_order);
            if (canDeleted.Validated == ValidationStatus.Invalid)
            {
                result.Status = OperationStatus.Error;
                result.ErrorMessage = canDeleted.ErrorMessage;
                return result;
            }

            result.Status = _orderRepository.DeleteOrder(orderId);
            if (result.Status != OperationStatus.Success)
            {
                result.ErrorMessage = $"DB Error while deleting order {orderId}";
            }
            return result;
        }
        private void FindAndInitOrder(Guid orderId)// ?
        {
            _order = _orderRepository.GetOrderById(orderId);
            if(_order == null)
            {
                _order = new Order();
                _order.Id = Guid.Empty;
            }
        }
    }
    public class OperationResult
    {
        public OperationStatus Status { get; set; }
        public string ErrorMessage { get; set; }
        public OperationResult()
        {
            ErrorMessage = string.Empty;
            Status = OperationStatus.Error;
        }
    }
}