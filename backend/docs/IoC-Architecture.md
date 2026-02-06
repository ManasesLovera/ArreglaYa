# Dependency Injection (IoC) Architecture

This document describes the modular Inversion of Control (IoC) architecture implemented in the ArreglaYa backend.

## Overview

The backend follows a **Clean Architecture** pattern with modular dependency injection. Each layer has its own IoC structure that organizes registrations by functional areas/sections.

## Architecture Layers

### 1. Infrastructure Layer (`Infrastructure/IOC/`)

**Main Entry Point:** `DependencyInjection.cs` → `AddInfrastructure()`

**Sub-sections:**
- **Data** (`Data/DataServiceExtensions.cs`) - Database context registration
  - Entity Framework Core DbContext
  - SQLite connection configuration
  
- **Identity** (`Identity/IdentityServiceExtensions.cs`) - Authentication & identity services
  - ASP.NET Core Identity configuration
  - User & Role management
  
- **Repositories** (`Repositories/RepositoryServiceExtensions.cs`) - Data access repositories
  - IUserRepository → UserRepository
  - Additional repositories as needed

### 2. Application Layer (`Application/IOC/`)

**Main Entry Point:** `DependencyInjection.cs` → `AddApplicationLayer()`

**Sub-sections:**
- **Mapper** (`Mapper/MapperServiceExtensions.cs`) - Object mapping configuration
  - AutoMapper profiles
  
- **Services** (`Services/ServiceExtensions.cs`) - Business logic services
  - IAuthService → AuthService
  - Additional application services as needed

### 3. WebAPI Layer (`WebAPI/IOC/`)

**Main Entry Point:** `DependencyInjection.cs` → `AddWebAPILayer()`

**Aggregates:**
- **Configuration** - CORS and Swagger setup
- **Authentication** - JWT token configuration
- **Validation** - FluentValidation validators

## Usage in Program.cs

```csharp
// Dependency Injection from other layers
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddWebAPILayer(builder.Configuration, builder);
```

## Benefits

1. **Modularity** - Each functional area has its own registration file
2. **Maintainability** - Easy to find and update specific service registrations
3. **Scalability** - New sections can be added without modifying existing code
4. **Separation of Concerns** - Each layer manages its own dependencies
5. **Clean Architecture** - Dependencies flow from outer layers to inner layers

## Adding New Services

### Infrastructure Layer
1. Add service registration to appropriate file in `Infrastructure/IOC/[Section]/`
2. Or create a new section file if it represents a new functional area
3. Update `Infrastructure/IOC/DependencyInjection.cs` to call the new section

### Application Layer
1. Add service registration to appropriate file in `Application/IOC/[Section]/`
2. Or create a new section file if it represents a new functional area
3. Update `Application/IOC/DependencyInjection.cs` to call the new section

### WebAPI Layer
1. Add service registration to appropriate configuration file
2. Update `WebAPI/IOC/DependencyInjection.cs` if needed

## Migration Notes

- **Old files deprecated:**
  - `Infrastructure/IOC/IOCInfraestructure.cs` (use `DependencyInjection.AddInfrastructure()` instead)
  - `Application/IOC/AddApplication.cs` (use `DependencyInjection.AddApplicationLayer()` instead)
  
- These files are marked with `[Obsolete]` attributes and kept for backward compatibility only.
