using OrdersManagement.Models.Enums;

namespace OrdersManagement.Models.Dtos;

/// <summary>
/// Represents an order create DTO.
/// </summary>
public class OrderCreateDto
{
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
}