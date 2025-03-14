using Microsoft.EntityFrameworkCore;
using OrdersManagement.data;
using OrdersManagement.Models;
using OrdersManagement.Models.Enums;
using OrdersManagement.Models.Validation;
using OrdersManagement.Repositories.Interfaces;

namespace OrdersManagement.Repositories;

/// <summary>
/// Represents an order repository.
/// </summary>
public class OrderRepository(OrdersDbContext context) : IOrderRepository
{
    /// <summary>
    /// Gets all orders
    /// </summary>
    /// <returns></returns>
    public async Task<List<Order>> GetOrdersAsync()
    {
        var orders = await context.Orders.ToListAsync();

        return orders;
    }
    
    public async Task<Order> CreateOrderAsync(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
        
        return order;
    }

    public async Task<Order> ChangeOrderStatusAsync(Guid orderId, OrderStatus orderStatus)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new KeyNotFoundException($"Order with id {orderId} not found.");
        
        order.OrderStatus = orderStatus;
        await context.SaveChangesAsync();

        return order;
    }



}