using OrdersManagement.Services.Interfaces;

namespace OrdersManagement.UI;

/// <summary>
/// Represents a menu manager.
/// </summary>
/// <param name="orderService">Order service</param>
public class MenuManager(IOrderService orderService)
{
    private readonly IOrderService _orderService = orderService;

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
    private static void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("============================================");
        Console.WriteLine("=          ORDER MANAGEMENT SYSTEM         =");
        Console.WriteLine("============================================");
        Console.WriteLine("=  1. Create Sample Order                  =");
        Console.WriteLine("=  2. Move Order to Warehouse              =");
        Console.WriteLine("=  3. Ship Order                           =");
        Console.WriteLine("=  4. View Orders                          =");
        Console.WriteLine("=  5. Search Order by ID                   =");
        Console.WriteLine("=  6. Close Order                          =");
        Console.WriteLine("=  7. Exit                                 =");
        Console.WriteLine("============================================");
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
        Console.WriteLine("============================================");
        Console.WriteLine("=          CREATE SAMPLE ORDER             =");
        Console.WriteLine("============================================");
        
        // TODO: Implement this method
        
        WaitForInput();
    }
    
    /// <summary>
    /// Searches an order by ID.
    /// </summary>
    private static async Task SearchOrderByIdAsync()
    {
        Console.Clear();
        Console.WriteLine("============================================");
        Console.WriteLine("=          SEARCH ORDER BY ID              =");
        Console.WriteLine("============================================");

        // TODO: Implement this method

        WaitForInput();
    }

    /// <summary>
    /// Closes an order.
    /// </summary>
    private static async Task CloseOrderAsync()
    {
        Console.Clear();
        Console.WriteLine("============================================");
        Console.WriteLine("=             CLOSE ORDER                  =");
        Console.WriteLine("============================================");

        // TODO: Implement this method

        WaitForInput();
    }

    /// <summary>
    /// Moves an order to warehouse.
    /// </summary>
    private static async Task MoveOrderToWarehouseAsync()
    {
        // TODO: Implement this method
    }

    /// <summary>
    /// Ships an order.
    /// </summary>
    private static async Task ShipOrderAsync()
    {
        // TODO: Implement this method
    }

    /// <summary>
    /// Views orders.
    /// </summary>
    private static async Task ViewOrdersAsync()
    {
        // TODO: Implement this method
    }
}