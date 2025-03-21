﻿using System.ComponentModel.DataAnnotations;
using OrdersManagement.Models;
using OrdersManagement.Models.Dtos;
using OrdersManagement.Models.Enums;
using OrdersManagement.Models.Validation;
using OrdersManagement.Repositories.Interfaces;
using OrdersManagement.Services.Interfaces;
using static OrdersManagement.Utils.Mapping;

namespace OrdersManagement.Services;
using ErrorCodes;


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
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order to retrieve</param>
    /// <returns>Result containing the order information if found</returns>
    public async Task<Result<OrderResponseDto>> GetOrderByIdAsync(Guid orderId)
    {
        try
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return Result<OrderResponseDto>.Failure(
                    [new ValidationResult(ErrorCodes.OrderNotFound)]);

            return Result<OrderResponseDto>.Success(MapToOrderResponseDto(order));
        }
        catch (Exception ex)
        {
            return Result<OrderResponseDto>.Failure(
                [new ValidationResult(ex.Message)]);
        }
    }

    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <remarks>
    /// For business logic purposes, we allow for the creation of orders with empty address.
    /// Order with empty address will be created with status 'Error'
    /// </remarks>
    /// <param name="orderDto">Order object to be created</param>
    /// <returns>Order response object</returns>
    public async Task<Result<OrderResponseDto>> CreateOrderAsync(OrderCreateDto orderDto)
    {
        try
        {
            var validationContext = new ValidationContext(orderDto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(orderDto, validationContext, validationResults, true);

            if (!isValid) return Result<OrderResponseDto>.Failure(validationResults);
        
            var order = new Order
            {
                Amount = orderDto.Amount,
                ProductName = orderDto.ProductName,
                CustomerType = orderDto.CustomerType,
                DeliveryAddress = orderDto.DeliveryAddress,
                PaymentMethod = orderDto.PaymentMethod,
            };
        
            if (string.IsNullOrWhiteSpace(order.DeliveryAddress.Trim()))
            {
                order.OrderStatus = OrderStatus.Error;
            }
            
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
        catch (Exception ex)
        {
            return Result<OrderResponseDto>.Failure(
                [new ValidationResult(ex.Message)]);
        }
        
    }
    
    /// <summary>
    /// Moves an order to the warehouse.
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>Result containing the updated order information</returns>
    public async Task<Result<OrderResponseDto>> MoveToWarehouseAsync(Guid orderId)
    {
        try
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            switch (order)
            {
                case null:
                    return Result<OrderResponseDto>.Failure(
                        [new ValidationResult(ErrorCodes.OrderNotFound)]);
                // Business rule: Cash on delivery orders ≥ 2500 should be returned
                case { PaymentMethod: PaymentMethod.CashOnDelivery, Amount: >= 2500 }:
                    order = await orderRepository.ChangeOrderStatusAsync(orderId, OrderStatus.ReturnedToCustomer);
                    if(order is not null)
                        return Result<OrderResponseDto>.Success(MapToOrderResponseDto(order));
                    return Result<OrderResponseDto>.Failure(
                        [new ValidationResult(ErrorCodes.OrderNotFound)]);
                case { OrderStatus: OrderStatus.Closed }:
                    return Result<OrderResponseDto>.Failure(
                        [new ValidationResult(ErrorCodes.OrderAlreadyClosed)]);
            }

            // Business rule: Orders without address should be marked as error
            if (string.IsNullOrEmpty(order.DeliveryAddress))
            {
                await orderRepository.ChangeOrderStatusAsync(orderId, OrderStatus.Error);
                return Result<OrderResponseDto>.Failure(
                    [new ValidationResult(ErrorCodes.OrderWithoutDeliveryAddress)]);
            }

            order = await orderRepository.ChangeOrderStatusAsync(orderId, OrderStatus.InStock);
            if(order is not null)
                return Result<OrderResponseDto>.Success(MapToOrderResponseDto(order));
            return Result<OrderResponseDto>.Failure(
                [new ValidationResult(ErrorCodes.OrderNotFound)]);
        }
        catch (Exception ex)
        {
            return Result<OrderResponseDto>.Failure(
                [new ValidationResult(ex.Message)]);
        }
    }

    /// <summary>
    /// Ships an order.
    /// </summary>
    /// <param name = "orderId" >Order identifier</param>
    /// <returns>Result containing the updated order information</returns>
    public async Task<Result<OrderResponseDto>> ShipOrderAsync(Guid orderId)
    {
        try
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return Result<OrderResponseDto>.Failure(
                    [new ValidationResult(ErrorCodes.OrderNotFound)]);
            if (order.OrderStatus == OrderStatus.Closed)
                return Result<OrderResponseDto>.Failure(
                    [new ValidationResult(ErrorCodes.OrderAlreadyClosed)]);

            // Business rule: Orders without address should be marked as error
            if (string.IsNullOrEmpty(order.DeliveryAddress.Trim()))
            {
                await orderRepository.ChangeOrderStatusAsync(orderId, OrderStatus.Error);
                return Result<OrderResponseDto>.Failure(
                    [new ValidationResult(ErrorCodes.OrderWithoutDeliveryAddress)]);
            }

            // Business rule: Orders should change to InShipping after max 5 seconds
            _ = Task.Run(async () =>
            {
                await Task.Delay(new Random().Next(1000, 5000));
                order = await orderRepository.ChangeOrderStatusAsync(orderId, OrderStatus.InShipping);
            });
            return Result<OrderResponseDto>.Success(MapToOrderResponseDto(order));
        }
        catch (Exception ex)
        {
            return Result<OrderResponseDto>.Failure(
                [new ValidationResult(ex.Message)]);
        }
    }

    /// <summary>
    /// Completes the shipping process and closes the order.
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>Result containing the updated order information</returns>
    public async Task<Result<OrderResponseDto>> CloseOrderAsync(Guid orderId)
    {
        try
        {
            var existingOrder = await orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
                return Result<OrderResponseDto>.Failure(
                    [new ValidationResult(ErrorCodes.OrderNotFound)]);
            
            if (existingOrder.OrderStatus == OrderStatus.Closed)
                return Result<OrderResponseDto>.Failure(
                    [new ValidationResult(ErrorCodes.OrderAlreadyClosed)]);
            
            var order = await orderRepository.ChangeOrderStatusAsync(orderId, OrderStatus.Closed);
            if(order is not null)
                return Result<OrderResponseDto>.Success(MapToOrderResponseDto(order));
            return Result<OrderResponseDto>.Failure(
                [new ValidationResult(ErrorCodes.OrderNotFound)]);
        }
        catch (Exception ex)
        {
            return Result<OrderResponseDto>.Failure(
                [new ValidationResult(ex.Message)]);
        }
    }
}