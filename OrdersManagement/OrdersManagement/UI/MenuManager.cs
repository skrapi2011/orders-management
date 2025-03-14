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
                    await CloseOrderAsync();
                    break;
                case "7":
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
        Console.WriteLine("=  6. Close Order                            =");
        Console.WriteLine("=  7. Exit                                   =");
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
    private static async Task CreateSampleOrderAsync()
    {
        Console.Clear();
        Console.WriteLine("==============================================");
        Console.WriteLine("=            CREATE SAMPLE ORDER             =");
        Console.WriteLine("==============================================");
        
        // TODO: Implement this method
        
        WaitForInput();
    }
    
    /// <summary>
    /// Searches an order by ID.
    /// </summary>
    private static async Task SearchOrderByIdAsync()
    {
        Console.Clear();
        Console.WriteLine("==============================================");
        Console.WriteLine("=             SEARCH ORDER BY ID             =");
        Console.WriteLine("==============================================");

        // TODO: Implement this method

        WaitForInput();
    }

    /// <summary>
    /// Closes an order.
    /// </summary>
    private static async Task CloseOrderAsync()
    {
        Console.Clear();
        Console.WriteLine("==============================================");
        Console.WriteLine("=                CLOSE ORDER                 =");
        Console.WriteLine("==============================================");

        // TODO: Implement this method

        WaitForInput();
    }

    /// <summary>
    /// Moves an order to warehouse.
    /// </summary>
    private async Task MoveOrderToWarehouseAsync()
    {
        Console.Clear();
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
}