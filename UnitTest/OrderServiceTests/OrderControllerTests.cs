using System;
using System.Linq.Expressions;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.Controllers;
using OrderMicroservice.Entities.DataToObjects;
using Shared.DataAccess.Abstract;
using Shared.Entities.DataToObjects;
using Entities.Concrete;

namespace UnitTests.OrderServiceTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderRepository> orderRepositoryStub = new();

        [Fact]
        public void CreateOrder_ReturnsCreatedOrderId()
        {
            // Arrange
            var orderToCreate = CreateRandomOrder();
            var orderCreateModel = new OrderDTO
            {
                Id = orderToCreate.Id,
                Quantity = orderToCreate.Quantity,
                Price = orderToCreate.Price,
                Status = orderToCreate.Status,
                AddressId = orderToCreate.AddressId,
                ProductId = orderToCreate.ProductId
            };
            orderRepositoryStub.Setup(repo => repo.Add(It.IsAny<Order>())).Returns(orderToCreate);
            var controller = new OrderController(orderRepositoryStub.Object);

            // Act
            var result = controller.Create(orderCreateModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var createdOrderId = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(orderToCreate.Id, createdOrderId);
        }

        [Fact]
        public void GetOrderById_ExistingId_ReturnsResultWithOrder()
        {
            // Arrange
            var existingOrder = CreateRandomOrder();
            Guid existingOrderId = existingOrder.Id;
            orderRepositoryStub.Setup(repo => repo.Get(It.IsAny<Expression<Func<Order, bool>>>())).Returns(existingOrder);
            var controller = new OrderController(orderRepositoryStub.Object);

            var requestBody = new IdDTO
            {
                Id = existingOrderId
            };

            // Act
            var result = controller.GetOrderById(requestBody);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrder = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(existingOrderId, returnedOrder.Id);
        }

        [Fact]
        public void Get_ReturnsListOfOrders()
        {
            // Arrange
            var orders = new List<Order>
    {
        CreateRandomOrder(),
        CreateRandomOrder(),
        CreateRandomOrder()
    };
            orderRepositoryStub.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(orders);
            var controller = new OrderController(orderRepositoryStub.Object);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrders = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Equal(orders.Count, returnedOrders.Count);
        }

        [Fact]
        public void UpdateOrder_ExistingOrder_ReturnsResult()
        {
            // Arrange
            var existingOrder = CreateRandomOrder();
            var updatedOrder = new OrderDTO
            {
                Id = existingOrder.Id,
                Quantity = 15,
                Price = 150.0,
                Status = "UpdatedStatus",
                AddressId = existingOrder.AddressId,
                ProductId = existingOrder.ProductId
            };
            orderRepositoryStub.Setup(repo => repo.Get(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(existingOrder);
            var controller = new OrderController(orderRepositoryStub.Object);

            // Act
            var result = controller.Update(updatedOrder);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updated = Assert.IsType<bool>(okResult.Value);
            Assert.True(updated);

            Assert.Equal(updatedOrder.Quantity, existingOrder.Quantity);
            Assert.Equal(updatedOrder.Price, existingOrder.Price);
            Assert.Equal(updatedOrder.Status, existingOrder.Status);
            Assert.Equal(updatedOrder.AddressId, existingOrder.AddressId);
            Assert.Equal(updatedOrder.ProductId, existingOrder.ProductId);
        }

        [Fact]
        public void DeleteOrder_ExistingOrder_ReturnsResult()
        {
            // Arrange
            var existingOrder = CreateRandomOrder();
            var requestBody = new IdDTO
            {
                Id = existingOrder.Id
            };
            orderRepositoryStub.Setup(repo => repo.Get(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(existingOrder);
            var controller = new OrderController(orderRepositoryStub.Object);

            // Act
            var result = controller.Delete(requestBody);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var isDeleted = Assert.IsType<bool>(okResult.Value);
            Assert.True(isDeleted);
        }



        private Order CreateRandomOrder()
        {
            DateTime now = DateTime.UtcNow;
            return new Order
            {
                Id = Guid.NewGuid(),
                Quantity = 10,
                Price = 100.0,
                Status = "Pending",
                AddressId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                CreatedAt = now,
                UpdatedAt = now
            };
        }
    }
}
