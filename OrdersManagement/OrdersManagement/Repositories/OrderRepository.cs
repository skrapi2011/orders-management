using OrdersManagement.Models;
using OrdersManagement.Repositories.Interfaces;

namespace OrdersManagement.Repositories;

/// <summary>
/// Represents an order repository.
/// </summary>
public class OrderRepository : IOrderRepository
{
    /// <summary>
    /// List of orders, imitating a database.
    /// </summary>
    private List<Order> _orders = [];
    
    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>List of orders</returns>
    public List<Order> GetOrders()
    {
        return _orders;
    }
    
    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="order">Order object to be created</param>
    /// <returns>Order object</returns>
    public Order CreateOrder(Order order)
    {
        _orders.Add(order);
        return order;
    }

}