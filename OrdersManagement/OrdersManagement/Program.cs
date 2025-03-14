using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrdersManagement.data;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Repositories;
using OrdersManagement.Repositories.Interfaces;
using OrdersManagement.Services;
using OrdersManagement.Services.Interfaces;

namespace OrdersManagement
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
    
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
                await dbContext.Database.EnsureCreatedAsync();
            }
    
            using (var scope = serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        
                var orderDto = new OrderCreateDto
                {
                    Amount = 100,
                    ProductName = "Test Product",
                    CustomerType = Models.Enums.CustomerType.Person,
                    DeliveryAddress = "Address",
                    PaymentMethod = Models.Enums.PaymentMethod.Card
                };
        
                var createResult = await orderService.CreateOrderAsync(orderDto);
                Console.WriteLine($"Order created: {!createResult.IsError}");

                if (createResult.Value != null)
                {
                    var moveResult = await orderService.MoveToWarehouseAsync(createResult.Value.Id);
                    if (moveResult.Value != null)
                        Console.WriteLine(
                            $"Order moved to warehouse: {!moveResult.IsError}, {moveResult.Value.OrderStatus}");
                    
                    var shipResult = await orderService.ShipOrderAsync(createResult.Value.Id);
                    if (shipResult.Value != null)
                        Console.WriteLine(
                            $"Order shipped: {!shipResult.IsError}, {shipResult.Value.OrderStatus}, {shipResult.Value.Id}");

                    if (shipResult.Value != null)
                    {
                        Console.WriteLine("Waiting for order to be shipped...");
                        await Task.Delay(6000);
                        var getResults = await orderService.GetOrderByIdAsync(shipResult.Value.Id);
                        if (getResults.Value != null) Console.Write("New Order status "+getResults.Value.OrderStatus);
                    }
                }
            }
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <returns>Service provider</returns>
        static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            
            string baseDirectory = AppContext.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "../../../"));
            string dataDirectory = Path.Combine(projectDirectory, "Data");

            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(dataDirectory, "database.db")}"));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            return services.BuildServiceProvider();
        }
    }
}