# Product Service + Controller Design

**Date:** 2026-09-21
**Status:** Approved for spec writing
**Project:** learning-validation-mediatr (ASP.NET Core 9, minimal API, OpenAPI enabled)

## Goal

Add an in-memory product catalog exposed via a REST API. The API is consumed via
a controller that depends on a service interface, backed by an in-memory store
behind its own store interface. This is a learning exercise aimed at
demonstrating clean layering: Controller -> service interface -> service
implementation -> store interface -> in-memory store. Requests and responses
travel as separate request/response DTOs; no validation is implemented.

## Domain Model

`Product` lives in `Models/Product.cs` and is the internal entity the store owns.
It is never exposed over the wire directly.

- `Id` — `int`, auto-increment, assigned by the store at creation time.
- `Name` — `string`, default `string.Empty`.
- `Price` — `decimal`.

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

## DTOs

Separate request and response DTOs live in the root `Dtos/` folder, one file per
DTO.

```csharp
// Dtos/CreateProductRequest.cs
public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

// Dtos/UpdateProductRequest.cs
public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

// Dtos/ProductResponse.cs
public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

Precedence rule: request DTOs never carry an `Id` (the store assigns it);
`ProductResponse` always carries the persisted `Id`.

## Project Structure (layered folders)

```
Models/
  Product.cs
Dtos/
  CreateProductRequest.cs
  UpdateProductRequest.cs
  ProductResponse.cs
Stores/
  IProductStore.cs
  ProductStore.cs
Services/
  IProductService.cs
  ProductService.cs
Controllers/
  ProductController.cs
```

Layer roles:
- `Models/` — the `Product` entity, owned by the store.
- `Dtos/` — wire contracts (requests/responses).
- `Stores/` — `IProductStore` plus its in-memory implementation.
- `Services/` — `IProductService` plus its implementation (DTO mapping).
- `Controllers/` — HTTP layer, depends only on `IProductService`.

Existing files touched: only `Program.cs` (DI wiring + `AddControllers`).

## Components

### IProductStore (`Stores/IProductStore.cs`)

Interface the service depends on, so the store stays swappable. Works on the
`Product` entity and is DTO-unaware.

```csharp
public interface IProductStore
{
    Product Add(Product product);
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product? Update(int id, Product product);
    bool Delete(int id);
}
```

`null` return means "entity not found".

### ProductStore (`Stores/ProductStore.cs`)

In-memory data structure holding all products; implements `IProductStore`.

- Storage: `Dictionary<int, Product>` keyed by `Id`.
- Identifiers: an integer `_nextId` counter auto-increments on each create, so
  clients never supply an `Id` on create; the store assigns it.
- Seeding: 2-3 sample products inserted on construction so the API returns data
  immediately without setup.
- Concurrency: synchronized with `lock` (or `ConcurrentDictionary`); chosen so
  simultaneous requests cannot corrupt the dictionary or re-use an Id.
- Methods:
  - `Product Add(Product product)` — assigns `Id = _nextId++`, inserts, returns the stored product.
  - `IEnumerable<Product> GetAll()` — returns all products.
  - `Product? GetById(int id)` — returns the product or `null`.
  - `Product? Update(int id, Product product)` — replaces the named product's
    Name/Price, returns the updated product, or `null` if no such Id exists.
  - `bool Delete(int id)` — removes and returns `true`, or `false` if absent.

### IProductService (`Services/IProductService.cs`)

Interface consumed by the controller. Method signatures use DTOs for input and
output; async so callers never block.

```csharp
public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<ProductResponse?> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(CreateProductRequest request);
    Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteAsync(int id);
}
```

`null` return means "entity not found".

### ProductService (`Services/ProductService.cs`)

Implements `IProductService` by delegating to the injected `IProductStore`.
The service is the only component that maps between DTOs and the `Product`
entity: requests become `Product` before hitting the store, and stored
`Product`s become `ProductResponse` before returning to the controller.

### ProductController (`Controllers/ProductController.cs`)

- Attributes: `[ApiController]`, `[Route("api/products")]` => route `api/products`.
- Constructor-injects `IProductService`.
- Bound bodies are `CreateProductRequest` / `UpdateProductRequest`; all bodies
  returned are `ProductResponse` / `IEnumerable<ProductResponse>`.
- Endpoints:

| Method | Route             | Request body        | Success                                      | Failure        |
|--------|-------------------|---------------------|----------------------------------------------|----------------|
| GET    | `api/products`    | —                   | 200 + `IEnumerable<ProductResponse>`         | —              |
| GET    | `api/products/{id}` | —                 | 200 + `ProductResponse`                      | 404 (not found)|
| POST   | `api/products`    | `CreateProductRequest` | 201 Created + `ProductResponse`, `Location` header | —        |
| PUT    | `api/products/{id}` | `UpdateProductRequest` | 200 + updated `ProductResponse`          | 404 (not found)|
| DELETE | `api/products/{id}` | —                 | 204 No Content                               | 404 (not found) |

### DI wiring (`Program.cs`)

- `builder.Services.AddControllers();`
- `builder.Services.AddSingleton<IProductStore, ProductStore>();` — a single
  store instance lives for the whole process, so all requests share one
  in-memory dataset.
- `builder.Services.AddScoped<IProductService, ProductService>();` — one
  service instance per request; stateless besides its store dependency.

## Error Handling

- Missing entity -> `404 NotFound` with no body (or a minimal message).
- No validation is implemented: no `[Required]`, `[Range]`, or other
  validation attributes anywhere; the model binder simply deserializes the
  incoming JSON onto the request DTOs.

## Testing / Verification

- `dotnet build` passes.
- Run the app and exercise the endpoints through the provided `.http` file:
  - GET all returns seeded products (as `ProductResponse`).
  - GET by id returns a seeded product; unknown id returns 404.
  - POST creates and returns 201 + Location with a `ProductResponse`.
  - PUT updates an existing product; unknown id returns 404.
  - DELETE removes; unknown id returns 404.

## Out of Scope

- Persistence to a real database.
- Validation (explicitly not implemented — no validation attributes).
- AutoMapper or any mapping library; mapping is hand-written in the service.
- Repository layer (the store interface already decouples persistence).
- MediatR — this iteration is plain services; MediatR is a separate session topic.