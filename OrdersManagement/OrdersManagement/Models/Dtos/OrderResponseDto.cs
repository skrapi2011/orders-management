using OrdersManagement.Models.Enums;

namespace OrdersManagement.Models.Dtos;

/// <summary>
/// Represents an order response.
/// </summary>
public class OrderResponseDto
{
    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public required Guid Id { get; set; }
    
    /// <summary>
    /// Amount of the order.
    /// </summary>
    public required decimal Amount { get; set; }
    
    /// <summary>
    /// Name of the product.
    /// </summary>
    public required string ProductName { get; set; }
    
    /// <summary>
    /// Type of the customer.
    /// </summary>
    public required CustomerType CustomerType { get; set; }
    
    /// <summary>
    /// Delivery address.
    /// </summary>
    public required string DeliveryAddress { get; set; }
    
    /// <summary>
    /// Payment method.
    /// </summary>
    public required PaymentMethod PaymentMethod { get; set; }
    
    /// <summary>
    /// Status of the order.
    /// </summary>
    public required OrderStatus OrderStatus { get; set; }
    
    /// <summary>
    /// Date and time when the order was created.
    /// </summary>
    public required DateTime CreatedAt { get; set; }
}