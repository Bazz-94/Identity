# Standards

## Project Standards
- The project adheres to CLEAN architecture principles, with a clear separation of concerns between the Domain, Application, Infrastructure, and API layers.
- The project is structured as follows:
  ```
  Identity.sln
  └── src
      ├── Domain            // contains the core domain logic, entities, and value objects.
      ├── App               // contains application services, commands, and queries.
      ├── Infrastructure    // contains infrastructure concerns like data access, external service integrations.
      ├── Api               // contains the API controllers and related code, as well as the startup project.
      └── Client            // contains the client-side code.
  ```

## C# Standards

- No `var` — always explicit types.
- Use `this.` for instance members; always use block bodies for methods.
- Stateful types expose private setters plus explicit state-transition methods (e.g. a `Player` with a private-set `Health` mutated only via `TakeDamage`/`Heal`, never by setting the field directly).
- Avoid single-use local variables — inline the expression instead.
- Define constants or Enums for meaningful values instead of hardcoding them; never hardcode string values. Exception: seed data values don't need constants unless the same value is duplicated elsewhere.
- Avoid redundant words in names (`GameLoop`, not `RiverRunGameLoop`, when context is already clear).
- Prefer `foreach` over `for`.
- Provide concise XML doc comments on all classes, methods, and properties.
- Unit test all Domain logic; Application and Infrastructure code may be untested or covered by integration tests only.