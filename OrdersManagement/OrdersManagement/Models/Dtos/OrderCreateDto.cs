using System.ComponentModel.DataAnnotations;
using OrdersManagement.Models.Enums;

namespace OrdersManagement.Models.Dtos;
using ErrorCodes;

/// <summary>
/// Represents an order create DTO.
/// </summary>
public class OrderCreateDto
{
    /// <summary>
    /// Amount of the order.
    /// </summary>
    [Required(ErrorMessage = ErrorCodes.AmountRequired)]
    [Range(0.01, double.MaxValue, ErrorMessage = ErrorCodes.AmountTooLow)]
    public required decimal Amount { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    [Required(ErrorMessage = ErrorCodes.ProductNameRequired)]
    public required string ProductName { get; set; }

    /// <summary>
    /// Type of the customer.
    /// </summary>
    [Required(ErrorMessage = ErrorCodes.CustomerTypeRequired)]
    public required CustomerType CustomerType { get; set; }

    /// <summary>
    /// Address for delivery.
    /// </summary>
    /// <remarks>
    /// Validation is skipped due to business requirements.
    /// </remarks>
    public required string DeliveryAddress { get; set; }

    /// <summary>
    /// Payment method.
    /// </summary>
    [Required(ErrorMessage = ErrorCodes.PaymentMethodRequired)]
    public required PaymentMethod PaymentMethod { get; set; }
}