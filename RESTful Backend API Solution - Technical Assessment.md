# RESTful Backend API Solution - Technical Assessment

## Overview
This document outlines approach to implementing a RESTful backend API solution as requested in the technical assessment. The solution needs to be designed with scalability, maintainability, and industry best practices in mind.

## Problem Statment 
Design a RESTfull API solution around Products to perform CRUD operation. 

## Test Submission
1. Do not submit/upload your code in this repository.
2. **_Create your own public repo_** and share link with us.


## Architecture
A High Level Technical Architecture highlighting each component
### Tech Stack
- **Framework**: .NET 8 with C#
- **API Framework**: ASP.NET Core Web API
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT with refresh token strategy
- **Testing**: xUnit, Moq, and WebApplicationFactory
- **Documentation**: Swagger/OpenAPI with Swashbuckle
- **Containerization**: Docker and Docker Compose
- **Logging**: Logging Framework of your choice for structured logging

### Project Structure

```
Solution/
├── src/
│   ├── API/                  # ASP.NET Core Web API project
│   │   ├── Controllers/      # API controllers
│   │   ├── Filters/          # Action filters for cross-cutting concerns
│   │   ├── Middleware/       # Custom middleware components
│   │   ├── Extensions/       # Extension methods for DI and app configuration
│   │   ├── Program.cs        # Application entry point and configuration
│   │   └── appsettings.json  # Configuration files
│   ├── Application/          # Application logic layer
│   │   ├── DTOs/             # Data Transfer Objects
│   │   ├── Interfaces/       # Service interfaces
│   │   ├── Mapping/          # Object mapping profiles
│   │   ├── Services/         # Service implementations
│   │   └── Validators/       # Request validation rules
│   ├── Domain/               # Domain layer
│   │   ├── Entities/         # Domain models
│   │   ├── Enums/            # Enumeration types
│   │   ├── Events/           # Domain events
│   │   └── Exceptions/       # Custom domain exceptions
│   └── Infrastructure/       # Infrastructure layer
│       ├── Data/             # Data access components
│       │   ├── Configurations/  # Entity type configurations
│       │   ├── Repositories/    # Repository implementations
│       │   ├── ApplicationDbContext.cs  # EF Core DbContext
│       │   └── UnitOfWork.cs    # Unit of Work implementation
│       ├── Identity/          # Authentication and authorization
│       ├── Logging/           # Logging infrastructure
│       └── Services/          # External service integrations
├── tests/
│   ├── API.Tests/            # Integration tests for API
│   ├── Application.Tests/    # Unit tests for application layer
│   └── Infrastructure.Tests/ # Unit tests for infrastructure layer
└── docker-compose.yml        # Docker Compose configuration
```

## API Design Expectation

### Resource-Oriented
- Resources are identified by clear, consistent URLs
- Actions on resources are represented by HTTP methods
- Resource relationships are reflected in URL structure

### Request/Response Format
- JSON for all request and response bodies
- Consistent error response format
- Use of HTTP status codes according to standards

### Endpoint Structure Example
```
# Get specific resource
# Create resource
# Update resource
# Delete resource
# Get related resources
```

### Databse Structure 
```
CREATE TABLE [dbo].[Item]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    [ProductId] INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    [Quantity] INT NOT NULL
)

CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[ProductName] NVARCHAR(255) NOT NULL,
	[CreatedBy]  NVARCHAR(100) NOT NULL,
	[CreatedOn]  DATETIME NOT NULL,
	[ModifiedBy]  NVARCHAR(100) NULL,
	[ModifiedOn]  DATETIME NULL

)

```


## Implementation Highlights

### Authentication & Authorization

### Error Handling Middleware

### Data Validation with FluentValidation

### Controller Example with API Versioning

### Service Layer approach

### Repository Pattern with Entity Framework Core

## Testing Strategy

### Unit Tests with xUnit and Moq  OR any other framework of your choice


## Performance Considerations

- Database query optimization using Entity Framework's AsNoTracking() for read-only operations
- Proper indexing strategy for SQL Server tables
- Pagination for all collection endpoints to limit result sizes
- Response compression middleware
- Async/await pattern throughout the application for better thread utilization

## Security Measures
- JWT authentication with short-lived access tokens and refresh token rotation
- Role-based authorization for sensitive operations
- Input validation with FluentValidation on all endpoints
- Implementation of CORS policy
- Usage of HTTPS with TLS 1.2+
- Protection against common web vulnerabilities using security headers


## Documentation
- OpenAPI/Swagger documentation for all endpoints
- JSDoc comments for code documentation
- Documented (High Level) authentication flow
- Environment setup (High Level) instructions
- Deployment procedures (High Level)

## Deployment Configuration
### Docker Setup
#### dockerfile
#### docker-compose.yml
