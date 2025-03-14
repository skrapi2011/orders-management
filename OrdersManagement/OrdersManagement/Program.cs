using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrdersManagement.data;
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