# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

Identity is an Identity Server intended to handle authentication for other apps (see README.md). The codebase is currently a fresh ASP.NET Core scaffold with no authentication logic implemented yet.

## Architecture

The solution (`Identity.slnx`) follows CLEAN architecture as a host-and-libraries composition (five projects, all `net10.0`, `Nullable` enabled, `ImplicitUsings` disabled):

- **Api** — the executable host (`Microsoft.NET.Sdk.Web`). `Api/Program.cs` builds the `WebApplication`, calls `AddApi()` / `AddApp()` / `AddClient()` to register services from each library, then `MapApi()` / `MapClient()` to wire up endpoints, and configures the shared HTTP pipeline (HSTS, HTTPS redirection, routing, authorization, OpenAPI/Swagger). Also owns MVC controllers (`Api/Controllers`) via its own `ApiExtensions.AddApi()`/`MapApi()`. References App and Client via `ProjectReference`.
- **App** — a class library (`Microsoft.NET.Sdk`) exposing `AppExtensions.AddApp()`. Owns cross-cutting application service wiring — currently `DbContext` registration and connection-string resolution (reads `AZURE_SQL_CONNECTIONSTRING` from config in Development, from the environment otherwise). References Infrastructure.
- **Client** — a class library (`Microsoft.NET.Sdk.Razor`) exposing `ClientExtensions.AddClient()`/`MapClient()`. Owns Razor Pages (`Client/Pages`) and static assets (`Client/wwwroot`). Composed into Api, not self-hosted.
- **Infrastructure** — a class library (`Microsoft.NET.Sdk`) owning EF Core concerns under `Infrastructure/Database` (`ModelDbContext`, and migrations under `Infrastructure/Database/Migrations`). References Domain.
- **Domain** — a class library (`Microsoft.NET.Sdk`) with no framework dependencies, holding domain models under `Domain/Models` (namespace `Domain.Models`).

When adding a new capability, put the code in the appropriate library (or a new library) and expose it through an `Add<Name>()` / `Map<Name>()` extension pair on `IServiceCollection` / `WebApplication`, then wire the pair into `Api/Program.cs`. Do not put feature logic directly in `Api/Program.cs` beyond composition — it should stay a thin composition root.

Package versions are centrally managed via `Directory.Packages.props` at the repo root (`ManagePackageVersionsCentrally`) — individual `.csproj` files reference packages by name only, without a `Version` attribute.

## Common commands

```bash
# Build the whole solution
dotnet build Identity.slnx

# Run the app (host project)
dotnet run --project Api

# Run via Docker (see Api/Dockerfile)
docker build -f Api/Dockerfile -t identity .
```

There are no test projects in the solution yet.

Local dev URLs (from `Api/Properties/launchSettings.json`): `http://localhost:5285` (http profile) / `https://localhost:7175` (https profile).

### EF Core migrations

See `Infrastructure/Database/efcommands.md` for the full commands. In summary, run from the repo root with `Infrastructure` as the migrations project and `Api` as the startup project, e.g.:

```bash
dotnet ef migrations add <MigrationName> -p Infrastructure -s Api -o Database/Migrations
dotnet ef migrations remove -p Infrastructure -s Api
dotnet ef database update <PreviousMigrationName> -p Infrastructure -s Api
```

## C# coding standards

(from `.claude/rules/standards.md` — these apply to all new/edited code)

- No `var` — always use explicit types.
- Use `this.` for instance members; always use block bodies for methods.
- Stateful types expose private setters plus explicit state-transition methods (e.g. a `Player` with a private-set `Health` mutated only via `TakeDamage`/`Heal`, never by setting the field directly).
- Avoid single-use local variables — inline the expression instead.
- Define constants or enums for meaningful values instead of hardcoding them; never hardcode string values.
- Avoid redundant words in names (`GameLoop`, not `RiverRunGameLoop`, when context is already clear).
- Prefer `foreach` over `for`.
- Provide concise XML doc comments on all classes, methods, and properties.
- Unit test all Domain logic; Application (App) and Infrastructure code may be untested or covered by integration tests only.

## Planning artifacts

`artifacts/stories/` contains story documents (e.g. client registry, Google SSO login, token issuance, refresh tokens, single sign-out, data access/environments) written by the `write-stories` / `create-implementation-plan` skills. Check there for feature specs and background before implementing related functionality.
