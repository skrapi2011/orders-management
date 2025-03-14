using OrdersManagement.Models;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Models.Validation;

namespace OrdersManagement.Services.Interfaces;

/// <summary>
/// Represents an order service.
/// </summary>
public interface IOrderService
{

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>List of order response objects</returns>
    public Task<Result<List<OrderResponseDto>>> GetOrdersAsync();
    
    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="order">Order object to be created</param>
    /// <returns>Order response object</returns>
    public Task<Result<OrderResponseDto>> CreateOrderAsync(OrderCreateDto order);
    
    /// <summary>
    /// Moves an order to the warehouse.
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>Result containing the updated order information</returns>
    public Task<Result<OrderResponseDto>> MoveToWarehouseAsync(Guid orderId);

    /// <summary>
    /// Ships an order.
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>Result containing the updated order information</returns>
    public Task<Result<OrderResponseDto>> ShipOrderAsync(Guid orderId);

    /// <summary>
    /// Completes the shipping process and closes the order.
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>Result containing the updated order information</returns>
    public Task<Result<OrderResponseDto>> CloseOrderAsync(Guid orderId);
}