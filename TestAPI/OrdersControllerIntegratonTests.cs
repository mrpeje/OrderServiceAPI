using FluentAssertions;
using Newtonsoft.Json;
using OrdersService.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace TestAPI
{
    public class OrdersControllerIntegratonTests
    {

        [Fact]
        public async Task GetOrder_WithoutOrders_ReturnNotFound()
        {
            // Arrange
            await using var application = new ApiWebApplicationFactory();
            using var testClient = application.CreateClient();
            var id = System.Guid.NewGuid();
            // Act
            var response = await testClient.GetAsync("/api/orders/" + id);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task GetOrder_WithExistingOrder_ReturnOK()
        {
            // Arrange
            await using var application = new ApiWebApplicationFactory();
            using var testClient = application.CreateClient();

            NewOrderModel orderModel = CreateOrderModel(1);
            var content = ConvertToByteArrayContent<NewOrderModel>(orderModel);
            var responseCreate = await testClient.PostAsync("/api/orders", content);
            // Act
            var responseGet = await testClient.GetAsync("/api/orders/" + orderModel.Id);
            // Assert
            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task CreateOrder_NewOrder_ReturnOk()
        {
            // Arrange
            await using var application = new ApiWebApplicationFactory();
            using var testClient = application.CreateClient();
            NewOrderModel orderModel = CreateOrderModel(1);
            var content = ConvertToByteArrayContent<NewOrderModel>(orderModel);
            // Act
            var response = await testClient.PostAsync("/api/orders", content);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteOrder_ExistingOrder_ReturnOk()
        {
            // Arrange
            await using var application = new ApiWebApplicationFactory();
            using var testClient = application.CreateClient();
            NewOrderModel orderModel = CreateOrderModel(1);
            var content = ConvertToByteArrayContent(orderModel);
            var createResponse = await testClient.PostAsync("/api/orders", content);
            // Act
            var deleteResponse = await testClient.DeleteAsync("/api/orders/" + orderModel.Id);
            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private ByteArrayContent ConvertToByteArrayContent<T>(T requestObject)
        {
            var myContent = JsonConvert.SerializeObject(requestObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }

        /// <summary>
        /// Create orderModel with specified number of lines.
        /// </summary>
        /// <param name="linesCount">The number of lines in the order.</param>
        /// <returns></returns>
        private NewOrderModel CreateOrderModel(int linesCount)
        {
            var linesModel = new List<OrderLineModel>();
            for (int i = 0; i < linesCount; i++)
            {
                var line = new OrderLineModel
                {
                    Id = System.Guid.NewGuid(),
                    qty = (uint)(i + 1)
                };
                linesModel.Add(line);
            }

            var orderModel = new NewOrderModel
            {
                Id = System.Guid.NewGuid(),
                Lines = linesModel
            };
            return orderModel;
        }
    }
}