namespace OrdersManagement.ErrorCodes;

/// <summary>
/// Contains error codes.
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Error code for when the amount is too low.
    /// </summary>
    public const string AmountTooLow = "Amount is too low.";
    
    /// <summary>
    /// Error code for when the amount is required.
    /// </summary>
    public const string AmountRequired = "Amount is required.";
    
    /// <summary>
    /// Error code for when the product name is required.
    /// </summary>
    public const string ProductNameRequired = "Product name is required.";
    
    /// <summary>
    /// Error code for when the customer type is required.
    /// </summary>
    public const string CustomerTypeRequired = "Customer type is required.";
    
    /// <summary>
    /// Error code for when the payment method is required.
    /// </summary>
    public const string PaymentMethodRequired = "Payment method is required.";
}