# AYA-UIS - University Information System

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-purple.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-red.svg)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

A comprehensive university information system built with ASP.NET Core 8.0, implementing Clean Architecture, CQRS pattern, and RESTful API design principles.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Database](#database)
- [Authentication & Authorization](#authentication--authorization)
- [Configuration](#configuration)
- [Development](#development)
- [Contributing](#contributing)

## 🎯 Overview

AYA-UIS is a modern university information system designed to manage academic operations including departments, grade levels, fee management, academic schedules, and user authentication. The system follows industry best practices with a focus on scalability, maintainability, and security.

## 🏗️ Architecture

The project follows **Clean Architecture** principles with **CQRS (Command Query Responsibility Segregation)** pattern:

```
┌─────────────────────────────────────────────────────────────┐
│                        API Layer                             │
│                    (AYA-UIS.API)                            │
│         Controllers, Middleware, Configuration              │
└─────────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│                   Application Layer                          │
│                (AYA-UIS.Application)                        │
│        Commands, Queries, Handlers (CQRS)                   │
└─────────────────────────────────────────────────────────────┘
                            │
┌──────────────────────┬──────────────────────────────────────┐
│   Presentation       │        Core Layer                     │
│  (Controllers)       │    (AYA-UIS.Core)                    │
│                      │  Domain, Services, Contracts          │
└──────────────────────┴──────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│              Infrastructure Layer                            │
│           (AYA-UIS.Infrastructure)                          │
│      Persistence, Identity, Data Access                     │
└─────────────────────────────────────────────────────────────┘
```

### Key Architectural Patterns

- **Clean Architecture**: Separation of concerns with clear dependency rules
- **CQRS**: Separate read and write operations using MediatR
- **Repository Pattern**: Abstraction over data access
- **Unit of Work**: Transaction management
- **Dependency Injection**: IoC container for loose coupling
- **Specification Pattern**: Complex query logic encapsulation

## ✨ Features

### Academic Management
- 📚 Department management
- 📊 Grade year tracking
- 💰 Department fee management with composite key queries
- 📅 Academic schedule distribution
- 📎 File upload and management for schedules

### Security & Authentication
- 🔐 JWT-based authentication with RSA encryption
- 👤 ASP.NET Core Identity integration
- 🔑 Role-based authorization (Admin, Student, etc.)
- 🛡️ Rate limiting to prevent abuse
- 🔒 Secure password policies

### API Features
- 📖 Swagger/OpenAPI documentation
- 🚫 Global exception handling middleware
- ✅ Model validation with custom error responses
- 🌐 CORS configuration
- 📊 Structured logging

## 🛠️ Technologies

### Backend
- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12.0
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server 2019+
- **Authentication**: ASP.NET Core Identity + JWT
- **API Documentation**: Swagger/Swashbuckle
- **CQRS**: MediatR 14.0
- **Mapping**: AutoMapper

### Security
- **JWT**: RS256 (RSA) asymmetric encryption
- **Rate Limiting**: Fixed window strategy
- **Data Protection**: TrustServerCertificate for SSL

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Moustafa24/AYA-UIS.git
   cd AYA-UIS
   ```

2. **Configure database connection**
   
   Update `appsettings.json` in `AYA-UIS.API` folder:
   ```json
   {
     "ConnectionStrings": {
       "InfoConnection": "Server=localhost;Database=university_db;User Id=sa;Password=YourPassword;TrustServerCertificate=True;",
       "IdentityConnection": "Server=localhost;Database=university_user_db;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
     }
   }
   ```

3. **Generate RSA keys for JWT (if not exists)**
   
   Place `public_key.pem` and `private_key.pem` in `AYA-UIS.API/Keys/` folder.

4. **Restore dependencies**
   ```bash
   dotnet restore
   ```

5. **Apply database migrations**
   ```bash
   # Info Database
   dotnet ef database update --context AYA_UIS_InfoDbContext --project ./AYA-UIS.Infrastructure/Presistence/Presistence.csproj --startup-project ./AYA-UIS.API/AYA-UIS.csproj
   
   # Identity Database
   dotnet ef database update --context IdentityAYADbContext --project ./AYA-UIS.Infrastructure/Presistence/Presistence.csproj --startup-project ./AYA-UIS.API/AYA-UIS.csproj
   ```

6. **Run the application**
   ```bash
   cd AYA-UIS.API
   dotnet run
   ```

7. **Access Swagger UI**
   
   Navigate to: `https://localhost:7121/swagger` or `http://localhost:5282/swagger`

## 📁 Project Structure

```
AYA-UIS/
├── AYA-UIS.API/                      # Main API project
│   ├── Controllers/                  # (Legacy - moved to Presentation)
│   ├── Factories/                    # Response factories
│   ├── MiddelWares/                  # Custom middleware
│   ├── Keys/                         # RSA keys for JWT
│   ├── wwwroot/                      # Static files
│   └── Program.cs                    # Application entry point
│
├── AYA-UIS.Application/              # CQRS Application layer
│   ├── Commands/                     # Write operations
│   │   └── DepartmentFees/
│   ├── Queries/                      # Read operations
│   │   └── DepartmentFees/
│   └── Handlers/                     # Command/Query handlers
│       └── DepartmentFees/
│
├── AYA-UIS.Core/                     # Core business logic
│   ├── Domain/                       # Domain entities
│   │   ├── Contracts/                # Repository interfaces
│   │   ├── Entities/                 # Domain models
│   │   └── Exceptions/               # Custom exceptions
│   ├── Services/                     # Business services
│   │   ├── Implementatios/           # Service implementations
│   │   ├── MappingProfile/           # AutoMapper profiles
│   │   └── Specifications/           # Query specifications
│   └── Services.Abstraction/         # Service contracts
│       └── Contracts/
│
├── AYA-UIS.Infrastructure/           # Infrastructure layer
│   ├── Presistence/                  # Data access
│   │   ├── Data/                     # DbContext & Configurations
│   │   ├── Identity/                 # Identity DbContext
│   │   └── Repositories/             # Repository implementations
│   └── Presentation/                 # API Controllers
│       └── Controllers/
│
└── Shared/                           # Shared kernel
    └── Dtos/                         # Data transfer objects
        ├── ErrorModels/
        └── Info_Module/
```

## 📚 API Documentation

### Base URL
- **HTTPS**: `https://localhost:7121/api`
- **HTTP**: `http://localhost:5282/api`

### Main Endpoints

#### Authentication
- `POST /api/Authentication/register` - Register new user
- `POST /api/Authentication/login` - User login
- `POST /api/Authentication/refresh-token` - Refresh JWT token

#### Department Fees
- `GET /api/DepartmentFees` - Get all department fees
- `GET /api/DepartmentFees/{departmentName}/{gradeYear}` - Get fee by composite key
- `PUT /api/DepartmentFees/{departmentName}/{gradeYear}` - Update fee (Admin only)

#### Academic Schedules
- `GET /api/AcademicSchedules` - Get all schedules
- `POST /api/AcademicSchedules` - Upload new schedule (Admin only)
- `DELETE /api/AcademicSchedules/{id}` - Delete schedule (Admin only)

### CQRS Implementation Example

**Query** (Read Operation):
```csharp
// Query
public record GetAllDepartmentFeesQuery : IRequest<IEnumerable<DepartmentFeeDtos>>;

// Handler
public class GetAllDepartmentFeesQueryHandler 
    : IRequestHandler<GetAllDepartmentFeesQuery, IEnumerable<DepartmentFeeDtos>>
{
    public async Task<IEnumerable<DepartmentFeeDtos>> Handle(
        GetAllDepartmentFeesQuery request, 
        CancellationToken cancellationToken)
    {
        return await _service.GetAllDepartmentFeeAsync();
    }
}
```

**Command** (Write Operation):
```csharp
// Command
public record UpdateDepartmentFeeCommand(
    string DepartmentName, 
    string GradeYear, 
    DepartmentFeeDtos Dto) : IRequest<Unit>;

// Handler
public class UpdateDepartmentFeeCommandHandler 
    : IRequestHandler<UpdateDepartmentFeeCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdateDepartmentFeeCommand request, 
        CancellationToken cancellationToken)
    {
        await _service.UpdateByCompositeKeyAsync(
            request.DepartmentName, 
            request.GradeYear, 
            request.Dto);
        return Unit.Value;
    }
}
```

## 🗄️ Database

### Info Database (`university_db`)
- Departments
- GradeYears
- DepartmentFees
- AcademicSchedules

### Identity Database (`university_user_db`)
- AspNetUsers (with custom User entity)
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- Identity tables

### Migrations

**Create new migration**:
```bash
dotnet ef migrations add MigrationName --context AYA_UIS_InfoDbContext --project ./AYA-UIS.Infrastructure/Presistence/Presistence.csproj --startup-project ./AYA-UIS.API/AYA-UIS.csproj
```

**Update database**:
```bash
dotnet ef database update --context AYA_UIS_InfoDbContext --project ./AYA-UIS.Infrastructure/Presistence/Presistence.csproj --startup-project ./AYA-UIS.API/AYA-UIS.csproj
```

## 🔐 Authentication & Authorization

### JWT Configuration

The system uses **RS256** (RSA asymmetric encryption) for JWT tokens:

```json
{
  "JwtOptions": {
    "Issuer": "https://localhost:7121/",
    "Audience": "https://localhost:7121/",
    "ExpirationInDay": 1
  }
}
```

### Roles
- **Admin**: Full system access
- **Student**: Limited access to student features
- **Teacher**: Access to teaching resources

### Authorization Example
```csharp
[Authorize(Roles = "Admin")]
[HttpPut("{departmentName}/{gradeYear}")]
public async Task<IActionResult> Update(string departmentName, string gradeYear, 
    [FromBody] DepartmentFeeDtos dto)
{
    await _mediator.Send(new UpdateDepartmentFeeCommand(departmentName, gradeYear, dto));
    return NoContent();
}
```

## ⚙️ Configuration

### Rate Limiting
```csharp
options.AddPolicy("PolicyLimitRate", httpContext =>
{
    return RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress!.ToString(),
        factory: key => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 3,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 2
        });
});
```

### CORS
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
});
```

## 🔧 Development

### Running in Development Mode
```bash
dotnet watch run --project ./AYA-UIS.API/AYA-UIS.csproj
```

### Build Solution
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Code Quality
- Follow C# coding conventions
- Use dependency injection
- Implement async/await for I/O operations
- Write unit tests for business logic
- Document public APIs

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Moustafa24** - *Initial work* - [GitHub](https://github.com/Moustafa24)

## 🙏 Acknowledgments

- ASP.NET Core Team for excellent framework
- MediatR for CQRS pattern implementation
- Entity Framework Core Team
- The Clean Architecture community

---

**Note**: This is an educational project for learning Clean Architecture and CQRS patterns in ASP.NET Core.

For questions or support, please open an issue on GitHub.
