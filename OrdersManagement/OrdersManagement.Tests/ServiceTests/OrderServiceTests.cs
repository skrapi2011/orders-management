using Moq;
using OrdersManagement.Models;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Models.Enums;
using OrdersManagement.Repositories.Interfaces;
using OrdersManagement.Services;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace OrdersManagement.Tests;

/// <summary>
/// Represents order service tests.
/// </summary>
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepository;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _orderService = new OrderService(_mockRepository.Object);
    }
    
    /// <summary>
    /// Business logic case test for moving an expensive order to the warehouse.
    /// </summary>
    [Fact]
    public async Task MoveToWarehouseAsync_CashOnDeliveryOverLimit_ReturnsToCustomer()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            ProductName = "Expensive Item",
            Amount = 3000m,
            CustomerType = CustomerType.Person,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            DeliveryAddress = "Test Address",
            OrderStatus = OrderStatus.New
        };

        _mockRepository.Setup(r => r.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        _mockRepository.Setup(r => r.ChangeOrderStatusAsync(orderId, OrderStatus.ReturnedToCustomer))
            .ReturnsAsync(new Order { 
                Id = orderId,
                ProductName = "Expensive Item",
                Amount = 3000m,
                CustomerType = CustomerType.Person,
                PaymentMethod = PaymentMethod.CashOnDelivery,
                DeliveryAddress = "Test Address",
                OrderStatus = OrderStatus.ReturnedToCustomer
            });

        // Act
        var result = await _orderService.MoveToWarehouseAsync(orderId);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value!.OrderStatus, Is.EqualTo(OrderStatus.ReturnedToCustomer));
        });
    }

    /// <summary>
    /// Tests creating an order.
    /// </summary>
    [Fact]
    public async Task CreateOrderAsync_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        var orderDto = new OrderCreateDto
        {
            ProductName = "Test Product",
            Amount = 100m,
            CustomerType = CustomerType.Person,
            DeliveryAddress = "123 Test St",
            PaymentMethod = PaymentMethod.Card
        };

        var createdOrder = new Order
        {
            Id = Guid.NewGuid(),
            ProductName = orderDto.ProductName,
            Amount = orderDto.Amount,
            CustomerType = orderDto.CustomerType,
            DeliveryAddress = orderDto.DeliveryAddress,
            PaymentMethod = orderDto.PaymentMethod,
            OrderStatus = OrderStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.CreateOrderAsync(It.IsAny<Order>()))
            .ReturnsAsync(createdOrder);

        // Act
        var result = await _orderService.CreateOrderAsync(orderDto);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value!.Id, Is.EqualTo(createdOrder.Id));
            Assert.That(result.Value.OrderStatus, Is.EqualTo(OrderStatus.New));
        });
    }

    /// <summary>
    /// Tests creating an order with an empty address.
    /// </summary>
    [Fact]
    public async Task CreateOrderAsync_WithEmptyAddress_SetsErrorStatus()
    {
        // Arrange
        var orderDto = new OrderCreateDto
        {
            ProductName = "Test Product",
            Amount = 100m,
            CustomerType = CustomerType.Person,
            DeliveryAddress = "",
            PaymentMethod = PaymentMethod.Card
        };

        _mockRepository.Setup(r => r.CreateOrderAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => o);

        // Act
        var result = await _orderService.CreateOrderAsync(orderDto);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value!.OrderStatus, Is.EqualTo(OrderStatus.Error));
        });
    }
}