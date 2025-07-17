# Customer Management System - .NET Core 8 REST API

This repository contains a complete customer management system converted from WCF .NET Framework 3.5 to a modern .NET Core 8 REST API with JSON responses.

## Project Structure

```
├── CustomerAPI/                 # .NET Core 8 Web API
│   ├── Controllers/
│   │   └── CustomersController.cs # REST API endpoints
│   ├── Models/
│   │   └── Customer.cs          # JSON-friendly customer model
│   ├── Services/
│   │   ├── ICustomerService.cs  # Service interface
│   │   └── CustomerService.cs   # Business logic implementation
│   ├── Program.cs               # API startup configuration
│   └── CustomerAPI.csproj
├── CustomerAPI.Client/          # .NET Core 8 Console Client
│   ├── Models/
│   │   └── Customer.cs          # Client-side customer model
│   ├── Program.cs               # REST API consumption demo
│   └── CustomerAPI.Client.csproj
└── Legacy WCF Projects/         # Original WCF .NET Framework 3.5
    ├── WCFDemo.Service/         # WCF Service Library
    ├── WCFDemo.Host/            # WCF Console Host
    └── WCFDemo.Client/          # WCF Console Client
```

## Modern REST API Features

### API Endpoints
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/customers/country/{country}` - Get customers by country
- `GET /api/customers/count` - Get total customer count
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update existing customer
- `DELETE /api/customers/{id}` - Delete customer (soft delete)

### Technology Stack
- **.NET Core 8**: Modern cross-platform framework
- **ASP.NET Core Web API**: RESTful API framework
- **System.Text.Json**: High-performance JSON serialization
- **Swagger/OpenAPI**: Interactive API documentation
- **HttpClient**: Modern HTTP client for API consumption

## Getting Started

### Prerequisites
- .NET Core 8 SDK
- Any modern IDE (Visual Studio, VS Code, Rider)

### Running the REST API

1. **Start the API server:**
   ```bash
   cd CustomerAPI
   dotnet run
   ```
   The API will be available at `http://localhost:5202`

2. **Access Swagger UI:**
   Open `http://localhost:5202/swagger` in your browser for interactive API documentation

### Running the Client Demo

1. **Ensure the API is running** (see above)

2. **Run the console client:**
   ```bash
   cd CustomerAPI.Client
   dotnet run
   ```

## API Usage Examples

### Get All Customers
```bash
curl -X GET http://localhost:5202/api/customers
```

### Get Customer by ID
```bash
curl -X GET http://localhost:5202/api/customers/3
```

### Get Customers by Country
```bash
curl -X GET http://localhost:5202/api/customers/country/USA
```

### Create New Customer
```bash
curl -X POST http://localhost:5202/api/customers \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Smith",
    "email": "john.smith@example.com",
    "dateOfBirth": "1990-05-15T00:00:00",
    "city": "Seattle",
    "country": "USA"
  }'
```

### Update Customer
```bash
curl -X PUT http://localhost:5202/api/customers/11 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 11,
    "firstName": "John",
    "lastName": "Smith",
    "email": "john.smith@example.com",
    "dateOfBirth": "1990-05-15T00:00:00",
    "city": "Portland",
    "country": "USA",
    "isActive": true
  }'
```

### Delete Customer
```bash
curl -X DELETE http://localhost:5202/api/customers/11
```

## Sample Data

The API includes 10 international customers for demonstration:

1. **John Doe** (USA) - New York
2. **Jane Smith** (UK) - London  
3. **Carlos Rodriguez** (Spain) - Madrid
4. **Marie Dubois** (France) - Paris
5. **Hans Mueller** (Germany) - Berlin
6. **Anna Kowalski** (Poland) - Warsaw
7. **Luigi Rossi** (Italy) - Rome
8. **Sarah Johnson** (Canada) - Toronto
9. **Yuki Tanaka** (Japan) - Tokyo
10. **Pedro Silva** (Brazil) - São Paulo

## JSON Response Format

All API responses return JSON. Example customer object:

```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@email.com",
  "dateOfBirth": "1985-03-15T00:00:00",
  "city": "New York",
  "country": "USA",
  "isActive": true
}
```

## Migration from WCF

This system replaces the original WCF implementation with modern REST API:

### What Changed
- **Protocol**: SOAP/XML → HTTP REST/JSON
- **Framework**: .NET Framework 3.5 → .NET Core 8
- **Service Model**: WCF ServiceContract → ASP.NET Core Controller
- **Data Format**: DataContract → JSON with System.Text.Json
- **Client**: WCF Channel Factory → HttpClient
- **Documentation**: None → Swagger/OpenAPI

### What Stayed the Same
- **Business Logic**: Customer operations and fake data unchanged
- **Operations**: All 7 customer operations preserved
- **Soft Delete**: Inactive customers still supported
- **Data Validation**: Enhanced with model validation attributes

## Development Features

- **Cross-Platform**: Runs on Windows, macOS, and Linux
- **Hot Reload**: Automatic reload during development
- **Async/Await**: Modern asynchronous programming patterns
- **Dependency Injection**: Built-in IoC container
- **Configuration**: JSON-based configuration system
- **Logging**: Structured logging with multiple providers

## Future Enhancements

The REST API foundation enables easy addition of:
- Database persistence (Entity Framework Core)
- Authentication/Authorization (JWT, OAuth)
- Caching (Redis, in-memory)
- Monitoring (Application Insights, Prometheus)
- API versioning
- Rate limiting
- CORS support for web applications

This modernized system provides a solid foundation for building scalable, maintainable customer management applications.