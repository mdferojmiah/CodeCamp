# SDD ledger — plan: docs/superpowers/plans/2026-09-21-product-core-layers.md

Created: 2026-09-21.
Execution mode: inline (native), driven by user task-by-task. User directive: NO git — no commits, checkpoints are build/behavior gate.
Verification method per approved spec: `dotnet build` (no test project).

## Tasks

Task 1: complete (Models/Product.cs created, tests: none — `dotnet build` → Build succeeded, 0 warnings, 0 errors)
Task 2: complete (Dtos/CreateProductRequest.cs, UpdateProductRequest.cs, ProductResponse.cs created; request DTOs have Name+Price only, response has Id+Name+Price — `dotnet build` → Build succeeded, 0 warnings, 0 errors)
Task 3: complete (Stores/IProductStore.cs + ProductStore.cs created; lock-protected, _nextId++ in lock, seeds ids 1-3 via Add, DTO-free interface, null/false on missing — `dotnet build` → Build succeeded, 0 warnings, 0 errors)
Task 4: complete (Services/IProductService.cs + ProductService.cs created; DTO surface, injects IProductStore, manual Product↔ProductResponse mapping — `dotnet build` → Build succeeded, 0 warnings, 0 errors)
Task 5: complete (Program.cs registers AddSingleton<IProductStore, ProductStore> + AddScoped<IProductService, ProductService> — `dotnet build` → Build succeeded, 0 warnings, 0 errors)

## Plan 1 complete — core layers delivered
Models/Product.cs, Dtos/*, Stores/IProductStore.cs + ProductStore.cs, Services/IProductService.cs + ProductService.cs, DI in Program.cs. Step 2 (controller) pending in docs/superpowers/plans/2026-09-21-product-controller.md.TASK 1 (PLAN 2): Ruling: plan used [Route(\"api/[controller]\")] which resolves to /api/Product (singular); spec mandates /api/products. Changed to [Route(\"api/products\")]. Cost if wrong: route differs from .http/plan wording only. Verified GET all/1/999 = 200/200/404.
TASK 2 (PLAN 2): verified POST -> 201, Location /api/products/4, body {id:4,Monitor,199.99}, GET all shows id 4.
TASK 3 (PLAN 2): verified PUT id1 -> 200 renamed; PUT 999 -> 404; DELETE id2 -> 204 then 404; GET all confirms.
Ruling: verification on Windows PowerShell 5.1 � inline -d \"{\"name\"...}\" mangles JSON (PowerShell strips inner quotes for native cmds). Use -d @C:\...\body.json (file body). Cost if wrong: verification commands differ from plan text.
FINAL REVIEW: fresh reviewer verdict = clean, ready to ship (all Plan1/Plan2 review-focus items verified). Deferred minors: (1) store hands out live mutable Product references (no in-app path hits it); (2) .http file hardcodes http://localhost:5058, breaks under https profile; (3) NRT makes non-nullable Name implicitly required by binder (no [Required] attribute, spec constraint kept); (4) service async-shaped but synchronous via Task.FromResult.
