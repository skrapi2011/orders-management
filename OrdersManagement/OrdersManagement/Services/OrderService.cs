using System.ComponentModel.DataAnnotations;
using OrdersManagement.Models;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Models.Validation;
using OrdersManagement.Repositories;
using OrdersManagement.Repositories.Interfaces;
using OrdersManagement.Services.Interfaces;

namespace OrdersManagement.Services;

/// <summary>
/// Represents an order service.
/// </summary>
public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>List of order response objects</returns>
    public async Task<Result<List<OrderResponseDto>>> GetOrdersAsync()
    {
        var orders = await orderRepository.GetOrdersAsync();
        
        var ordersResponse = orders.Select(o => new OrderResponseDto
        {
            Id = o.Id,
            Amount = o.Amount,
            ProductName = o.ProductName,
            CustomerType = o.CustomerType,
            DeliveryAddress = o.DeliveryAddress,
            PaymentMethod = o.PaymentMethod,
            OrderStatus = o.OrderStatus,
            CreatedAt = o.CreatedAt
        }).ToList();
        
        return Result<List<OrderResponseDto>>.Success(ordersResponse);
    }

    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="orderDto">Order object to be created</param>
    /// <returns>Order response object</returns>
    public async Task<Result<OrderResponseDto>> CreateOrderAsync(OrderCreateDto orderDto)
    {
        var validationContext = new ValidationContext(orderDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(orderDto, validationContext, validationResults, true);

        if (isValid)
        {
            var order = new Order()
            {
                Amount = orderDto.Amount,
                ProductName = orderDto.ProductName,
                CustomerType = orderDto.CustomerType,
                DeliveryAddress = orderDto.DeliveryAddress,
                PaymentMethod = orderDto.PaymentMethod,
            };
            
            var createdOrder = await orderRepository.CreateOrderAsync(order);
            
            return Result<OrderResponseDto>.Success(new OrderResponseDto
            {
                Id = createdOrder.Id,
                Amount = createdOrder.Amount,
                ProductName = createdOrder.ProductName,
                CustomerType = createdOrder.CustomerType,
                DeliveryAddress = createdOrder.DeliveryAddress,
                PaymentMethod = createdOrder.PaymentMethod,
                OrderStatus = createdOrder.OrderStatus,
                CreatedAt = createdOrder.CreatedAt
            });
        }
        return Result<OrderResponseDto>.Failure(validationResults);
        
    }
}