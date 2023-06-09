using Moq;
using Orders.Domain;
using OrdersService.Business_layer;
using OrdersService.Business_layer.Validator;
using OrdersService.Context;
using OrdersService.DB_Access;
using OrdersService.Interfaces;
using OrdersService.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestApplication
{
    public class TestsOrderService
    {

        [Fact]
        public void GetOrderModel_GetsOrderFromRepository_OK()
        {
            // Arrange
            OrderGenerator _orderGenerator = new OrderGenerator();
            var repositoryMock = new Mock<IOrderRepository>();
            var validatorMock = new Mock<IOrderValidator>();
            IOrderService _service = new OrderService(repositoryMock.Object, validatorMock.Object);

            OrderModel orderModel = _orderGenerator.OrderModel();
            Order order = _orderGenerator.Order();
            repositoryMock.Setup(or => or.GetOrderById(order.Id)).Returns(order);

            // Act
            OrderModel returnedOrderModel = _service.GetOrderModel(order.Id);

            // Assert
            Assert.Equal(orderModel.Id, returnedOrderModel.Id);
        }

        [Fact]
        public void CreateOrder_ReturnsSuccess_WhenOrderCreatedSuccessfully()
        {
            // Arrange
            OrderGenerator _orderGenerator = new OrderGenerator();
            var repositoryMock = new Mock<IOrderRepository>();
            var validatorMock = new Mock<IOrderValidator>();
            IOrderService _service = new OrderService(repositoryMock.Object, validatorMock.Object);

            NewOrderModel newOrderModel = _orderGenerator.NewOrderModel();

            repositoryMock.Setup(or => or.CreateOrder(It.IsAny<Order>()))
                .Returns(OperationStatus.Success);
            validatorMock.Setup(ov => ov.ValidateOrderLines(It.IsAny<List<OrderLineModel>>(), It.IsAny<Guid>()))
                .Returns(new ValidationResult { Validated = ValidationStatus.Valid });
            // Act
            OperationResult result = _service.CreateOrder(newOrderModel);

            // Assert
            Assert.Equal(OperationStatus.Success, result.Status);
        }
        [Fact]
        public void CreateOrder_ReturnsError_WhenOrderValidation_Invalid()
        {
            // Arrange
            OrderGenerator _orderGenerator = new OrderGenerator();
            var repositoryMock = new Mock<IOrderRepository>();
            var validatorMock = new Mock<IOrderValidator>();
            IOrderService _service = new OrderService(repositoryMock.Object, validatorMock.Object);

            NewOrderModel newOrderModel = _orderGenerator.NewOrderModel();
            Order order = _orderGenerator.Order();

            repositoryMock.Setup(or => or.CreateOrder(order))
                .Returns(OperationStatus.Success);
            validatorMock.Setup(ov => ov.ValidateOrderLines(It.IsAny<List<OrderLineModel>>(), It.IsAny<Guid>()))
                .Returns(new ValidationResult { Validated = ValidationStatus.Invalid });
            // Act
            OperationResult result = _service.CreateOrder(newOrderModel);

            // Assert
            Assert.Equal(OperationStatus.Error, result.Status);
        }

        [Fact]
        public void UpdateOrder_ReturnsSuccess()
        {
            // Arrange
            OrderGenerator _orderGenerator = new OrderGenerator();
            var repositoryMock = new Mock<IOrderRepository>();
            var validatorMock = new Mock<IOrderValidator>();
            IOrderService _service = new OrderService(repositoryMock.Object, validatorMock.Object);

            EditOrderModel editOrderModel = _orderGenerator.EditOrderModel();
            Order order = _orderGenerator.Order();

            repositoryMock.Setup(or => or.GetOrderById(order.Id))
                .Returns(order);
            repositoryMock.Setup(or => or.UpdateOrder(order))
                .Returns(OperationStatus.Success);
            validatorMock.Setup(ov => ov.ValidateOrderLines(It.IsAny<List<OrderLineModel>>(), It.IsAny<Guid>()))
                .Returns(new ValidationResult { Validated = ValidationStatus.Valid });
            validatorMock.Setup(ov => ov.CanEditOrderLines(order))
                .Returns(new ValidationResult { Validated = ValidationStatus.Valid });
            // Act
            OperationResult result = _service.UpdateOrder(order.Id, editOrderModel);
            // Assert
            Assert.Equal(OperationStatus.Success, result.Status);
        }
        [Fact]
        public void DeleteOrder_ReturnsSuccess()
        {
            // Arrange
            OrderGenerator _orderGenerator = new OrderGenerator();
            var repositoryMock = new Mock<IOrderRepository>();
            var validatorMock = new Mock<IOrderValidator>();
            IOrderService _service = new OrderService(repositoryMock.Object, validatorMock.Object);

            EditOrderModel editOrderModel = _orderGenerator.EditOrderModel();
            Order order = _orderGenerator.Order();

            repositoryMock.Setup(or => or.GetOrderById(order.Id))
                .Returns(order);
            repositoryMock.Setup(or => or.DeleteOrder(order.Id))
                .Returns(OperationStatus.Success);
            validatorMock.Setup(ov => ov.CanDeleteOrder(It.IsAny<Order>()))
                .Returns(new ValidationResult { Validated = ValidationStatus.Valid });

            // Act
            OperationResult result = _service.DeleteOrder(order.Id);
            // Assert
            Assert.Equal(OperationStatus.Success, result.Status);
        }         
        [Fact]
        public void DeleteOrder_ReturnsFail()
        {
            // Arrange
            OrderGenerator _orderGenerator = new OrderGenerator();
            var repositoryMock = new Mock<IOrderRepository>();
            var validatorMock = new Mock<IOrderValidator>();
            IOrderService _service = new OrderService(repositoryMock.Object, validatorMock.Object);

            EditOrderModel editOrderModel = _orderGenerator.EditOrderModel();
            Order order = _orderGenerator.Order();

            repositoryMock.Setup(or => or.GetOrderById(order.Id))
                .Returns(order);
            repositoryMock.Setup(or => or.DeleteOrder(order.Id))
                .Returns(OperationStatus.Success);
            validatorMock.Setup(ov => ov.CanDeleteOrder(It.IsAny<Order>()))
                .Returns(new ValidationResult { Validated = ValidationStatus.Invalid });

            // Act
            OperationResult result = _service.DeleteOrder(order.Id);
            // Assert
            Assert.Equal(OperationStatus.Error, result.Status);
        }
    }
}