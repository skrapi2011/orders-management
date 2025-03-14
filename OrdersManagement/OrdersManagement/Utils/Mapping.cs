using OrdersManagement.Models;
using OrdersManagement.Models.Dtos;

namespace OrdersManagement.Utils;

/// <summary>
/// Contains mapping methods.
/// </summary>
public abstract class Mapping
{
    /// <summary>
    /// Maps an Order to OrderResponseDto.
    /// </summary>
    /// <param name="order">Order object</param>
    /// <returns>OrderResponseDto object</returns>
    public static OrderResponseDto MapToOrderResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            Amount = order.Amount,
            ProductName = order.ProductName,
            CustomerType = order.CustomerType,
            DeliveryAddress = order.DeliveryAddress,
            PaymentMethod = order.PaymentMethod,
            OrderStatus = order.OrderStatus,
            CreatedAt = order.CreatedAt
        };
    }
}