# Dapper - Fluent Execution
![dapper-fluent-execution](https://github.com/AugustoDeveloper/dapper-fluent-execution/actions/workflows/release.yml/badge.svg) [![NuGet](https://img.shields.io/nuget/v/Dapper.FluentExecution.svg)](https://nuget.org/packages/Dapper.FluentExecution) [![Nuget](https://img.shields.io/nuget/dt/Dapper.FluentExecution.svg)](https://nuget.org/packages/Dapper.FluentExecution)

This package is a simple, easy and fluent way to execute SQL on database connection by Dapper.

## Installation
You can install the package using one of the options bellow:
 - Package Manager
```
PM> NuGet\Install-Package Dapper.FluentExecution -Version 0.2.0
```

 - .NET CLI
```
dotnet add package Dapper.FluentExecution --version 0.2.0
```

 - PackageReference
```
<PackageReference Include="Dapper.FluentExecution" Version="0.2.0" />
```

## Usage

Simple query:
```csharp
public async Task<List<Person>> GetAllAsync(CancellationToken cancellation = default)
    => await "SELECT * FROM Persons"
        .On(DatabaseConnection)
        .QueryAsync<Person>(cancellation)
        .ToListAsync();
```
Querying with parameter:
```csharp
public async Task<Person?> GetByIdAsync(int id, CancellationToken cancellation = default)
    => await "SELECT * FROM Persons where Id = @Id"
        .On(DatabaseConnection)
        .WithParameter("@Id", id)
        .QuerySingleOrDefaultAsync<Person>(cancellation);
```

Querying with conditional parameter and append:
```csharp
public async Task<Person?> GetByIdAsync(int id, int? specificAddressId, CancellationToken cancellation = default)
    => await "SELECT p.* FROM Persons p inner join Addresses a on a.PersonId = p.Id where Id = @Id"
        .On(DatabaseConnection)
        .AppendSql(specificAddressId.HasValue, "AND a.Id = @AddressId")
        .WithParameter(specificAddressId.HasValue, "@AddressId", specificAddressId)
        .WithParameter("@Id", id)
        .QuerySingleOrDefaultAsync<Person>(cancellation);
```
## Release - v0.2.0
- Add summary doc on all .cs files
- Add suport to `Execution` methods on `IExecutionBuilder`. E.g.: `Execute, ExecuteAsync, ExecuteScalar, ExecuteScalarAsync...`.
- Add support to dynamic parameter by dynamic object. E.g.: `new { Param1 = "1", Param2 = 2 }`
- Add support to set command timeout
- Add support to set query as an execution of stored procedure
- Add `QueryAsync, Query,...` an `IEnumerable<T>` result

## Release - v0.1.1
- Fixing `csproj` documentation to Nuget.org

## Release - v0.1.0
- Methods to query directly from a string by fluent builder
- Adding support to add `SqlDbType` parameters
