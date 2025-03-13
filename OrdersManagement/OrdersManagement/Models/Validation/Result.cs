using System.ComponentModel.DataAnnotations;

namespace OrdersManagement.Models.Validation;

/// <summary>
/// Represents a response result.
/// </summary>
/// <typeparam name="T">Type of the value</typeparam>
public class Result<T>
{
    /// <summary>
    /// Value of the result.
    /// </summary>
    public T? Value { get; private set; }
    
    /// <summary>
    /// Indicates if the result is an error.
    /// </summary>
    public bool IsError { get; private set; }
    
    /// <summary>
    /// List of error codes.
    /// </summary>
    public List<ValidationResult> ErrorCodes { get; private set; } = new();
    
    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <param name="value">Value of the result</param>
    /// <returns>Result object</returns>
    public static Result<T> Success(T value) => new() 
    { 
        Value = value, 
        IsError = false
    };
    
    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorCodes">List of error codes</param>
    /// <returns>Result object</returns>
    public static Result<T> Failure(List<ValidationResult> errorCodes) => new() 
    { 
        IsError = true, 
        ErrorCodes = errorCodes 
    };
}