using System.ComponentModel.DataAnnotations;
using OrdersManagement.Models;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Models.Validation;
using OrdersManagement.Repositories;
using OrdersManagement.Services.Interfaces;

namespace OrdersManagement.Services;

/// <summary>
/// Represents an order service.
/// </summary>
public class OrderService(OrderRepository orderRepository) : IOrderService
{
    
    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>List of order response objects</returns>
    public List<OrderResponseDto> GetOrders()
    {
        var orders = orderRepository.GetOrders();
         
        var orderResponseDtos = orders.Select(order => new OrderResponseDto
        {
            Id = order.Id,
            Amount = order.Amount,
            ProductName = order.ProductName,
            CustomerType = order.CustomerType,
            DeliveryAddress = order.DeliveryAddress,
            PaymentMethod = order.PaymentMethod,
            OrderStatus = order.OrderStatus,
            CreatedAt = order.CreatedAt
        }).ToList();

        return orderResponseDtos;
    }

    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="orderDto">Order object to be created</param>
    /// <returns>Order response object</returns>
    public Result<OrderResponseDto> CreateOrder(OrderCreateDto orderDto)
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
            
            var createdOrder = orderRepository.CreateOrder(order);
            
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