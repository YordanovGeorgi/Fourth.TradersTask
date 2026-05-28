# Northwind Traders Backend API

## 🚀 Getting Started

### Prerequisites

- **.NET 8 SDK** or later
- **SQL Server** 2019 or later (LocalDB or Express)
- **Visual Studio** 2022 or **VS Code** with C# Dev Kit

### Database Setup

The solution expects a Northwind database. Set up options:

**Using Northwind Sample Database**
1. Download the Northwind SQL script from Microsoft
https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs
2. Execute against your SQL Server instance
3. Update connection string in `appsettings.json`

### Configuration

Update **appsettings.json**:

```json
{
  "ConnectionStrings": {
    "NorthwindConnection": "Server=.;Database=Northwind;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Running the Application

```bash
# Build solution
dotnet build

# Run API
dotnet run --project src/Fourth.TradersTask.API

### Running Tests

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test tests/Fourth.TradersTask.UnitTests

# Run integration tests only
dotnet test tests/Fourth.TradersTask.IntegrationTests

# Run with verbose output
dotnet test -v n
```

## 📚 API Endpoints

### Get Customers List

```http
GET /api/customers?pageNumber=1&pageSize=10&searchTerm=company
```

**Parameters:**
- `pageNumber` (int, default: 1) - Current page number
- `pageSize` (int, default: 10, max: 100) - Items per page
- `searchTerm` (string, optional) - Search by company name or contact name

**Response (200 OK):**
```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "data": [
    {
      "customerId": "ALFKI",
      "companyName": "Alfreds Futterkiste",
      "numberOfOrders": 6
    }
  ]
}
```

**Error Responses:**
- `400 Bad Request` - Invalid pagination parameters
- `500 Internal Server Error` - Unexpected server error

---

### Get Customer Details

```http
GET /api/customers/{id}
```

**Parameters:**
- `id` (string) - Customer ID (e.g., "ALFKI")

**Response (200 OK):**
```json
{
  "customerId": "ALFKI",
  "companyName": "Alfreds Futterkiste",
  "contactName": "Maria Anders",
  "contactTitle": "Sales Representative",
  "address": "Obere Str. 57",
  "city": "Berlin",
  "postalCode": "12209",
  "country": "Germany",
  "phone": "030-0074321",
  "fax": "030-0076545",
  "orders": [
    {
      "orderId": 10643,
      "orderDate": "2023-08-25T00:00:00",
      "totalOrderValue": 3200.00,
      "numberOfProducts": 3
    }
  ]
}
```

## 📊 Assumptions

### Assumptions
- Northwind database is properly set up
- SQL Server is accessible via connection string
- .NET 8 runtime available

### Business Logic Assumptions
- Assume customer's name is [CompanyName]

### Future improvements
- If have more time i would do also front end

**Generated**: May 27, 2026 | **Updated**: May 28, 2026 (.NET 9 upgrade)
