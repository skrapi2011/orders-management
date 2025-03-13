using OrdersManagement.Models.Dtos;
using OrdersManagement.Services.Interfaces;

namespace OrdersManagement.Services;

/// <summary>
/// Represents an order service.
/// </summary>
public class OrderService : IOrderService
{
    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>List of order response objects</returns>
    public List<OrderResponseDto> GetOrders()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="order">Order object to be created</param>
    /// <returns>Order response object</returns>
    public OrderResponseDto CreateOrder(OrderCreateDto order)
    {
        throw new NotImplementedException();
    }
}