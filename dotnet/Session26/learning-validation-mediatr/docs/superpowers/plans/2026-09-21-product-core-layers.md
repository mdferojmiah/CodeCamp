# Product Core Layers Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build the non-HTTP layers of the product feature — `Product` model, request/response DTOs, `IProductStore`/`ProductStore`, and `IProductService`/`ProductService` — and register them in the DI container.

**Architecture:** Layered folders (`Models/`, `Dtos/`, `Stores/`, `Services/`, `Controllers/`). The controller will eventually depend only on `IProductService`; the service depends on `IProductStore`; the store owns the in-memory `Product` data and is DTO-unaware. This plan produces every layer except the controller, which step 2 adds.

**Tech Stack:** ASP.NET Core 9 (minimal hosting), C# 13, no extra packages.

**Spec:** `docs/superpowers/specs/2026-09-21-product-service-design.md`

## Global Constraints

- Root namespace is `learning_validation_mediatr`; namespaces follow folders: `learning_validation_mediatr.Models`, `learning_validation_mediatr.Dtos`, `learning_validation_mediatr.Stores`, `learning_validation_mediatr.Services`.
- No validation attributes anywhere (`[Required]`, `[Range]`, etc.) — plain deserialization only.
- Request DTOs (`CreateProductRequest`, `UpdateProductRequest`) never carry an `Id`; the store assigns it.
- `ProductStore` is DTO-unaware: its methods take and return `Product`, never DTOs.
- Store is concurrency-safe (lock or `ConcurrentDictionary`) so parallel requests cannot corrupt the dictionary or reuse an `Id`.
- `ProductStore` seeds 2-3 products so the API returns data without setup.
- **No git in this session** (user directive): tasks end with a build/behavior checkpoint instead of a commit.

## Review Focus

Inputs and failure modes the spec implies; each is pinned to its owning task.

1. **Two concurrent creates must not produce duplicate `Id`s** — the `_nextId++` step must be inside the lock, not outside it (Task 3).
2. **Client-supplied `Id` on create is ignored** — `ProductStore.Add` overwrites `product.Id` with the store's counter, so a request DTO (no `Id` anyway) can never collide (Task 3).
3. **Missing entity returns `null`, never an exception** — `GetById`/`Update`/`Delete` on an unknown id must be a graceful `null`/`false` so the controller (step 2) can map to 404 (Task 3).
4. **Store stays DTO-unaware** — `IProductStore` methods signature-check: they must reference `Product`, not any DTO type (Task 3).
5. **DI wiring compiles and matches lifetimes** — store singleton, service scoped; exercise nothing at runtime here, full resolution is verified in Plan 2 (Task 5).

---

### Task 1: Product model

**Files:**
- Create: `Models/Product.cs`

**Interfaces:**
- Consumes: nothing (no dependencies).
- Produces: `learning_validation_mediatr.Models.Product` — the entity every later file in this plan and plan 2 references. Members: `int Id`, `string Name`, `decimal Price`.

- [ ] **Step 1: Create the model**

`Models/Product.cs`:

```csharp
namespace learning_validation_mediatr.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

- [ ] **Step 2: Verify the project builds**

Run: `dotnet build`
Expected: `Build succeeded` with the `Models/Product.cs` compiled in.

---

### Task 2: Request/response DTOs

**Files:**
- Create: `Dtos/CreateProductRequest.cs`
- Create: `Dtos/UpdateProductRequest.cs`
- Create: `Dtos/ProductResponse.cs`

**Interfaces:**
- Consumes: nothing (`Product` is not referenced by the DTOs).
- Produces: `learning_validation_mediatr.Dtos.CreateProductRequest` (`Name`, `Price`), `UpdateProductRequest` (`Name`, `Price`), `ProductResponse` (`Id`, `Name`, `Price`). Request DTOs carry no `Id`; the response always does.

- [ ] **Step 1: Create the request DTOs**

`Dtos/CreateProductRequest.cs`:

```csharp
namespace learning_validation_mediatr.Dtos;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

`Dtos/UpdateProductRequest.cs`:

```csharp
namespace learning_validation_mediatr.Dtos;

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

- [ ] **Step 2: Create the response DTO**

`Dtos/ProductResponse.cs`:

```csharp
namespace learning_validation_mediatr.Dtos;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

- [ ] **Step 3: Verify the project builds**

Run: `dotnet build`
Expected: `Build succeeded`. Confirm `CreateProductRequest` and `UpdateProductRequest` have exactly `Name` + `Price` (no `Id`), `ProductResponse` has `Id` + `Name` + `Price`.

---

### Task 3: IProductStore + ProductStore

**Files:**
- Create: `Stores/IProductStore.cs`
- Create: `Stores/ProductStore.cs`

**Interfaces:**
- Consumes: `Product` from Task 1.
- Produces: `learning_validation_mediatr.Stores.IProductStore` with synchronous methods working on `Product`:
  - `Product Add(Product product)` — assigns `Id = _nextId++`, inserts, returns the stored product.
  - `IEnumerable<Product> GetAll()`
  - `Product? GetById(int id)`
  - `Product? Update(int id, Product product)` — replaces Name/Price, returns updated product, or `null` if id absent.
  - `bool Delete(int id)`

- [ ] **Step 1: Create the store interface**

`Stores/IProductStore.cs`:

```csharp
using learning_validation_mediatr.Models;

namespace learning_validation_mediatr.Stores;

public interface IProductStore
{
    Product Add(Product product);
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product? Update(int id, Product product);
    bool Delete(int id);
}
```

- [ ] **Step 2: Create the in-memory store**

`Stores/ProductStore.cs`:

```csharp
using learning_validation_mediatr.Models;

namespace learning_validation_mediatr.Stores;

public class ProductStore : IProductStore
{
    private readonly object _lock = new();
    private readonly Dictionary<int, Product> _products = new();
    private int _nextId = 1;

    public ProductStore()
    {
        Add(new Product { Name = "Laptop", Price = 999.99m });
        Add(new Product { Name = "Mouse", Price = 29.99m });
        Add(new Product { Name = "Keyboard", Price = 49.99m });
    }

    public Product Add(Product product)
    {
        lock (_lock)
        {
            product.Id = _nextId++;
            _products[product.Id] = product;
            return product;
        }
    }

    public IEnumerable<Product> GetAll()
    {
        lock (_lock)
        {
            return _products.Values.ToList();
        }
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            return _products.TryGetValue(id, out var product) ? product : null;
        }
    }

    public Product? Update(int id, Product product)
    {
        lock (_lock)
        {
            if (!_products.TryGetValue(id, out var existing))
            {
                return null;
            }

            existing.Name = product.Name;
            existing.Price = product.Price;
            return existing;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _products.Remove(id);
        }
    }
}
```

Note the two rules pinned by the review focus: `_nextId++` sits inside the lock so concurrent creates get unique Ids, and the constructor seeds 3 products by calling `Add` so Ids 1, 2, 3 are sequential.

- [ ] **Step 3: Verify the project builds**

Run: `dotnet build`
Expected: `Build succeeded`.
Checkpoint — the store interface is DTO-free (review focus 4): every method references `Product`, none reference `CreateProductRequest`/`UpdateProductRequest`/`ProductResponse`.

---

### Task 4: IProductService + ProductService

**Files:**
- Create: `Services/IProductService.cs`
- Create: `Services/ProductService.cs`

**Interfaces:**
- Consumes: `Product` (Task 1), the DTOs (Task 2), `IProductStore` (Task 3).
- Produces: `learning_validation_mediatr.Services.IProductService`, consumed by the controller in Plan 2:
  - `Task<IEnumerable<ProductResponse>> GetAllAsync()`
  - `Task<ProductResponse?> GetByIdAsync(int id)`
  - `Task<ProductResponse> CreateAsync(CreateProductRequest request)`
  - `Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)`
  - `Task<bool> DeleteAsync(int id)`

- [ ] **Step 1: Create the service interface**

`Services/IProductService.cs`:

```csharp
using learning_validation_mediatr.Dtos;

namespace learning_validation_mediatr.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<ProductResponse?> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(CreateProductRequest request);
    Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteAsync(int id);
}
```

- [ ] **Step 2: Create the service implementation**

`Services/ProductService.cs` — injects `IProductStore` and maps DTOs ↔ `Product`:

```csharp
using learning_validation_mediatr.Dtos;
using learning_validation_mediatr.Models;
using learning_validation_mediatr.Stores;

namespace learning_validation_mediatr.Services;

public class ProductService : IProductService
{
    private readonly IProductStore _store;

    public ProductService(IProductStore store)
    {
        _store = store;
    }

    public Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var responses = _store.GetAll().Select(ToResponse);
        return Task.FromResult(responses);
    }

    public Task<ProductResponse?> GetByIdAsync(int id)
    {
        var product = _store.GetById(id);
        return Task.FromResult(product is null ? null : ToResponse(product));
    }

    public Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        var product = _store.Add(new Product { Name = request.Name, Price = request.Price });
        return Task.FromResult(ToResponse(product));
    }

    public Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = _store.Update(id, new Product { Name = request.Name, Price = request.Price });
        return Task.FromResult(product is null ? null : ToResponse(product));
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(_store.Delete(id));
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
```

The service method signatures use DTOs for input/output (per `IProductService`); the `_store` methods take/return `Product` only.

- [ ] **Step 3: Verify the project builds**

Run: `dotnet build`
Expected: `Build succeeded`.

---

### Task 5: Register dependencies in Program.cs

**Files:**
- Modify: `Program.cs`

**Interfaces:**
- Consumes: `IProductStore`, `ProductStore` (Task 3), `IProductService`, `ProductService` (Task 4).
- Produces: a composition root that resolves `IProductService` in Plan 2.

- [ ] **Step 1: Register the layers with the specified lifetimes**

`Program.cs`:

```csharp
using learning_validation_mediatr.Services;
using learning_validation_mediatr.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IProductStore, ProductStore>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
```

`AddOpenApi` stays as-is. Store is singleton (one dataset for the process); service is scoped (one instance per request).

- [ ] **Step 2: Verify the project builds**

Run: `dotnet build`
Expected: `Build succeeded`.

- [ ] **Step 3: Checkpoint — end of core layers**

Layered folders now exist and compile: `Models/Product.cs`, `Dtos/*`, `Stores/IProductStore.cs` + `ProductStore.cs`, `Services/IProductService.cs` + `ProductService.cs`, with DI registered in `Program.cs`. Nothing here is HTTP-exercised yet — plan 2 adds `AddControllers()`/`MapControllers()`, the controller, and full endpoint verification.

---

**Handoff to plan 2:** `docs/superpowers/plans/2026-09-21-product-controller.md`