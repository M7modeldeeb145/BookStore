# BookStore

Simple Book Store API built with .NET 10 following a clean layered architecture.

## Requirements

- .NET 10 SDK
- (Optional) dotnet-ef tool for migrations: `dotnet tool install --global dotnet-ef`

## Projects

- BookStore.API — ASP.NET Core Web API (startup project)
- BookStore.Application — Application layer (services, DTOs, validation)
- BookStore.Infrastructure — Infrastructure helpers
- BookStore.Presistance — Entity Framework Core persistence (DbContext, repositories)
- BookStore.Domain — Domain entities and interfaces

## Getting Started

1. Restore and build

   dotnet restore
   dotnet build

2. (Optional) Create and apply EF migrations

   dotnet ef migrations add InitialCreate --project BookStore.Presistance --startup-project BookStore.API
   dotnet ef database update --project BookStore.Presistance --startup-project BookStore.API

3. Run the API

   dotnet run --project BookStore.API

The API exposes Swagger when running in development.

## Notable libraries

- AutoMapper (mapping profiles)
- FluentValidation (DTO validation)
- Entity Framework Core (data access)
- Swashbuckle/Swagger (API docs)

## Notes

- Projects target .NET 10.
- If you encounter package advisory warnings during restore (for example NU1903), ensure packages are updated. The repository was updated to use AutoMapper 12.0.1 to address a reported advisory.
