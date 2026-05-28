# AI Prompt Log

## 2026-05-27 — Create Solutio and required projects and implement logic
**Tool:** Claude  
**Prompt:** "You are a senior .NET software engineer and solution architect.

Your task is to build a complete backend solution for the following technical assessment using C# and .NET.

IMPORTANT INSTRUCTIONS:
- Generate the solution incrementally step-by-step.
- Do NOT generate the entire solution at once.
- After each phase, clearly explain what was generated and why.
- Ensure the generated code compiles before moving to the next phase.
- Follow clean coding principles and pragmatic architecture.
- Prioritize readability, maintainability, and simplicity over overengineering.
- Use production-quality coding standards.

==================================================
BUSINESS CONTEXT
==================================================

Northwind Traders is a fictional company that sells food products worldwide.

The company requires a simple internal backend service that allows staff to:
1. Retrieve customers
2. Search/filter customers by name
3. View customer details
4. View customer order summaries

Data source:
Use the existing Microsoft Northwind SQL Server database.

==================================================
TECH STACK
==================================================

Use:
- .NET 8
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- REST API with Controllers
- Swagger/OpenAPI
- xUnit
- FluentAssertions
- Moq
- Testcontainers or WebApplicationFactory for integration tests

==================================================
ARCHITECTURE REQUIREMENTS
==================================================

Use a pragmatic layered architecture.

DO NOT overengineer.

Preferred structure:
- API Layer
- Application Layer
- Infrastructure Layer
- Domain Layer
- Tests

Use clear separation of concerns.

Apply:
- SOLID principles
- Clean code practices
- Dependency Injection
- Proper exception handling
- Async/await best practices
- DTO pattern
- Validation
- Logging
- Configuration via appsettings

Avoid unnecessary complexity.

DO NOT implement CQRS/MediatR unless truly beneficial.

Repository pattern is optional:
- Only use it if it adds clarity/value.
- Avoid generic repositories if unnecessary.

==================================================
SOLUTION STRUCTURE
==================================================

Use the following naming convention:

- Fourth.TradersTask.sln
- Fourth.TradersTask.API
- Fourth.TradersTask.Application
- Fourth.TradersTask.Domain
- Fourth.TradersTask.Infrastructure
- Fourth.TradersTask.UnitTests
- Fourth.TradersTask.IntegrationTests

==================================================
DATABASE REQUIREMENTS
==================================================

Use Scaffold-DbContext against the existing Northwind SQL Server database.

Generate EF Core entities from the database.

Use proper entity configuration.

DO NOT expose EF entities directly from the API.

==================================================
FUNCTIONAL REQUIREMENTS
==================================================

1. Customer List Endpoint

Create endpoint:
GET /api/customers

Requirements:
- Returns customer list
- Include:
  - Customer ID
  - Customer Name
  - Number of Orders
- Support:
  - Pagination
  - Search/filter by customer name
- Use efficient LINQ queries
- Avoid N+1 queries

Response should include:
- Current page
- Page size
- Total count
- Total pages
- Data collection

--------------------------------------------------

2. Customer Detail Endpoint

Create endpoint:
GET /api/customers/{id}

Requirements:
Return:
- Customer details
- Order summaries

For each order include:
- Order ID
- Order Date
- Total Order Value
- Number of Products

Calculate:
- Total order value from order details
- Product count from order details

Use efficient querying.

==================================================
NON-FUNCTIONAL REQUIREMENTS
==================================================

Implement:
- Global exception handling middleware
- Structured logging
- Validation
- Proper HTTP status codes
- CancellationToken support where appropriate
- Swagger documentation
- XML comments if useful

Use async operations throughout.

==================================================
TESTING REQUIREMENTS
==================================================

Include:
- Unit tests
- Integration tests

Unit tests should cover:
- Customer service logic
- Pagination logic
- Order summary calculations

Integration tests should verify:
- API endpoints
- HTTP responses
- Filtering/search behavior

Use:
- xUnit
- FluentAssertions
- Moq

Integration tests should use:
- WebApplicationFactory OR Testcontainers

==================================================
README REQUIREMENTS
==================================================

Generate a professional README.md including:

- Project overview
- Architecture overview
- Technologies used
- How to run locally
- Database setup
- How to run tests
- API endpoints
- Design decisions
- Assumptions/trade-offs
- Future improvements

Also include:
- AI prompts used for generation

==================================================
IMPLEMENTATION STYLE
==================================================

Generate incrementally in phases.

PHASE ORDER:

Phase 1:
- Solution structure
- Project creation
- Project references
- NuGet packages

Phase 2:
- Scaffold EF Core from Northwind
- Configure DbContext
- Infrastructure setup

Phase 3:
- Domain models and DTOs
- Application services/interfaces
- Pagination models

Phase 4:
- Customer list endpoint
- Search/filter implementation
- Pagination implementation

Phase 5:
- Customer detail endpoint
- Order summary calculations

Phase 6:
- Exception middleware
- Logging
- Swagger
- Validation

Phase 7:
- Unit tests

Phase 8:
- Integration tests

Phase 9:
- README generation

For each phase:
1. Explain architectural reasoning
2. Generate code
3. Explain important decisions
4. Ensure code consistency with previous phases

==================================================
CODING STANDARDS
==================================================

Requirements:
- Use file-scoped namespaces
- Enable nullable reference types
- Use clean naming conventions
- Keep controllers thin
- Keep business logic in services
- Use DTOs for API contracts
- Avoid duplicated logic
- Keep methods small and focused
- Use meaningful abstractions
- Use IConfiguration/options pattern properly

==================================================
IMPORTANT
==================================================

The final solution should feel like:
- A professional senior-level backend assessment submission
- Pragmatic and maintainable
- Clean and easy to review
- Not overengineered
- Production-quality but appropriately scoped

Always prioritize:
- Clarity
- Simplicity
- Correctness
- Maintainability"  
**Files affected:** `Create solution with required projects`

## 2026-05-27 — Update to .NET 9
**Tool:** Claude  
**Prompt:** "Update the projects to .Net 9 and run all tests"  
**Files affected:** `Updated all 6 .csproj files from .NET 8.0 → .NET 9.0, EntityFrameworkCore 8.0 → 9.0, AspNetCore.OpenApi 8.0 → 9.0, Microsoft.NET.Test.Sdk 17.9 → 17.10, Swashbuckle.AspNetCore 6.5 → 7.0, All Microsoft.Extensions.* packages 8.0 → 9.0`

## 2026-05-27 — build the project
**Tool:** Claude  
**Prompt:** "build the project"  
**Files affected:** `Program.cs`

## 2026-05-27 — Review the solution
**Tool:** Claude  
**Prompt:** "review the entire solution if found code to improve list it and propose changes that make it better"  
**Files affected:** `Program.cs, CustomersController, GlobalExceptionHandlingMiddleware, PaginationParams class, CustomerService, ApiConstants.cs, Models, PaginationParamsValidator`

## 2026-05-28 — Issue when starting project
**Tool:** Claude  
**Prompt:** "when i start the project it is not loading swagger ## currently is white page with nothing in it"  
**Files affected:** `Program.cs`

## 2026-05-28 — Issue when starting project
**Tool:** Claude  
**Prompt:** "when i try /api/customers the response is not visualized in swagger"  
**Files affected:** `appsettings.json`

## 2026-05-28 — Issue with Swagger
**Tool:** Claude  
**Prompt:** "when i try /api/customers the response is not visualized in swagger ## it is not connection issue i test it this code var result = await _customerService.GetCustomersAsync(paginationParams, cancellationToken); the result has data but after that the data is not visualized in swagger"  
**Files affected:** `CustomersController`