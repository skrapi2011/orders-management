using OrdersManagement.Models.Enums;

namespace OrdersManagement.Models;
using System.ComponentModel.DataAnnotations;
using ErrorCodes;

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
    [Required]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = ErrorCodes.AmountTooLow)]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Name of the product.
    /// </summary>
    [Required(ErrorMessage = ErrorCodes.NameRequired)]
    public string ProductName { get; set; }
    
    /// <summary>
    /// Type of the customer.
    /// </summary>
    [Required]
    public CustomerType CustomerType { get; set; }
    
    /// <summary>
    /// Delivery address.
    /// </summary>
    public string DeliveryAddress { get; set; }
    
    /// <summary>
    /// Payment method.
    /// </summary>
    [Required]
    public PaymentMethod PaymentMethod { get; set; }
    
    /// <summary>
    /// Status of the order.
    /// </summary>
    public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
    
    /// <summary>
    /// Date and time when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}