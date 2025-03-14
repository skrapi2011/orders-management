using Microsoft.EntityFrameworkCore;
using OrdersManagement.data;
using OrdersManagement.Models;
using OrdersManagement.Models.Enums;
using OrdersManagement.Repositories;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace OrdersManagement.Tests;

/// <summary>
/// Represents order repository tests.
/// </summary>
public class OrderRepositoryTests
{
    private static DbContextOptions<OrdersDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
    

    /// <summary>
    /// Tests getting all orders.
    /// </summary>
    [Fact]
    public async Task GetOrderByIdAsync_ExistingOrder_ReturnsOrder()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var testOrder = new Order
        {
            Id = Guid.NewGuid(),
            ProductName = "Test Product",
            Amount = 100m,
            CustomerType = CustomerType.Person,
            DeliveryAddress = "123 Test St",
            PaymentMethod = PaymentMethod.Card,
            OrderStatus = OrderStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        await using (var context = new OrdersDbContext(options))
        {
            await context.Orders.AddAsync(testOrder);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new OrdersDbContext(options))
        {
            var repository = new OrderRepository(context);
            var result = await repository.GetOrderByIdAsync(testOrder.Id);
            Assert.Multiple(() =>
            {

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(testOrder.Id, Is.EqualTo(result!.Id));
                Assert.That(testOrder.ProductName, Is.EqualTo(result.ProductName));
            });
        }
    }
}