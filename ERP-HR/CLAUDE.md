# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

ERP-HR is an HR module of an ERP system, built as an ASP.NET Core Web API on **.NET 10** (`net10.0`, nullable + implicit usings enabled). It is in early scaffolding — much of the wiring does not yet exist and the project does **not currently compile** (see "Current state" below).

## Commands

```powershell
dotnet build                 # build (currently FAILS — see Current state)
dotnet run                   # run the API
dotnet watch run             # run with hot reload
```

- No test project exists yet. When one is added, run with `dotnet test` (single test: `dotnet test --filter "FullyQualifiedName~TestName"`).
- `ERP-HR.http` contains REST Client requests for manual API testing, but its sample request (`/weatherforecast/`) points at an endpoint that does not exist.

### Ports — note the inconsistency
- `Program.cs` hardcodes `app.Run("http://+:8080")`, which **overrides** `launchSettings.json`. The app actually listens on **8080**.
- `Properties/launchSettings.json` advertises `5174`/`7190`, and `ERP-HR.http` targets `5174`. These are currently wrong relative to `Program.cs`.

## Architecture

This is a **single-project Clean Architecture layout**: all layers live inside one csproj (`ERP-HR.csproj` at the repo root), separated by folders + namespaces rather than by separate projects.

- `ERP.Domain/` — namespace `ERP.Domain.*`. Entities (`Entities/`) and enums (`Enums/`). No dependencies on other layers.
- `ERP.Application/` — namespace `ERP.Application.*`. DTOs (`DTOs/`), e.g. `CreateEmployee` records. Application/use-case logic belongs here.
- `ERP.Infrastructure/` — namespace `ERP.Infrastructure.*`. EF Core `AppDbContext` (`Configurations/`) and persistence concerns.
- Root — `Program.cs` (minimal-API host/composition root), `appsettings*.json`.

Note: folder/namespace prefixes are `ERP.*` but the project's `RootNamespace` is `ERP_HR`. Declare namespaces explicitly (as existing files do) to keep the `ERP.Domain` / `ERP.Application` / `ERP.Infrastructure` convention.

### Domain entity conventions — two coexisting styles
The intended pattern is a **rich domain model**: see `Employee` — private setters, a private parameterless constructor, and a static `Create(...)` factory that assigns `Id`, timestamps (`CreatedAt`/`UpdatedAt` in UTC), and default `Status`. Prefer this for new entities.

Other entities (`LeaveRequest`, `User`) are currently **anemic** (public getters/setters, no factory). When extending them, migrate toward the `Employee` pattern (encapsulated invariants, factory methods) rather than adding more public setters.

Common conventions: `Guid` primary keys, `DateOnly` for calendar dates, `DateTime` (UTC) for timestamps, enums in `ERP.Domain.Enums` for status/role/level fields.

## Current state / known breakage

Before adding features, expect to fix the existing scaffold first:

1. **`ERP.Infrastructure/Configurations/AppDbContext.cs` is broken** and is the reason the build fails:
   - Class is `AppDbContext` but the constructor is named `AppContext` (won't match).
   - `DbSet<Employee>` is declared *inside* the constructor body instead of as a class member.
   - `set<Employee>()` should be `Set<Employee>()`.
   - Namespace is `ERP.Infrastructure.Configuration` (singular) while the folder is `Configurations`.
2. **EF Core is not referenced.** `AppDbContext` uses `Microsoft.EntityFrameworkCore`, but `ERP-HR.csproj` only references `Microsoft.AspNetCore.OpenApi`. Add the `Microsoft.EntityFrameworkCore` packages (and a provider) before the DbContext will compile.
3. **`Program.cs` is a bare host** — no DI registration (DbContext, services), no endpoint mapping, no OpenAPI/middleware wiring. Endpoints and service registration still need to be built out.
