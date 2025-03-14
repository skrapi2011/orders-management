using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrdersManagement.data;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Repositories;
using OrdersManagement.Repositories.Interfaces;
using OrdersManagement.Services;
using OrdersManagement.Services.Interfaces;
using OrdersManagement.UI;

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
                    ProductName = "Laptop",
                    CustomerType = Models.Enums.CustomerType.Person,
                    DeliveryAddress = "123 Main St, Springfield, IL",
                    PaymentMethod = Models.Enums.PaymentMethod.Card
                };
        
                // creating example order to seed the database
                // var createResult = await orderService.CreateOrderAsync(orderDto);
                var menuManager = new MenuManager(orderService);
                await menuManager.RunAsync();
            }
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <returns>Service provider</returns>
        static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            
            var baseDirectory = AppContext.BaseDirectory;
            var projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "../../../"));
            var dataDirectory = Path.Combine(projectDirectory, "Data");

            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(dataDirectory, "database.db")}"));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            return services.BuildServiceProvider();
        }
    }
}