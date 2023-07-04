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
    public class TestsOrderValidator
    {
        [Fact]
        public void CanEditOrderLines_ReturnsValid()
        {
            // Arrange
            OrderGenerator orderGenerator = new OrderGenerator();
            ValidationStatus excpectedResult = ValidationStatus.Valid;
            IOrderValidator validator = new OrderValidator();

            Order orderData = orderGenerator.Order();
            // Act
            ValidationResult result = validator.CanEditOrderLines(orderData);
            Assert.Equal(excpectedResult, result.Validated);
        }
        [Fact]
        public void CanEditOrderLines_ReturnsCannotEditLines()
        {
            // Arrange
            OrderGenerator orderGenerator = new OrderGenerator();
            ValidationStatus excpectedResult = ValidationStatus.CannotEditLines;
            IOrderValidator validator = new OrderValidator();

            Order orderData = orderGenerator.Order(OrderStatus.InDelivery);
            // Act
            ValidationResult result = validator.CanEditOrderLines(orderData);
            Assert.Equal(excpectedResult, result.Validated); 
        }
        [Fact]
        public void CanDeleteOrder_ReturnsValid()
        {
            // Arrange
            OrderGenerator orderGenerator = new OrderGenerator();
            ValidationStatus excpectedResult = ValidationStatus.Valid;
            IOrderValidator validator = new OrderValidator();

            Order orderData = orderGenerator.Order(OrderStatus.Paid);
            // Act
            ValidationResult result = validator.CanDeleteOrder(orderData);
            // Assert
            Assert.Equal(excpectedResult, result.Validated);
        }
        [Fact]
        public void CanDeleteOrder_ReturnsInvalid()
        {           
            // Arrange
            OrderGenerator orderGenerator = new OrderGenerator();
            ValidationStatus excpectedResult = ValidationStatus.Invalid;
            IOrderValidator validator = new OrderValidator();

            Order orderData = orderGenerator.Order(OrderStatus.Delivered);
            // Act
            ValidationResult result = validator.CanDeleteOrder(orderData);
            // Assert
            Assert.Equal(excpectedResult, result.Validated);
        }
        [Fact]
        public void ValidateOrderLines_ReturnsValid()
        {
            // Arrange
            IOrderValidator validator = new OrderValidator();
            List<OrderLineModel> orderLines = LineGenerator.LinesModel(1, true);
            ValidationStatus excpectedResult = ValidationStatus.Valid;

            // Act
            ValidationResult result = validator.ValidateOrderLines(orderLines, Guid.NewGuid());
            // Assert
            Assert.Equal(excpectedResult, result.Validated);
        }
        [Fact]
        public void ValidateOrderLines_ReturnsInvalid()
        {
            // Arrange
            IOrderValidator validator = new OrderValidator();
            List<OrderLineModel> orderLines = LineGenerator.LinesModel(1, false);
            ValidationStatus excpectedResult = ValidationStatus.Invalid;

            // Act
            ValidationResult result = validator.ValidateOrderLines(orderLines, Guid.NewGuid());
            // Assert
            Assert.Equal(excpectedResult, result.Validated);
        }
        
    }
}
