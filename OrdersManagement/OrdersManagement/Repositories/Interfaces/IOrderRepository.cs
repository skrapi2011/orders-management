using OrdersManagement.Models;
using OrdersManagement.Models.Enums;

namespace OrdersManagement.Repositories.Interfaces;

/// <summary>
/// Represents an order repository.
/// </summary>
public interface IOrderRepository
{
    
    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>List of orders</returns>
    public Task<List<Order>> GetOrdersAsync();
    
    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="order">Order object to be created</param>
    /// <returns>Order object</returns>
    public Task<Order> CreateOrderAsync(Order order);
    
    /// <summary>
    /// Changes the status of an order.
    /// </summary>
    /// <param name="orderId">Unique identifier of the order</param>
    /// <param name="orderStatus">New status of the order</param>
    /// <returns>Order object</returns>
    public Task<Order> ChangeOrderStatusAsync(Guid orderId, OrderStatus orderStatus);
    
}