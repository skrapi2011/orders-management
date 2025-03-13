using OrdersManagement.Models;

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
    public List<Order> GetOrders();
    
    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="order">Order object to be created</param>
    /// <returns>Order object</returns>
    public Order CreateOrder(Order order);
    
}