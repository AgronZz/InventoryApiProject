# Inventory Management API

A RESTful Web API built with ASP.NET Core 8 for managing products, categories, suppliers, orders, and stock transactions. This was built as a final project for our .NET Web API course.


## Project Structure

```
InventoryApi/
│
├── InventoryApi/                        # Main API project
│   ├── Auth/                            # Authentication-related files
│   │   ├── UserConstants.cs             # Hardcoded users (admin, staff, viewer)
│   │   ├── UserDto.cs                   # DTO used for the login request body
│   │   └── UserModel.cs                 # User model with username, password, role, email
│   │
│   ├── Controllers/                     # API endpoints, one controller per feature area
│   │   ├── AuthController.cs            # POST /api/auth — login and get a JWT token
│   │   ├── ProductController.cs         # CRUD for products + search and pagination
│   │   ├── CategoryController.cs        # CRUD for product categories
│   │   ├── SupplierController.cs        # CRUD for suppliers
│   │   ├── OrderController.cs           # Place and retrieve orders
│   │   ├── StockController.cs           # Stock transactions and stock summary
│   │   ├── CurrencyController.cs        # Live currency conversion using a third-party API
│   │   ├── ReportController.cs          # Report endpoints like products grouped by category
│   │   └── UserController.cs            # User info endpoints
│   │
│   ├── Data/
│   │   └── AppDbContext.cs              # Entity Framework Core database context
│   │
│   ├── Dtos/                            # Data Transfer Objects (what the API sends and receives)
│   │   ├── ProductDto.cs                # Product response DTO
│   │   ├── CreateProductDto.cs          # DTO for creating a product
│   │   ├── UpdateProductDto.cs          # DTO for updating a product
│   │   ├── CategoryDto.cs               # Category response DTO
│   │   ├── CreateCategoryDto.cs         # DTO for creating a category
│   │   ├── SupplierDto.cs               # Supplier response DTO
│   │   ├── CreateSupplierDto.cs         # DTO for creating a supplier
│   │   ├── UpdateSupplierDto.cs         # DTO for updating a supplier
│   │   ├── OrderDto.cs                  # Order response DTO
│   │   ├── CreateOrderDto.cs            # DTO for placing an order
│   │   ├── CreateOrderItemDto.cs        # DTO for individual items inside an order
│   │   ├── OrderItemDto.cs              # Order item response DTO
│   │   ├── StockTransactionDto.cs       # Stock movement response DTO
│   │   ├── CurrencyConversionDto.cs     # Currency conversion response DTO
│   │   └── ProductsByCategoryDto.cs     # Report DTO for products grouped by category
│   │
│   ├── Helpers/
│   │   ├── ExceptionHandlingMiddleware.cs  # Global error handler, catches unhandled exceptions and returns a clean JSON error instead of crashing
│   │   └── PagedModel.cs                   # Generic wrapper used for paginated responses
│   │
│   ├── Mappings/                        # AutoMapper profiles that handle model to DTO conversion
│   │   ├── ProductProfile.cs
│   │   ├── CategoryProfile.cs
│   │   ├── SupplierProfile.cs
│   │   ├── OrderProfile.cs
│   │   └── StockProfile.cs
│   │
│   ├── Migrations/                      # EF Core database migrations (auto-generated)
│   │   ├── 20260521162255_InitialMigration.cs
│   │   └── AppDbContextModelSnapshot.cs
│   │
│   ├── Models/                          # Database entity models
│   │   ├── Product.cs                   # Product entity (name, price, stock quantity, categoryId, supplierId)
│   │   ├── Category.cs                  # Category entity
│   │   ├── Supplier.cs                  # Supplier entity
│   │   ├── Order.cs                     # Order entity which contains a list of OrderItems
│   │   ├── OrderItem.cs                 # Represents one product line inside an order
│   │   └── StockTransaction.cs          # Tracks IN and OUT stock movements per product
│   │
│   ├── Repositories/                    # Data access layer using the Repository Pattern
│   │   ├── IProductRepository.cs        # Interface defining product data operations
│   │   ├── ProductRepository.cs         # Implementation using EF Core
│   │   ├── ICategoryRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── ISupplierRepository.cs
│   │   ├── SupplierRepository.cs
│   │   ├── IOrderRepository.cs
│   │   ├── OrderRepository.cs
│   │   ├── IStockTransactionRepository.cs
│   │   └── StockTransactionRepository.cs
│   │
│   ├── Services/                        # External service integrations
│   │   ├── ICurrencyService.cs          # Interface for the currency conversion service
│   │   └── CurrencyService.cs           # Makes HTTP calls to open.er-api.com for live exchange rates
│   │
│   ├── appsettings.json                 # Main config file: database connection string and JWT settings
│   ├── appsettings.Development.json     # Development environment overrides
│   ├── Program.cs                       # App entry point, registers all services, middleware, and auth
│   └── log.txt                          # Serilog log output file, auto-generated when the app runs
│
├── InventoryApi.Tests/                  # Unit test project
│   ├── Controllers/
│   │   ├── ProductControllerTests.cs
│   │   ├── CategoryControllerTests.cs
│   │   ├── SupplierControllerTests.cs
│   │   ├── OrderControllerTests.cs
│   │   └── StockControllerTests.cs
│   └── InventoryApi.Tests.csproj
│
└── InventoryApi.sln                     # Visual Studio solution file
```


## Prerequisites

Before running the project make sure you have the following installed:

- .NET SDK 8.0 or newer — https://dotnet.microsoft.com/download
- SQL Server (Express edition is fine) — https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- Visual Studio 2022 — https://visualstudio.microsoft.com/

You can also use VS Code with the C# Dev Kit extension if you prefer that over Visual Studio.



## How to Run

### 1. Clone the repository

```bash
git clone https://github.com/your-username/InventoryApi.git
cd InventoryApi
```

### 2. Update the database connection string

Open `InventoryApi/appsettings.json` and change the connection string to match your SQL Server setup:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=InventoryDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
}
```

Replace `YOUR_SERVER_NAME` with your actual server name, for example `localhost`, `.\SQLEXPRESS`, or whatever your instance is called. You can find this in SQL Server Management Studio when you connect.

### 3. Apply the database migrations

Navigate into the main project folder and run:

```bash
cd InventoryApi
dotnet ef database update
```

This will create the `InventoryDB` database and all the tables automatically based on the migrations we already have.

If you don't have the EF Core CLI tools installed yet, run this first:

```bash
dotnet tool install --global dotnet-ef
```

### 4. Run the API

```bash
dotnet run
```

Or just open `InventoryApi.sln` in Visual Studio and press F5.

The API will start on:
- https://localhost:7109
- http://localhost:5109

### 5. Open Swagger

Once the API is running, open your browser and go to:

```
https://localhost:7109/swagger
```

Swagger gives you a UI where you can test all the endpoints without needing Postman.


## Authentication

The API uses JWT Bearer tokens. Most endpoints require you to be logged in.

### Step 1 - Log in

Send a POST request to `/api/auth` with one of the test accounts below:

| Username | Password | Role          |
|----------|----------|---------------|
| admin    | admin    | Administrator |
| staff    | staff    | Staff         |
| viewer   | viewer   | Viewer        |

Request body:
```json
{
  "username": "admin",
  "password": "admin"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Administrator"
}
```

### Step 2 - Authorize in Swagger

Copy the token from the response, then click the Authorize button in Swagger (top right of the page) and paste it like this:

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

After that all your requests will automatically include the token.



## Main Endpoints

| Method | Endpoint | Description | Who can access |
|--------|----------|-------------|----------------|
| POST | /api/auth | Login and get a JWT token | Public |
| GET | /api/product | Get all products (paginated) | Public |
| GET | /api/product/{id} | Get one product by ID | Authenticated |
| GET | /api/product/search | Search, filter and sort products | Authenticated |
| POST | /api/product | Create a product | Administrator only |
| PUT | /api/product/{id} | Update a product | Administrator only |
| DELETE | /api/product/{id} | Delete a product | Administrator only |
| GET | /api/category | Get all categories | Authenticated |
| GET | /api/supplier | Get all suppliers | Authenticated |
| GET | /api/order | Get all orders | Authenticated |
| POST | /api/order | Place a new order | Authenticated |
| GET | /api/stock | Get all stock transactions | Administrator and Staff |
| GET | /api/stock/summary | Stock IN/OUT summary per product | Authenticated |
| GET | /api/currency/convert | Convert between currencies | Authenticated |
| GET | /api/report/... | Various reports | Authenticated |

---

## Pagination, Sorting and Filtering

The product search endpoint supports filtering by multiple fields at once:

```
GET /api/product/search?pageNumber=1&pageSize=10&searchText=laptop&priceFrom=100&priceTo=500&sortField=name&sortOrder=true
```

The response comes back as a PagedModel that includes the results plus metadata so the client knows where it is in the full dataset:

```json
{
  "items": [...],
  "totalItems": 42,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

---

## Third-Party Integration

The CurrencyController connects to the Open Exchange Rates API (https://open.er-api.com) which is free and does not require an API key.

Example request:
```
GET /api/currency/convert?from=USD&to=EUR&amount=100
```

Example response:
```json
{
  "from": "USD",
  "to": "EUR",
  "amount": 100,
  "result": 91.45
}
```



## Running the Tests

```bash
cd InventoryApi.Tests
dotnet test
```

The tests use mocked repositories so you do not need a database connection to run them. We have tests for ProductController, CategoryController, SupplierController, OrderController, and StockController. Each test checks that the endpoints return the correct HTTP status codes and data in both normal and edge case scenarios.



## Logging

The app uses Serilog for logging. Logs are written to two places:

- The console (information level and above)
- `log.txt` in the project root (debug level and above)

Things that get logged include user logins, product creation, product deletion, stock summary generation, and any unhandled exceptions. The ExceptionHandlingMiddleware catches crashes across the whole API and logs them before returning a clean error message to the client.



## Technologies Used

- ASP.NET Core 8 — web API framework
- Entity Framework Core — database access and ORM
- SQL Server — database
- JWT Bearer Authentication — login and role-based access control
- AutoMapper — handles model to DTO mapping automatically
- Serilog — logging to console and file
- xUnit — unit testing framework
- Moq — mocking library used in tests
- Open Exchange Rates API — live currency conversion
- Swagger / Swashbuckle — API documentation and testing UI
