using System;
using System.Linq.Expressions;
using CustomerMicroservice.Controllers;
using CustomerMicroservice.Entities.DataToObjects;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DataAccess.Abstract;
using Shared.Entities.DataToObjects;
using Xunit;

namespace UnitTests.CustomerServiceTests;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerRepository> customerRepositoryStub = new();
    private readonly Mock<IOrderRepository> orderRepository = new();

    [Fact]
    public void CreateCustomer_ReturnsCreatedCustomerId()
    {
        //Arrange

        var customerToCreate = CreateRandomCustomer();
        var customerCreateModel = new CustomerDTO
        {
            Id = customerToCreate.Id,
            Name = customerToCreate.Name,
            Email = customerToCreate.Email,
            AddressId = customerToCreate.AddressId
        };
        customerRepositoryStub.Setup(repo => repo.Add(It.IsAny<Customer>())).Returns(customerToCreate);
        var controller = new CustomerController(customerRepositoryStub.Object);

        //Act

        var result = controller.Create(customerCreateModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var createdCustomerId = Assert.IsType<Guid>(okResult.Value);
        Assert.Equal(customerToCreate.Id, createdCustomerId);
    }

    [Fact]
    public void GetCustomerById_ExistingId_ReturnsResultWithCustomer()
    {
        // Arrange
        var existingCustomer = CreateRandomCustomer();
        Guid existingCustomerId = existingCustomer.Id;
        customerRepositoryStub.Setup(repo => repo.Get(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(existingCustomer);
        var controller = new CustomerController(customerRepositoryStub.Object);

        var requestBody = new IdDTO
        {
            Id = existingCustomerId
        };

        // Act
        var result = controller.GetCustomerById(requestBody);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomer = Assert.IsType<Customer>(okResult.Value);
        Assert.Equal(existingCustomerId, returnedCustomer.Id);
    }

    [Fact]
    public void get_ReturnsListOfCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            CreateRandomCustomer(),
            CreateRandomCustomer(),
            CreateRandomCustomer(),
            CreateRandomCustomer(),
            CreateRandomCustomer(),
            CreateRandomCustomer()
        };
        customerRepositoryStub.Setup(repo => repo.GetList(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(customers);
        var controller = new CustomerController(customerRepositoryStub.Object);

        // Act
        var result = controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Equal(customers.Count, returnedCustomers.Count);

    }


    [Fact]
    public void UpdateCustomer_ExistingCustomer_ReturnsResult()
    {
        // Arrange
        var existingCustomer = CreateRandomCustomer();
        var updatedCustomer = new CustomerDTO
        {
            Id = existingCustomer.Id,
            Name = "updatedName",
            Email = "updatedEmail",
            AddressId = existingCustomer.AddressId
        };

        customerRepositoryStub.Setup(repo => repo.Get(It.IsAny<Expression<Func<Customer, bool>>>()))
       .Returns(existingCustomer);
        var controller = new CustomerController(customerRepositoryStub.Object);

        // Act
        var result = controller.Update(updatedCustomer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updated = Assert.IsType<bool>(okResult.Value);
        Assert.True(updated);
    }

    [Fact]
    public void ValidateCustomer_ExistingCustomer_ReturnsResultWithTrue()
    {
        // Arrange
        var existingCustomer = CreateRandomCustomer();
        var requestBody = new IdDTO
        {
            Id = existingCustomer.Id
        };
        customerRepositoryStub.Setup(repo => repo.Get(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(existingCustomer);
        var controller = new CustomerController(customerRepositoryStub.Object);

        // Act
        var result = controller.Validate(requestBody);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var isValid = Assert.IsType<bool>(okResult.Value);
        Assert.True(isValid);
    }

    [Fact]
    public void DeleteCustomer_ExistingCustomer_ReturnsResult()
    {
        // Arrange
        var existingCustomer = CreateRandomCustomer();
        var requestBody = new IdDTO
        {
            Id = existingCustomer.Id
        };
        customerRepositoryStub.Setup(repo => repo.Get(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(existingCustomer);
        var controller = new CustomerController(customerRepositoryStub.Object);

        // Act
        var result = controller.Delete(requestBody);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var isDeleted = Assert.IsType<bool>(okResult.Value);
        Assert.True(isDeleted);
    }


    private Customer CreateRandomCustomer()
    {
        DateTime now = DateTime.UtcNow;
        return new()
        {
            Id = Guid.NewGuid(),
            Name = "RandomName",
            Email = "example@example.com",
            AddressId = Guid.NewGuid(),
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}

