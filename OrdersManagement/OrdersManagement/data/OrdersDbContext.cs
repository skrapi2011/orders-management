using Microsoft.EntityFrameworkCore;
using OrdersManagement.Models;

namespace OrdersManagement.data;

/// <summary>
/// Represents the database context of the orders.
/// </summary>
/// <param name="options"> The options of the database context. </param>
public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Represents the orders table.
    /// </summary>
    public DbSet<Order> Orders { get; set; }
    
    /// <summary>
    /// Configures the model of the database.
    /// </summary>
    /// <param name="modelBuilder"> The model builder. </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
        
            entity.Property(o => o.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
        
            entity.Property(o => o.ProductName)
                .IsRequired()
                .HasMaxLength(100);
        
            entity.Property(o => o.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.HasIndex(o => o.OrderStatus);
            
            entity.Property(o => o.CustomerType).IsRequired();
            entity.Property(o => o.PaymentMethod).IsRequired();
            entity.Property(o => o.CreatedAt).IsRequired();
        });
    }
}