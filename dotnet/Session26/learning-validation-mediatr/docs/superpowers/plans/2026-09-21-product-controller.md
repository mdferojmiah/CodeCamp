# Product Controller Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add `ProductController` exposing the full CRUD API, wire controller support into `Program.cs`, and verify every endpoint over HTTP.

**Architecture:** Step 2 of 2. The controller lives in `Controllers/`, depends only on `IProductService` (constructor injection), and maps service results to HTTP status codes. Runs on minimal hosting that already has the store (singleton) and service (scoped) registered from plan 1.

**Tech Stack:** ASP.NET Core 9, attribute-routed API controller, auto-generated OpenAPI. No new packages.

**Spec:** `docs/superpowers/specs/2026-09-21-product-service-design.md`
**Depends on plan 1 (must exist and build):** `docs/superpowers/plans/2026-09-21-product-core-layers.md`

## Global Constraints

- Namespace for the controller: `learning_validation_mediatr.Controllers`.
- Route prefix is `api/[controller]` → `api/products`.
- Controller injects `IProductService` via constructor; it never talks to the store.
- `ProductStore` is registered singleton, `ProductService` scoped in `Program.cs` (from plan 1) — do not change those lifetimes.
- No validation attributes anywhere.
- **No git in this session** (user directive): tasks end with build/HTTP checkpoints instead of commits.

## Review Focus

Inputs and failure modes the spec implies; each is pinned to its owning task.

1. **Unknown id on GET/PUT/DELETE must be 404, not a 500** — controller maps `null` / `false` service results to `NotFound()` (Tasks 1, 3).
2. **POST must return 201 with a `Location` header** pointing at the new resource via `CreatedAtAction` (Task 2).
3. **DELETE success must be 204, and a second delete of the same id must be 404** — the store returns `false` when the id is already gone (Task 3).
4. **Controllers only activate when both `AddControllers()` and `MapControllers()` are registered** — forgetting either yields 404 for every endpoint at runtime even though the project builds (Task 1).
5. **Request bodies bind to the DTOs without validation** — posts/puts accept a bare `{ "name": ..., "price": ... }` JSON body with no requirement attributes (Tasks 2, 3, 4).

---

### Task 1: Controller support + GET endpoints

**Files:**
- Modify: `Program.cs`
- Create: `Controllers/ProductController.cs`

**Interfaces:**
- Consumes: `IProductService.GetAllAsync()` → `Task<IEnumerable<ProductResponse>>`; `IProductService.GetByIdAsync(int)` → `Task<ProductResponse?>`; `ProductResponse` from `learning_validation_mediatr.Dtos`.
- Produces: routed endpoints `GET api/products` and `GET api/products/{id}`.

- [ ] **Step 1: Register and map controller support**

`Program.cs` — full intended final state:

```csharp
using learning_validation_mediatr.Services;
using learning_validation_mediatr.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IProductStore, ProductStore>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
```

`AddControllers()` registers the controller infrastructure; `app.MapControllers()` maps the controller routes. Both are required — see review focus 4.

- [ ] **Step 2: Create the controller with the GET endpoints**

`Controllers/ProductController.cs`:

```csharp
using learning_validation_mediatr.Dtos;
using learning_validation_mediatr.Services;
using Microsoft.AspNetCore.Mvc;

namespace learning_validation_mediatr.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }
}
```

`{id:int}` constrains the route so `api/products/abc` gets a 404 instead of reaching the action.

- [ ] **Step 3: Build**

Run: `dotnet build`
Expected: `Build succeeded`.

- [ ] **Step 4: Verify the GET endpoints over HTTP**

Run the app: `dotnet run --launch-profile http` (stays running; from a second terminal run the checks below).

```powershell
curl.exe -s http://localhost:5058/api/products          # seeded list
curl.exe -s http://localhost:5058/api/products/1        # first seeded product
curl.exe -s -o NUL -w "%{http_code}" http://localhost:5058/api/products/999   # 404
```

Expected: GET all → `[{"id":1,"name":"Laptop","price":999.99},...]`; GET id 1 → the Laptop; GET id 999 prints `404`.

---

### Task 2: POST — create

**Files:**
- Modify: `Controllers/ProductController.cs`

**Interfaces:**
- Consumes: `IProductService.CreateAsync(CreateProductRequest)` → `Task<ProductResponse>`.
- Produces: routed endpoint `POST api/products` returning `201 Created` with a `Location` header.

- [ ] **Step 1: Add the POST action**

In `ProductController.cs`, after `GetById`:

```csharp
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request)
    {
        var product = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
```

`CreatedAtAction(nameof(GetById), ...)` produces `201` plus a `Location` header pointing at `api/products/{id}`.

- [ ] **Step 2: Build**

Run: `dotnet build`
Expected: `Build succeeded`.

- [ ] **Step 3: Verify POST over HTTP**

Restart the app; in a second terminal. Note: on Windows, pass the JSON body from a file (`-d @file`) because inline `-d "{\"...\"}"` gets mangled by PowerShell's quoting:

```powershell
'{"name":"Monitor","price":199.99}' | Out-File -Encoding ascii C:\Temp\post-body.json
curl.exe -s -i -X POST http://localhost:5058/api/products -H "Content-Type: application/json" -d "@C:\Temp\post-body.json"
```

`-i` prints response headers so you can see the `Location` header. Expected: first line `HTTP/1.1 201 Created`, a `Location: http://localhost:5058/api/products/4` header, and a body of `{"id":4,"name":"Monitor","price":199.99}` (the store assigns id 4 after the three seeds).

---

### Task 3: PUT + DELETE

**Files:**
- Modify: `Controllers/ProductController.cs`

**Interfaces:**
- Consumes: `IProductService.UpdateAsync(int, UpdateProductRequest)` → `Task<ProductResponse?>`; `IProductService.DeleteAsync(int)` → `Task<bool>`.
- Produces: routed endpoints `PUT api/products/{id}` and `DELETE api/products/{id}`.

- [ ] **Step 1: Add the PUT action**

In `ProductController.cs`, after `Create`:

```csharp
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponse>> Update(int id, UpdateProductRequest request)
    {
        var product = await _productService.UpdateAsync(id, request);
        return product is null ? NotFound() : Ok(product);
    }
```

- [ ] **Step 2: Add the DELETE action**

```csharp
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
```

- [ ] **Step 3: Build**

Run: `dotnet build`
Expected: `Build succeeded`.

- [ ] **Step 4: Verify PUT and DELETE over HTTP**

Restart the app; in a second terminal (put the JSON body in a file — see Task 2 step 3 for the quoting note):

```powershell
'{"name":"Laptop Pro","price":1299.99}' | Out-File -Encoding ascii C:\Temp\put-body.json

# PUT existing id -> 200 + updated body
curl.exe -s -i -X PUT http://localhost:5058/api/products/1 -H "Content-Type: application/json" -d "@C:\Temp\put-body.json"

# PUT unknown id -> 404
curl.exe -s -o NUL -w "%{http_code}" -X PUT http://localhost:5058/api/products/999 -H "Content-Type: application/json" -d "@C:\Temp\put-body.json"

# DELETE existing id -> 204, then same id again -> 404 (already gone)
curl.exe -s -o NUL -w "%{http_code}" -X DELETE http://localhost:5058/api/products/2
curl.exe -s -o NUL -w "%{http_code}" -X DELETE http://localhost:5058/api/products/2
```

Expected: PUT id 1 → `200` with body `{"id":1,"name":"Laptop Pro","price":1299.99}`; PUT id 999 → `404`; DELETE id 2 → `204` then `404`.

---

### Task 4: .http file + full endpoint verification

**Files:**
- Modify: `learning-validation-mediatr.http`

**Interfaces:**
- Consumes: all five endpoints from Tasks 1-3.
- Produces: a runnable request collection covering the whole CRUD surface.

- [ ] **Step 1: Replace the file's contents with product requests**

`learning-validation-mediatr.http`:

```http
@learning_validation_mediatr_HostAddress = http://localhost:5058

GET {{learning_validation_mediatr_HostAddress}}/api/products
Accept: application/json

###

GET {{learning_validation_mediatr_HostAddress}}/api/products/1
Accept: application/json

###

GET {{learning_validation_mediatr_HostAddress}}/api/products/999
Accept: application/json

###

POST {{learning_validation_mediatr_HostAddress}}/api/products
Content-Type: application/json

{
  "name": "Monitor",
  "price": 199.99
}

###

PUT {{learning_validation_mediatr_HostAddress}}/api/products/1
Content-Type: application/json

{
  "name": "Laptop Pro",
  "price": 1299.99
}

###

DELETE {{learning_validation_mediatr_HostAddress}}/api/products/3

###
```

- [ ] **Step 2: Build and run the full sweep**

Run: `dotnet build` → `Build succeeded`.
Run: `dotnet run --launch-profile http` (restart the app so the singleton store reseeds — the expected values below assume fresh state from a new run), then execute the requests in the `.http` file (REST Client in VS Code) or the `curl.exe` commands from Tasks 1-3.

Expected, in order:
1. `200` — 3 seeded products (list).
2. `200` — the seeded Laptop.
3. `404` — unknown id 999.
4. `201` — Monitor created, `Location: .../api/products/4`.
5. `200` — Laptop renamed to "Laptop Pro" at price 1299.99.
6. `204` — Keyboard (id 3) deleted.

- [ ] **Step 3: Checkpoint — step 2 complete**

All five endpoints work: index listing, single fetch, create, update, delete; the four 404 branches (GET/PUT/DELETE unknown id) verified; POST returns 201 + Location; DELETE returns 204. The feature is fully implemented across the two plans.

---

**End of step 2.** The full feature (spec: `docs/superpowers/specs/2026-09-21-product-service-design.md`) is implemented.