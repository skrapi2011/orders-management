using OrdersManagement.Models.Dtos;
using OrdersManagement.Models.Enums;
using OrdersManagement.Services.Interfaces;

namespace OrdersManagement.UI;

/// <summary>
/// Represents a menu manager.
/// </summary>
/// <param name="orderService">Order service</param>
public class MenuManager(IOrderService orderService)
{
    /// <summary>
    /// Initiates the menu manager.
    /// </summary>
    public async Task RunAsync()
    {
        var exit = false;

        while (!exit)
        {
            Console.Clear();
            DisplayMainMenu();
            var input = Console.ReadLine() ?? string.Empty;

            switch (input)
            {
                case "1":
                    await CreateSampleOrderAsync();
                    break;
                case "2":
                    await MoveOrderToWarehouseAsync();
                    break;
                case "3":
                    await ShipOrderAsync();
                    break;
                case "4":
                    await ViewOrdersAsync();
                    break;
                case "5":
                    await SearchOrderByIdAsync();
                    break;
                case "6":
                    await ViewOrderStatisticsAsync();
                    break;
                case "7":
                    await CloseOrderAsync();
                    break;
                case "8":
                    exit = true;
                    break;
                default:
                    DisplayInvalidOption();
                    break;
            }
        }
    }
    
    /// <summary>
    /// Displays the main menu.
    /// </summary>
    private static async void DisplayMainMenu()
    {
        Console.Clear();
        await Task.Delay(300);
        Console.WriteLine("==============================================");
        Console.WriteLine("=           ORDER MANAGEMENT SYSTEM          =");
        Console.WriteLine("==============================================");
        Console.WriteLine("=  1. Create Order                           =");
        Console.WriteLine("=  2. Move Order to Warehouse                =");
        Console.WriteLine("=  3. Move Order to Shipping                 =");
        Console.WriteLine("=  4. View All Orders                        =");
        Console.WriteLine("=  5. Search Order by ID                     =");
        Console.WriteLine("=  6. Show Orders Statistics                 =");
        Console.WriteLine("=  7. Close Order                            =");
        Console.WriteLine("=  8. Exit                                   =");
        Console.WriteLine("==============================================");
        Console.Write("Enter your choice (1-7): ");
    }

    /// <summary>
    /// Waits for user input.
    /// </summary>
    private static void WaitForInput()
    {
        Console.WriteLine("\nPress any key to return to main menu...");
        Console.ReadKey();
    }

    /// <summary>
    /// Displays an invalid option message.
    /// </summary>
    private static void DisplayInvalidOption()
    {
        Console.WriteLine("\nInvalid option. Press any key to try again...");
        Console.ReadKey();
    }

    /// <summary>
    /// Creates a sample order.
    /// </summary>
    private async Task CreateSampleOrderAsync()
    {
        var isFormComplete = false;
        
        decimal amount = 0;
        var productName = string.Empty;
        var customerType = CustomerType.Person;
        var deliveryAddress = string.Empty;
        var paymentMethod = PaymentMethod.Card;
        
        while (!isFormComplete)
        {
            Console.Clear();
            await Task.Delay(300);
            Console.WriteLine("==============================================");
            Console.WriteLine("=                CREATE ORDER                =");
            Console.WriteLine("==============================================");
            
            // Name
            Console.Write("Enter Product Name: ");
            productName = Console.ReadLine() ?? string.Empty;
            
            // Amount
            Console.Write("Enter Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Invalid amount format. Please enter a valid number.");
                WaitForInput();
                continue;
            }
            
            // Customer type
            Console.WriteLine("Select Customer Type:");
            Console.WriteLine("1. Person");
            Console.WriteLine("2. Company");
            Console.Write("Enter choice (1-2): ");

            string customerChoice;
            var validCustomerChoice = false;
            do
            {
                customerChoice = Console.ReadLine() ?? string.Empty;
    
                if (customerChoice == "1" || customerChoice == "2")
                {
                    validCustomerChoice = true;
                }
                else
                {
                    Console.Write("Invalid choice. Please enter 1 or 2: ");
                }
            } while (!validCustomerChoice);

            customerType = customerChoice == "1" ? CustomerType.Person : CustomerType.Company;
            
            // Delivery address
            Console.Write("Enter Delivery Address: ");
            deliveryAddress = Console.ReadLine() ?? string.Empty;
            
            // Payment method
            Console.WriteLine("Select Payment Method:");
            Console.WriteLine("1. Card");
            Console.WriteLine("2. Cash On Delivery");
            Console.Write("Enter choice (1-2): ");

            string paymentChoice;
            var validPaymentChoice = false;
            do
            {
                paymentChoice = Console.ReadLine() ?? string.Empty;
    
                if (paymentChoice is "1" or "2")
                {
                    validPaymentChoice = true;
                }
                else
                {
                    Console.Write("Invalid choice. Please enter 1 or 2: ");
                }
            } while (!validPaymentChoice);

            paymentMethod = paymentChoice == "1" ? PaymentMethod.Card : PaymentMethod.CashOnDelivery;
            
            // Review information
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine("=              ORDER SUMMARY                 =");
            Console.WriteLine("==============================================");
            Console.WriteLine($"Product: {productName}");
            Console.WriteLine($"Amount: {amount:C}");
            Console.WriteLine($"Customer Type: {customerType}");
            Console.WriteLine($"Delivery Address: {deliveryAddress}");
            Console.WriteLine($"Payment Method: {paymentMethod}");
            Console.WriteLine("==============================================");
            
            Console.Write("Is provided information correct? (Y/N): ");
            var confirmation = Console.ReadLine()?.ToUpper();
            isFormComplete = confirmation == "Y";
        }
        
        // Dto
        var orderDto = new OrderCreateDto
        {
            ProductName = productName,
            Amount = amount,
            CustomerType = customerType,
            DeliveryAddress = deliveryAddress,
            PaymentMethod = paymentMethod
        };
        
        // Submit order
        var result = await orderService.CreateOrderAsync(orderDto);
        
        if (result.IsError)
        {
            Console.WriteLine("\nFailed to create order:");
            foreach (var error in result.ErrorCodes)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
        }
        else
        {
            Console.WriteLine("\nOrder created successfully:");
            Console.WriteLine($"Order ID: {result.Value!.Id}");
            Console.WriteLine($"Product: {result.Value.ProductName}");
            Console.WriteLine($"Status: {result.Value.OrderStatus}");
            Console.WriteLine($"Created At: {result.Value.CreatedAt}");
        }
        
        WaitForInput();
    }
    
    /// <summary>
    /// Searches an order by ID.
    /// </summary>
    private async Task SearchOrderByIdAsync()
    {
        Console.Clear();
        await Task.Delay(300);
        Console.WriteLine("==============================================");
        Console.WriteLine("=             SEARCH ORDER BY ID             =");
        Console.WriteLine("==============================================");

        Console.Write("Enter Order ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var orderId))
        {
            Console.WriteLine("Invalid Order ID format.");
            WaitForInput();
            return;
        }

        var result = await orderService.GetOrderByIdAsync(orderId);

        if (result.IsError)
        {
            Console.WriteLine($"Error: {string.Join(", ", result.ErrorCodes.Select(e => e.ErrorMessage))}");
        }
        else
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Order details:");
            Console.WriteLine($"Order ID: {result.Value!.Id}");
            Console.WriteLine($"Product Name: {result.Value.ProductName}");
            Console.WriteLine($"Amount: {result.Value.Amount:C}");
            Console.WriteLine($"Customer Type: {result.Value.CustomerType}");
            Console.WriteLine($"Delivery Address: {result.Value.DeliveryAddress}");
            Console.WriteLine($"Payment Method: {result.Value.PaymentMethod}");
            Console.WriteLine($"Order Status: {result.Value.OrderStatus}");
            Console.WriteLine($"Created At: {result.Value.CreatedAt}");
            Console.WriteLine("==============================================");
        }

        WaitForInput();
    }

    /// <summary>
    /// Closes an order.
    /// </summary>
    private async Task CloseOrderAsync()
    {
        Console.Clear();
        await Task.Delay(300);
        Console.WriteLine("==============================================");
        Console.WriteLine("=                CLOSE ORDER                 =");
        Console.WriteLine("==============================================");

        Console.Write("Enter Order ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var orderId))
        {
            Console.WriteLine("Invalid Order ID format.");
            WaitForInput();
            return;
        }

        var result = await orderService.CloseOrderAsync(orderId);

        if (result.IsError)
        {
            Console.WriteLine($"Error: {string.Join(", ", result.ErrorCodes.Select(e => e.ErrorMessage))}");
        }
        else
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Order successfully closed:");
            Console.WriteLine($"Order ID: {result.Value!.Id}");
            Console.WriteLine($"Product Name: {result.Value.ProductName}");
            Console.WriteLine($"Order Status: {result.Value.OrderStatus}");
            Console.WriteLine("==============================================");
        }

        WaitForInput();
    }

    /// <summary>
    /// Moves an order to warehouse.
    /// </summary>
    private async Task MoveOrderToWarehouseAsync()
    {
        Console.Clear();
        await Task.Delay(300);
        Console.WriteLine("=============================================");
        Console.WriteLine("=          MOVE ORDER TO WAREHOUSE          =");
        Console.WriteLine("=============================================");
    
        Console.Write("Enter Order ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var orderId))
        {
            Console.WriteLine("Invalid Order ID format.");
            WaitForInput();
            return;
        }
    
        var result = await orderService.MoveToWarehouseAsync(orderId);
    
        if (result.IsError)
        {
            Console.WriteLine($"Error: {string.Join(", ", result.ErrorCodes.Select(e => e.ErrorMessage))}");
        }
        else
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Order successfully moved to warehouse:");
            Console.WriteLine($"Order ID: {result.Value!.Id}");
            Console.WriteLine($"Product Name: {result.Value.ProductName}");
            Console.WriteLine($"Order Status: {result.Value.OrderStatus}");
            Console.WriteLine("==============================================");
        }
    
        WaitForInput();
    }

    /// <summary>
    /// Ships an order.
    /// </summary>
    private async Task ShipOrderAsync()
    {
        Console.Clear();
        await Task.Delay(300);
        Console.WriteLine("==============================================");
        Console.WriteLine("=                 SHIP ORDER                 =");
        Console.WriteLine("==============================================");

        Console.Write("Enter Order ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out var orderId))
        {
            Console.WriteLine("Invalid Order ID format.");
            WaitForInput();
            return;
        }

        var result = await orderService.ShipOrderAsync(orderId);

        if (result.IsError)
        {
            Console.WriteLine($"Error: {string.Join(", ", result.ErrorCodes.Select(e => e.ErrorMessage))}");
        }
        else
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Order shipping process initiated:");
            Console.WriteLine($"Order ID: {result.Value!.Id}");
            Console.WriteLine($"Product Name: {result.Value.ProductName}");
            Console.WriteLine($"Current Status: {result.Value.OrderStatus}");
            Console.WriteLine("Status will update to InShipping within 5 seconds.");
            Console.WriteLine("==============================================");
        }

        WaitForInput();
    }

    /// <summary>
    /// Views orders.
    /// </summary>
    private async Task ViewOrdersAsync()
    {
        Console.Clear();
        await Task.Delay(300);
        Console.WriteLine("==============================================");
        Console.WriteLine("=                VIEW ORDERS                 =");
        Console.WriteLine("==============================================");

        var ordersResult = await orderService.GetOrdersAsync();
        
        if (!ordersResult.IsError)
        {
            var orders = ordersResult.Value;
            if (orders is { Count: 0 })
            {
                Console.WriteLine("No orders found.");
            }
            else
            {
                if (orders != null)
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"Order ID: {order.Id}");
                        Console.WriteLine($"Product Name: {order.ProductName}");
                        Console.WriteLine($"Amount: {order.Amount}");
                        Console.WriteLine($"Customer Type: {order.CustomerType}");
                        Console.WriteLine($"Delivery Address: {order.DeliveryAddress}");
                        Console.WriteLine($"Payment Method: {order.PaymentMethod}");
                        Console.WriteLine($"Order Status: {order.OrderStatus}");
                        Console.WriteLine($"Created At: {order.CreatedAt}");
                        Console.WriteLine("==============================================");
                    }
            }
        }
        else
        {
            Console.WriteLine("Failed to retrieve orders.");
        }

        WaitForInput();
    }
    
    /// <summary>
    /// Displays order statistics and reporting.
    /// </summary>
    private async Task ViewOrderStatisticsAsync()
    {
        Console.Clear();
        var orders = (await orderService.GetOrdersAsync()).Value;
    
        Console.WriteLine("==============================================");
        Console.WriteLine("=             ORDER STATISTICS               =");
        Console.WriteLine("==============================================");
    
        if (orders == null || orders.Count == 0)
        {
            Console.WriteLine("No orders available for statistics.");
            Console.WriteLine("==============================================");
            WaitForInput();
            return;
        }
    
        // Status distribution
        var byStatus = orders.GroupBy(o => o.OrderStatus)
            .Select(g => new { Status = g.Key, Count = g.Count() });
        Console.WriteLine("Orders by Status:");
        foreach (var group in byStatus)
            Console.WriteLine($"- {group.Status}: {group.Count}");
        Console.Write("==============================================");
    
        // Customer type distribution
        var byCustomer = orders.GroupBy(o => o.CustomerType)
            .Select(g => new { Type = g.Key, Count = g.Count() });
        Console.WriteLine("\nOrders by Customer Type:");
        foreach (var group in byCustomer)
            Console.WriteLine($"- {group.Type}: {group.Count}");
        Console.Write("==============================================");
    
        // Revenue statistics
        Console.WriteLine($"\nTotal Revenue: {orders.Sum(o => o.Amount):C}");
        Console.WriteLine("==============================================");
        Console.WriteLine($"Average Order Value: {orders.Average(o => o.Amount):C}");
        Console.WriteLine("==============================================");
    
        WaitForInput();
    }
}