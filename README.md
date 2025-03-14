# Orders Management App

This repository contains a .NET console application developed as part of a recruitment task.

## 🚀 Features

### Order Management
- Create orders with customer details and payment information
- Process orders through various statuses
- Search for specific orders by ID
- View all orders in the system

### Business Rules
- Orders with empty delivery addresses marked as **Error**
- Cash on delivery orders ≥ 2500 automatically returned to customer
- Orders transition to **InShipping** status asynchronously (5-second delay)

### Statistics
- Distribution of orders by status and customer type
- Revenue metrics and average order values

## 🔧 Technical Details
- Clean architecture with repository pattern
- Dependency injection for loosely coupled components
- Entity Framework Core with SQLite database
- Result pattern for error handling

## 📊 Order Workflow
Orders progress through the following states:
- **New** - Initial order state
- **InStock** - Order processed in warehouse
- **InShipping** - Order in transit
- **Closed** - Order successfully delivered
- **Error** - Invalid order (e.g., missing address)
- **ReturnedToCustomer** - Order returned due to business rule violation

## 🧪 Testing
- **Unit Tests:** Verifying service and repository logic
- **Integration Tests:** End-to-end flow testing with in-memory database
- **UI Tests:** Console interface validation

## 🏗️ Project Structure
- **Models:** Data entities, DTOs, and validation logic
- **Services:** Business logic implementation
- **Repositories:** Data access layer
- **UI:** Console interface components
