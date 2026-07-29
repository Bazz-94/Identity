# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

Identity is an Identity Server intended to handle authentication for other apps (see README.md). The codebase is currently a fresh ASP.NET Core scaffold — the App host wires together an Api library and a Client (Razor Pages) library, with no authentication logic implemented yet.

## Architecture

The solution (`Identity.slnx`) uses a host-and-libraries composition pattern rather than one monolithic web project:

- **App** — the executable host (`Microsoft.NET.Sdk.Web`, `net10.0`). `App/Program.cs` builds the `WebApplication`, calls `AddApi()`/`AddClient()` to register services from each library, then `MapApi()`/`MapClient()` to wire up their endpoints, and configures the shared HTTP pipeline (HSTS, HTTPS redirection, routing, authorization). App references both Api and Client via `ProjectReference`.
- **Api** — a class library (`Microsoft.NET.Sdk`) exposing `ApiExtensions.AddApi()`/`MapApi()`. Owns MVC controllers (`Api/Controllers`) and OpenAPI setup. Has no host of its own — it's composed into App.
- **Client** — a class library (`Microsoft.NET.Sdk.Razor`) exposing `ClientExtensions.AddClient()`/`MapClient()`. Owns Razor Pages (`Client/Pages`) and static assets (`Client/wwwroot`). Also composed into App, not self-hosted.

When adding a new capability, follow this pattern: put the code in the appropriate library (or a new library) and expose it through an `Add<Name>()` / `Map<Name>()` extension pair on `IServiceCollection` / `WebApplication`, then wire the pair into `App/Program.cs`. Do not put feature logic directly in `App` — it should stay a thin composition root.

All projects target `net10.0` with `Nullable` and `ImplicitUsings` enabled.

## Common commands

```bash
# Build the whole solution
dotnet build Identity.slnx

# Run the app (host project)
dotnet run --project App

# Run via Docker (see App/Dockerfile)
docker build -f App/Dockerfile -t identity .
```

There are no test projects in the solution yet.

Local dev URLs (from `App/Properties/launchSettings.json`): `http://localhost:5285` (http profile) / `https://localhost:7175` (https profile).
