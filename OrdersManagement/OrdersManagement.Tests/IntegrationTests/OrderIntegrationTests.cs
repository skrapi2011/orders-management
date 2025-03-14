using Microsoft.EntityFrameworkCore;
using OrdersManagement.data;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Models.Enums;
using OrdersManagement.Repositories;
using OrdersManagement.Services;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace OrdersManagement.Tests;

/// <summary>
/// Represents order integration tests.
/// </summary>
public class OrderIntegrationTests
{
    private readonly OrderService _service;

    public OrderIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase(databaseName: "OrderIntegrationTest")
            .Options;
        
        using var context = new OrdersDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var repository = new OrderRepository(new OrdersDbContext(options));
        _service = new OrderService(repository);
    }

    /// <summary>
    /// Tests the order flow from creation to closing.
    /// </summary>
    [Fact]
    public async Task OrderFlow_CreationToClosing_CompletesSuccessfully()
    {
        var orderDto = new OrderCreateDto
        {
            ProductName = "Flow Test Product",
            Amount = 150m,
            CustomerType = CustomerType.Person,
            DeliveryAddress = "456 Flow St",
            PaymentMethod = PaymentMethod.Card
        };
        
        // Asserting that Status == New
        var createResult = await _service.CreateOrderAsync(orderDto);
        Assert.Multiple(() =>
        {
            Assert.That(createResult.IsError, Is.False);
            Assert.That(createResult.Value!.OrderStatus, Is.EqualTo(OrderStatus.New));
        });
        if (createResult.Value != null)
        {
            var orderId = createResult.Value.Id;
            
            // Asserting that Status == InStock
            var warehouseResult = await _service.MoveToWarehouseAsync(orderId);
            Assert.Multiple(() =>
            {
                Assert.That(warehouseResult.IsError, Is.False);
                Assert.That(warehouseResult.Value!.OrderStatus, Is.EqualTo(OrderStatus.InStock));
            });
            
            // Move to shipping
            var shipResult = await _service.ShipOrderAsync(orderId);
            Assert.That(shipResult.IsError, Is.False);
            
            await Task.Delay(6000);
        
            // Asserting that Status == InShipping
            var checkResult = await _service.GetOrderByIdAsync(orderId);
            Assert.That(checkResult.Value!.OrderStatus, Is.EqualTo(OrderStatus.InShipping));

            // Asserting that Status == Closed
            var closeResult = await _service.CloseOrderAsync(orderId);
            Assert.Multiple(() =>
            {
                Assert.That(closeResult.IsError, Is.False);
                Assert.That(closeResult.Value!.OrderStatus, Is.EqualTo(OrderStatus.Closed));
            });
        }
    }
}