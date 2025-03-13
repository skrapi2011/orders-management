using OrdersManagement.Models.Enums;

namespace OrdersManagement.Models;


/// <summary>
/// Represents an order.
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
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
    public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
    
    /// <summary>
    /// Date and time when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}