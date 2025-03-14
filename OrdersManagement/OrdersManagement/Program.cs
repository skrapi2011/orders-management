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
        
                var result = await orderService.CreateOrderAsync(orderDto);
                Console.WriteLine($"Order created: {!result.IsError}");
                
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
            string dataDirectory = Path.Combine(projectDirectory, "data");

            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(dataDirectory, "database.db")}"));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            return services.BuildServiceProvider();
        }
    }
}