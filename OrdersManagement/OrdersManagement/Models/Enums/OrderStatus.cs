namespace OrdersManagement.Models.Enums;

/// <summary>
/// Represents the status of an order.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// The order is pending.
    /// </summary>
    New = 1,
    
    /// <summary>
    /// The order is in stock.
    /// </summary>
    InStock = 2,
    
    /// <summary>
    /// The order is in shipping.
    /// </summary>
    InShipping = 3,
    
    /// <summary>
    /// The order is returned to the customer.
    /// </summary>
    ReturnedToCustomer = 4,
    
    /// <summary>
    /// The order has an error.
    /// </summary>
    Error = 5,
    
    /// <summary>
    /// The order is closed.
    /// </summary>
    Closed = 6
    
}